using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel.Claim;

namespace GU.HQ.BL.DataMapping
{
    public class ClaimDeclarerBaseRegDataMapper : AbstractDataMapper<ClaimDeclarerBaseReg> 
    {

        public ClaimDeclarerBaseRegDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        protected override ClaimDeclarerBaseReg RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<ClaimDeclarerBaseReg>(id);

            var lst = (from i in dbManager.GetDomainTable<ClaimDeclarerBaseRegItem>()
                       where i.ClaimDecBaseRegId == obj.Id
                       select i.Id).Distinct().ToList().
           Select(val => dbManager.RetrieveDomainObject<ClaimDeclarerBaseRegItem>(val)).ToList();

            obj.BaseRegItems = new EditableList<ClaimDeclarerBaseRegItem>(lst);

            if (obj.BaseRegItems != null && obj.BaseRegItems.Count != 0)
                foreach (var items in obj.BaseRegItems)
                    items.QueueBaseReg = dbManager.RetrieveDomainObject<QueueBaseReg>(items.QueueBaseRegId);
            
            return obj;
        }

        /// <summary>
        /// Сохранение информации об основании учета
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        /// <param name="forceSave"></param>
        /// <returns></returns>
        protected override ClaimDeclarerBaseReg SaveOperation(ClaimDeclarerBaseReg obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var drbClone = obj.Clone();
            dbManager.BeginDomainTransaction();
            dbManager.SaveDomainObject(drbClone);

            // сохраняем список оснований учета
            if (drbClone.BaseRegItems != null)
                foreach (var item in drbClone.BaseRegItems)
                {
                    item.ClaimDecBaseRegId = drbClone.Id;
                    dbManager.SaveDomainObject(item);
                }

            dbManager.CommitDomainTransaction();

            return drbClone;
        }

        protected override void FillAssociationsOperation(ClaimDeclarerBaseReg obj, IDomainDbManager dbManager)
        {
            throw new System.NotImplementedException();
        }
    }
}
