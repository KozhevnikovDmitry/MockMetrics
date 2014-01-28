using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using BLToolkit.EditableObjects;
using Common.BL.Exceptions;
using Common.BL.Validation;

using GU.BL.Extensions;
using GU.BL.Policy.Interface;
using GU.DataModel;

namespace GU.BL.Policy
{
    public class ContentPolicy : IContentPolicy
    {
        #region Create

        
        public Content CreateEmpty(Spec spec)
        {
            var content = Content.CreateInstance();
            content.Spec = spec;
            content.RootContentNodes = new EditableList<ContentNode>();
            return content;
        }
        
        public Content CreateDefault(Spec spec)
        {
            var content = CreateEmpty(spec);
            
            var nodes = CreateDefaultNodes(spec);
            // рекурсивное проставление контента для созданных узлов
            var allNodes = nodes.SelectMany(n => n.Descendants(t => t.ChildContentNodes, true)).ToList();
            foreach (var node in allNodes)
                node.Content = content;
            
            content.RootContentNodes.AddRange(nodes);

            return content;
        }

        private List<ContentNode> CreateDefaultNodes(Spec spec)
        {
            var nodes = new List<ContentNode>();
            foreach (var specNode in spec.RootSpecNodes)
            {
                int minimumOccurence = 0;

                // для простых необязательных нод всегда создаётся дефолтная контент нода
                if (specNode.SpecNodeType == SpecNodeType.Simple 
                    && specNode.IsSingleOrNull)
                {
                    minimumOccurence = -1;
                }

                for (int i = minimumOccurence; i < specNode.MinOccurs; i++)
                {
                    var node = CreateDefaultNode(specNode);
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        public ContentNode CreateEmptyNode(SpecNode specNode)
        {
            var node = ContentNode.CreateInstance();
            node.SpecNode = specNode;
            node.SpecNodeId = specNode.Id;
            return node;
        }
        
        public ContentNode CreateDefaultNode(SpecNode specNode)
        {
            var node = CreateEmptyNode(specNode);

            if (specNode.SpecNodeType == SpecNodeType.Complex)
            {
                foreach (var childElement in specNode.ChildSpecNodes)
                {
                    var childNode = CreateDefaultNode(childElement);
                    node.ChildContentNodes.Add(childNode);
                    childNode.ParentContentNode = node;
                }
            }
            else if (specNode.SpecNodeType == SpecNodeType.ComplexChoice)
            {
                // для choice в качестве значения по умолчанию выбираем первого потомка
                var childSpecNode = specNode.ChildSpecNodes.FirstOrDefault();
                var childNode = CreateDefaultNode(childSpecNode);
                node.ChildContentNodes.Add(childNode);
                childNode.ParentContentNode = node;
            }
            else if (specNode.SpecNodeType == SpecNodeType.RefSpec)
            {
                var childNodes = CreateDefaultNodes(specNode.RefSpec);
                foreach (var childNode in childNodes)
                {
                    node.ChildContentNodes.Add(childNode);
                    childNode.ParentContentNode = node;
                }
            }
            else if (specNode.SpecNodeType == SpecNodeType.Simple)
            {
                //NOTE: взято из TaskAttrPolicy, не совсем понятно зачем это
                if (specNode.AttrDataType == AttrDataType.List)
                {
                    node.StrValue = null;
                }
            }
            else
            {
                throw new DomainBLLException("unknown SpecNodeType", specNode);
            }

            return node;
        }

        #endregion

        #region Add/Remove

        public ContentNode AddChildNode(ContentNode node, SpecNode childSpecNode, int? nodeNum = null)
        {
            var childNode = CreateDefaultNode(childSpecNode);
            AddChildNode(node, childNode, nodeNum);
            return childNode;
        }

        public void AddChildNode(ContentNode node, ContentNode childNode, int? nodeNum = null)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (childNode == null) throw new ArgumentNullException("childNode");

            if (childNode.Content != null)
                throw new DomainBLLException("childNode already attached to content", childNode, childNode.Content);
            if (childNode.ParentContentNode != null)
                throw new DomainBLLException("childNode already attached to parent node", childNode, childNode.ParentContentNode);

            var allowedSpecNodes = new List<SpecNode>(node.SpecNode.ChildSpecNodes);
            if (node.SpecNode.RefSpec != null)
                allowedSpecNodes.AddRange(node.SpecNode.RefSpec.RootSpecNodes);

            if (allowedSpecNodes.All(t => t.Id != childNode.SpecNode.Id))
                throw new DomainBLLException("childNode.SpecNode is not child of node.SpecNode", node, childNode, node.SpecNode, childNode.SpecNode);

            var relatedNodes = node.ChildContentNodes.Where(t => t.SpecNode.Id == childNode.SpecNode.Id).ToList();
            
            if (relatedNodes.Count >= childNode.SpecNode.MaxOccurs)
                throw new DomainBLLException(string.Format("Количество элементов типа {0} больше максимального ({1})", childNode.SpecNode.Name, childNode.SpecNode.MaxOccurs), node, childNode, childNode.SpecNode);

            if (nodeNum != null)
                throw new NotImplementedException("nodeNum != null not implemented");

            node.ChildContentNodes.Add(childNode);
            childNode.ParentContentNode = node;
        }

        public void RemoveNode(ContentNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            
            if (node.ParentContentNode == null) 
                throw new DomainBLLException("Нельзя удалить корневой узел", node);

            if (!node.ParentContentNode.ChildContentNodes.Remove(node))
                throw new DomainBLLException("Ошибка удаления узла: узел не найден в списке потомков родителя", node);
            node.ParentContentNode = null;
            node.Content = null;

            //TODO: сдвинуть nodeNum у остальных узлов
        }

        #endregion

        #region Choice

        public void SwitchChoice(ContentNode node, SpecNode childSpecNode)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (childSpecNode == null) throw new ArgumentNullException("childSpecNode");

            if (node.SpecNode.SpecNodeType != SpecNodeType.ComplexChoice)
                throw new DomainBLLException("node is not choice", node, node.SpecNode);

            if (node.SpecNode.ChildSpecNodes.All(t => t.Id != childSpecNode.Id))
                throw new DomainBLLException("childSpecNode is not child of node.SpecNode", node, node.SpecNode, childSpecNode);

            //если для чойса выбран другой элемент, то удаляем потомков и создаем новый дочерний элемент
            if (node.ChildContentNodes.Count > 1 || node.ChildContentNodes.First().SpecNode.Id != childSpecNode.Id)
            {
                node.ChildContentNodes.Clear();
                AddChildNode(node, childSpecNode);
            }
        }

        #endregion

        #region Validattion

        public ValidationErrorInfo Validate(Content content)
        {
            //TODO: переписать валидацию на обход через спеку
            
            var result = new ValidationErrorInfo();

            foreach (var childContentNode in content.RootContentNodes.OrderBy(t => t.SpecNode.Order))
            {
                result.AddResult(Validate(childContentNode));
            }

            return result;
        }

        public ValidationErrorInfo Validate(ContentNode contentNode)
        {
            var result = new ValidationErrorInfo();

            if (contentNode.SpecNode.SpecNodeType != SpecNodeType.Simple)
            {
                foreach (var childContentNode in contentNode.ChildContentNodes.OrderBy(t => t.SpecNode.Order))
                {
                    result.AddResult(Validate(childContentNode));
                }

                return result;
            }

            if (HasValue(contentNode))
            {
                if (!IsMatchRegexp(contentNode))
                {
                    string errorInfo = string.Format("Ошибка заполнения поля \"{0}\"", contentNode.SpecNode.Name);
                    if (!string.IsNullOrWhiteSpace(contentNode.SpecNode.FormatDescription))
                        errorInfo = string.Format("{0}: {1}", errorInfo, contentNode.SpecNode.FormatDescription);
                    result.AddError(errorInfo);
                }
            }
            else
            {
                if (contentNode.SpecNode.MinOccurs > 0)
                    return new ValidationErrorInfo(String.Format("Поле \"{0}\" не заполнено", contentNode.SpecNode.Name));
            }

            return result;
        }

        public bool HasValue(ContentNode contentNode)
        {
            if (!contentNode.SpecNode.AttrDataType.HasValue)
            {
                throw new DomainBLLException("Для фрагмента контента не задан тип значения", contentNode);
            }

            switch (contentNode.SpecNode.AttrDataType)
            {
                    case AttrDataType.String:
                    {
                        return !string.IsNullOrWhiteSpace(contentNode.StrValue);
                    }
                    case AttrDataType.Number:
                    {
                        return contentNode.NumValue.HasValue;
                    }
                    case AttrDataType.Boolean:
                    {
                        return contentNode.BoolValue.HasValue;
                    }
                    case AttrDataType.Date:
                    {
                        return contentNode.DateValue.HasValue;
                    }
                    case AttrDataType.List:
                    {
                        return !string.IsNullOrWhiteSpace(contentNode.StrValue);
                    }
                    case AttrDataType.File:
                    {
                        return contentNode.BlobValue != null && !string.IsNullOrEmpty(contentNode.BlobName);
                    }
            }

            throw new DomainBLLException("Тип значения контента не поддерживается", contentNode);
        }

        private bool IsMatchRegexp(ContentNode contentNode)
        {
            if (!string.IsNullOrEmpty(contentNode.SpecNode.FormatRegexp))
            {
                switch (contentNode.SpecNode.AttrDataType)
                {
                    case AttrDataType.String:
                        return Regex.IsMatch(contentNode.StrValue, contentNode.SpecNode.FormatRegexp);
                }
            }

            return true;
        }

        #endregion

    }
}
