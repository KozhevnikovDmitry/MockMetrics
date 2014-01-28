using System;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.DataModel;
using GU.BL.Extensions;

namespace GU.BL.DataMapping
{
    public class ContentDataMapper : AbstractDataMapper<Content>
    {
        private readonly IDictionaryManager _dictionaryManager;
        // private readonly IDomainDataMapper<ContentNode> _contentNodeDataMapper;

        public ContentDataMapper(IDomainContext domainContext, IDictionaryManager dictionaryManager)
            : base(domainContext)
        {
            _dictionaryManager = dictionaryManager;
        }

        protected override Content RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var data = dbManager.RetrieveDomainObject<Content>(id);

            data.Spec = _dictionaryManager.GetDictionaryItem<Spec>(data.SpecId);

            var dataElements = dbManager
                .GetDomainTable<ContentNode>()
                .Where(t => t.ContentId == data.Id)
                .Select(t => t.Id)
                .ToList()
                .Select(t => dbManager.RetrieveDomainObject<ContentNode>(t))
                .ToDictionary(t => t.Id);
            
            data.RootContentNodes = new EditableList<ContentNode>(dataElements.Values.Where(t => !t.ParentContentNodeId.HasValue).ToList());

            //заполнение ассоциаций
            foreach (var de in dataElements.Values)
            {
                de.Content = data;
                de.SpecNode = _dictionaryManager.GetDictionaryItem<SpecNode>(de.SpecNodeId);
                if (de.ParentContentNodeId.HasValue)
                {
                    de.ParentContentNode = dataElements[de.ParentContentNodeId.Value];
                    if (de.ParentContentNode.ChildContentNodes == null)
                        de.ParentContentNode.ChildContentNodes = new EditableList<ContentNode>();
                    de.ParentContentNode.ChildContentNodes.Add(de);
                }

            }

            return data;
        }

        private void SaveContentNodeList(EditableList<ContentNode> contentNodes, ContentNode parentContentNode, IDomainDbManager dbManager, bool forceSave = false)
        {
            //var clonedDe = de.Clone();

            //dbManager.SaveDomainObject<ContentNode>(clonedDe);

            if (contentNodes.DelItems != null)
                {
                    foreach (var childDe in contentNodes.DelItems.Cast<ContentNode>())
                    {
                        DeleteNodeOperation(childDe, dbManager);
                    }
                }

            for (int i = 0; i < contentNodes.Count; i++)
            {
                // меняем ссылку на родителя, так как объект склонирован
                contentNodes[i].ParentContentNode = parentContentNode;
                
                //валидация идентификаторов ассоциаций
                contentNodes[i].ParentContentNodeId = contentNodes[i].ParentContentNode == null ? (int?)null : contentNodes[i].ParentContentNode.Id;
                contentNodes[i].ContentId = contentNodes[i].Content.Id;
                contentNodes[i].SpecNodeId = contentNodes[i].SpecNode.Id;

                dbManager.SaveDomainObject<ContentNode>(contentNodes[i], forceSave);

                if (contentNodes[i].ChildContentNodes != null)
                    this.SaveContentNodeList(contentNodes[i].ChildContentNodes, contentNodes[i], dbManager, forceSave);
            }
        }

        protected override Content SaveOperation(Content obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var clonedData = obj.Clone();
            clonedData.SpecId = clonedData.Spec.Id;
            
            var dataElements = clonedData
                .RootContentNodes
                .SelectMany(de => de.Descendants(t => t.ChildContentNodes, true));
            
            foreach (var de in dataElements)
            {
                de.Content = clonedData;
                de.ContentId = clonedData.Id;
            }
            dbManager.SaveDomainObject<Content>(clonedData);

            this.SaveContentNodeList(clonedData.RootContentNodes, null, dbManager, forceSave);

            return clonedData;

            /*
            if (tmp.DataElements != null)
            {
                foreach (var dataElement in tmp.DataElements)
                {
                    dataElement.Content = tmp;
                    //установка id для ассоциаций
                    dataElement.ContentId = dataElement.Content.Id;
                    dataElement.SpecNodeId = dataElement.SpecNode.Id;
                    dataElement.ParentContentNodeId = dataElement.ParentContentNode == null ? (int?) null : dataElement.ParentContentNode.Id;
                }

                var rootDataElements = tmp.DataElements.Where(t => !t.ParentContentNodeId.HasValue).ToList();
                // список элементов в порядке обхода с корня дерева - чтобы дочерние элементы не вставлялись раньше родителя
                var contentNodes = rootDataElements.SelectMany(r => r.Descendants(t => t.ChildContentNodes, true)).ToList();

                for (var i = 0; i < contentNodes.Count; i++)
                {
                    var de = tmp.DataElements[i];
                    de = de.Clone();
                    dbManager.SaveDomainObject(de);
                    tmp.DataElements[i] = de;
                    if (de.ParentContentNode != null)
                    {
                        de.ParentContentNode.ChildContentNodes.
                    }
                }
            }
            return tmp;*/
        }

        protected override void DeleteOperation(Content obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
            /*
            var tmp = obj.Clone();
            tmp.MarkDeleted();
            foreach (var dataElement in tmp.DataElements)
            {
                //TODO: где-то есть clone при делете, где-то нету...
                dataElement.MarkDeleted();
                dbManager.SaveDomainObject(dataElement);
            }
            dbManager.SaveDomainObject(tmp);
            */
        }

        private void DeleteNodeOperation(ContentNode contentNode, IDomainDbManager dbManager)
        {
            if (contentNode.ChildContentNodes != null)
            {
                foreach (var childNode in contentNode.ChildContentNodes)
                {
                    DeleteNodeOperation(childNode, dbManager);
                }
            }
            contentNode.MarkDeleted();
            dbManager.SaveDomainObject<ContentNode>(contentNode);
        }

        protected override void FillAssociationsOperation(Content obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
