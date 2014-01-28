using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.Types.Exceptions;
using GU.BL.Import.ImportException;
using GU.DataModel;

namespace GU.BL.Import
{
    /// <summary>
    /// Класс, занимающийся импортом сырых данных из заявок.
    /// </summary>
    public class ContentImporter : IContentImporter
    {
        private readonly IDictionaryManager _dictionaryManager;

        public ContentImporter(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        public ContentNode GetContentNode(Content content, string path)
        {
            try
            {
                return GetContentNode(content.RootContentNodes, path);
            }
            catch (ImportTagException ex)
            {
                throw new ImportTagException(content, ex);
            }
        }

        public ContentNode GetContentNode(ContentNode contentNode, string path)
        {
            try
            {
                return GetContentNode(contentNode.ChildContentNodes, path);
            }
            catch (ImportTagException ex)
            {
                throw new ImportTagException(contentNode, ex);
            }
        }

        public IList<ContentNode> GetContentNodes(Content content, string path)
        {
            try
            {
                return GetContentNodes(content.RootContentNodes, path);
            }
            catch (ImportTagException ex)
            {
                throw new ImportTagException(content, ex);
            }
        }

        public IList<ContentNode> GetContentNodes(ContentNode contentNode, string path)
        {
            try
            {
                return GetContentNodes(contentNode.ChildContentNodes, path);
            }
            catch (ImportTagException ex)
            {
                throw new ImportTagException(contentNode, ex);
            }
        }

        public bool HasContentNode(ContentNode contentNode, string path)
        {
            try
            {
                GetContentNode(contentNode, path);
                return true;
            }
            catch (ImportTagException)
            {
                return false;
            }
        }

        public bool HasChildContentNodeEndsWith(ContentNode contentNode, string tagEnd)
        {
            return GetChildContentNodeEndsWith(contentNode, tagEnd) != null;
        }

        public bool HasChildContentNodeStartsWith(ContentNode contentNode, string tagStart)
        {
            return GetChildContentNodeStartsWith(contentNode, tagStart) != null;
        }

        public ContentNode GetChildContentNodeEndsWith(ContentNode contentNode, string tagEnd)
        {
            var nodes = contentNode.ChildContentNodes.Where(t => t.SpecNode.Tag.EndsWith(tagEnd));

            if (nodes.Count() > 1)
            {
                throw new BLLException(string.Format("Найдено более одной дочерней ветки контента у ветки Id=[{0}] по тегу [*{1}]", contentNode.Id, tagEnd));
            }

            return nodes.SingleOrDefault();
        }

        public ContentNode GetChildContentNodeStartsWith(ContentNode contentNode, string tagStart)
        {
            var nodes = contentNode.ChildContentNodes.Where(t => t.SpecNode.Tag.StartsWith(tagStart));

            if (nodes.Count() > 1)
            {
                throw new BLLException(string.Format("Найдено более одной дочерней ветки контента у ветки Id=[{0}] по тегу [{1}*]", contentNode.Id, tagStart));
            }

            return nodes.SingleOrDefault();
        }

        public string GetNodeStrValue(Content content, string path)
        {
            return GetContentNode(content.RootContentNodes, path).StrValue;
        }

        public string GetNodeStrValue(ContentNode contentNode, string path)
        {
            return GetContentNode(contentNode.ChildContentNodes, path).StrValue;
        }

        public decimal? GetNodeNumValue(Content content, string path)
        {
            return GetContentNode(content.RootContentNodes, path).NumValue;
        }

        public decimal? GetNodeNumValue(ContentNode contentNode, string path)
        {
            return GetContentNode(contentNode.ChildContentNodes, path).NumValue;
        }

        public DateTime? GetNodeDateValue(Content content, string path)
        {
            return GetContentNode(content.RootContentNodes, path).DateValue;
        }

        public DateTime? GetNodeDateValue(ContentNode contentNode, string path)
        {
            return GetContentNode(contentNode.ChildContentNodes, path).DateValue;
        }

        public bool? GetNodeBoolValue(Content content, string path)
        {
            return GetContentNode(content.RootContentNodes, path).BoolValue;
        }

        public bool? GetNodeBoolValue(ContentNode contentNode, string path)
        {
            return GetContentNode(contentNode.ChildContentNodes, path).BoolValue;
        }

        private ContentNode GetContentNode(IList<ContentNode> nodes, string path)
        {
            var parentNodes = nodes;
            var pathTokens = path.Split(new[] { '/' });
            ContentNode curNode = null;
            foreach (var token in pathTokens)
            {
                curNode = GetContentNodeByTag(parentNodes, token);
                parentNodes = curNode.ChildContentNodes;
            }
            return curNode;
        }

        private IList<ContentNode> GetContentNodes(IList<ContentNode> nodes, string path)
        {
            var parentNodes = nodes;
            var pathTokens = path.Split(new[] { '/' });
            IList<ContentNode> curNodes = new List<ContentNode>();
            foreach (var token in pathTokens)
            {
                curNodes = GetContentNodesByTag(parentNodes, token);
                parentNodes = curNodes.SelectMany(t => t.ChildContentNodes).ToList();
            }
            return curNodes;
        }

        private IList<ContentNode> GetContentNodesByTag(IList<ContentNode> nodes, string tag)
        {
            return nodes.Where(t => t.SpecNode.Tag == tag).ToList();
        }

        private ContentNode GetContentNodeByTag(IList<ContentNode> nodes, string tag)
        {
            var foundNodes = GetContentNodesByTag(nodes, tag);
            if (foundNodes.Count == 0)
            {
                throw new SingleAttrValueTagMissingException(tag);
            }
            if (foundNodes.Count > 1)
            {
                throw new TagWrongStructureException(tag);
            }
            return foundNodes[0];
        }
    }
}
