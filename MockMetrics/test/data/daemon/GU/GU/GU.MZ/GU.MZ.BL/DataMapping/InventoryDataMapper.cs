using System;
using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.DataMapping
{
    public class InventoryDataMapper : AbstractDataMapper<DocumentInventory>
    {
        public InventoryDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override DocumentInventory RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var inventory = dbManager.RetrieveDomainObject<DocumentInventory>(id);

            var documentIds =
                dbManager.GetDomainTable<ProvidedDocument>()
                         .Where(t => t.DocumentInventoryId == inventory.Id)
                         .Select(t => t.Id)
                         .ToList();

            foreach (var documentId in documentIds)
            {
                var doc = dbManager.RetrieveDomainObject<ProvidedDocument>(documentId);
                inventory.ProvidedDocumentList.Add(doc);
                doc.DocumentInventory = inventory;
            }

            return inventory;
        }

        protected override DocumentInventory SaveOperation(DocumentInventory obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            dbManager.SaveDomainObject(tmp);
            tmp.Id = obj.Id;

            foreach (var document in tmp.ProvidedDocumentList)
            {
                document.DocumentInventoryId = tmp.Id;
                dbManager.SaveDomainObject(document);
            }

            if (tmp.ProvidedDocumentList.DelItems != null)
            {
                foreach (var delItem in tmp.ProvidedDocumentList.DelItems.Cast<ProvidedDocument>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }

            return tmp;
        }

        protected override void FillAssociationsOperation(DocumentInventory obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}