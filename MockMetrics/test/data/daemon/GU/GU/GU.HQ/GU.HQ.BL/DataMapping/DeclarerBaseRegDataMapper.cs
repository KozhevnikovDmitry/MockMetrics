using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.BL.DataMapping
{
    public class DeclarerBaseRegDataMapper : AbstractDataMapper<DeclarerBaseReg> 
    {

        public DeclarerBaseRegDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        protected override DeclarerBaseReg RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<DeclarerBaseReg>(id);

            var lst = (from i in dbManager.GetDomainTable<DeclarerBaseRegItem>()
                       where i.DeclarerBaseRegId == obj.Id
                       select i.Id).Distinct().ToList().
           Select(val => dbManager.RetrieveDomainObject<DeclarerBaseRegItem>(val)).ToList();

            obj.BaseRegItems = new EditableList<DeclarerBaseRegItem>(lst);

            if (obj.BaseRegItems != null && obj.BaseRegItems.Count != 0)
                foreach (var items in obj.BaseRegItems)
                    items.QueueBaseRegType = dbManager.RetrieveDomainObject<QueueBaseRegType>(items.QueueBaseRegTypeId);
            
            return obj;
        }

        /// <summary>
        /// Сохранение информации об основании учета
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        /// <param name="forceSave"></param>
        /// <returns></returns>
        protected override DeclarerBaseReg SaveOperation(DeclarerBaseReg obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var drbClone = obj.Clone();
            dbManager.BeginDomainTransaction();
            dbManager.SaveDomainObject(drbClone);

            // сохраняем список оснований учета
            if (drbClone.BaseRegItems != null)
                foreach (var item in drbClone.BaseRegItems)
                {
                    item.DeclarerBaseRegId = drbClone.Id;
                    dbManager.SaveDomainObject(item);
                }

            dbManager.CommitDomainTransaction();

            return drbClone;
        }

        protected override void FillAssociationsOperation(DeclarerBaseReg obj, IDomainDbManager dbManager)
        {
            throw new System.NotImplementedException();
        }
    }
}
