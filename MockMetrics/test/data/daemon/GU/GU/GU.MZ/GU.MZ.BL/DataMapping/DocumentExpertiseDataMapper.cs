using System;
using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.MZ.DataModel.Inspect;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер сущностей Документарная проверка
    /// </summary>
    public class DocumentExpertiseDataMapper : AbstractDataMapper<DocumentExpertise>
    {
        /// <summary>
        /// Класс маппер сущностей Документарная проверка
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        public DocumentExpertiseDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override DocumentExpertise RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var expertise = dbManager.RetrieveDomainObject<DocumentExpertise>(id);

            expertise.ExperiseResultList =
                new EditableList<DocumentExpertiseResult>(
                    dbManager.GetDomainTable<DocumentExpertiseResult>()
                             .Where(t => t.DocumentExpertiseId == expertise.Id)
                             .Select(t => t.Id)
                             .ToList()
                             .Select(t => dbManager.RetrieveDomainObject<DocumentExpertiseResult>(t))
                             .ToList());

            return expertise;
        }

        protected override DocumentExpertise SaveOperation(DocumentExpertise obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();
            
            int id = tmp.Id;
            dbManager.SaveDomainObject(tmp);
            tmp.Id = id;

            foreach (DocumentExpertiseResult t in tmp.ExperiseResultList)
            {
                t.DocumentExpertiseId = tmp.Id;
                dbManager.SaveDomainObject(t);
            }

            if (tmp.ExperiseResultList.DelItems != null)
            {
                foreach (var delItem in tmp.ExperiseResultList.DelItems.Cast<DocumentExpertiseResult>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }

            return tmp;
        }

        protected override void DeleteOperation(DocumentExpertise obj, IDomainDbManager dbManager)
        {
            var tmp = obj.Clone();

            foreach (var t in tmp.ExperiseResultList)
            {
                t.MarkDeleted();
                dbManager.SaveDomainObject(t);
            }

            tmp.MarkDeleted();
            dbManager.SaveDomainObject(tmp);
        }

        protected override void FillAssociationsOperation(DocumentExpertise obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
