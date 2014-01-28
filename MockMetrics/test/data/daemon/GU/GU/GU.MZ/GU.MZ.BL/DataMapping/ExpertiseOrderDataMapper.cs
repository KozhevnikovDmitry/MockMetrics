using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.DataMapping
{
    public class ExpertiseOrderDataMapper : AbstractDataMapper<ExpertiseOrder>
    {
        public ExpertiseOrderDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override ExpertiseOrder RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var order = dbManager.RetrieveDomainObject<ExpertiseOrder>(id);

            order.ExpertiseOrderAgreeList =
                new EditableList<ExpertiseOrderAgree>(
                    dbManager.GetDomainTable<ExpertiseOrderAgree>()
                             .Where(t => t.ExpertiseOrderId == order.Id)
                             .Select(t => t.Id)
                             .ToList()
                             .Select(t => dbManager.RetrieveDomainObject<ExpertiseOrderAgree>(t))
                             .ToList());

            order.ExpertiseHolderAddressList =
                new EditableList<ExpertiseHolderAddress>(
                    dbManager.GetDomainTable<ExpertiseHolderAddress>()
                             .Where(t => t.ExpertiseOrderId == order.Id)
                             .Select(t => t.Id)
                             .ToList()
                             .Select(t => dbManager.RetrieveDomainObject<ExpertiseHolderAddress>(t))
                             .ToList());

            return order;
        }

        protected override ExpertiseOrder SaveOperation(ExpertiseOrder obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            int id = tmp.Id;
            dbManager.SaveDomainObject(tmp);
            tmp.Id = id;

            foreach (ExpertiseOrderAgree t in tmp.ExpertiseOrderAgreeList)
            {
                t.ExpertiseOrderId = tmp.Id;
                dbManager.SaveDomainObject(t);
            }

            if (tmp.ExpertiseOrderAgreeList.DelItems != null)
            {
                foreach (var delItem in tmp.ExpertiseOrderAgreeList.DelItems.Cast<ExpertiseOrderAgree>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }

            foreach (ExpertiseHolderAddress t in tmp.ExpertiseHolderAddressList)
            {
                t.ExpertiseOrderId = tmp.Id;
                dbManager.SaveDomainObject(t);
            }

            if (tmp.ExpertiseHolderAddressList.DelItems != null)
            {
                foreach (var delItem in tmp.ExpertiseHolderAddressList.DelItems.Cast<ExpertiseHolderAddress>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }


            return tmp;
        }

        protected override void DeleteOperation(ExpertiseOrder obj, IDomainDbManager dbManager)
        {
            var tmp = obj.Clone();

            foreach (var expertiseHolderAddress in tmp.ExpertiseHolderAddressList)
            {
                expertiseHolderAddress.MarkDeleted();
                dbManager.SaveDomainObject(expertiseHolderAddress);
            }

            foreach (var expertiseOrderAgree in tmp.ExpertiseOrderAgreeList)
            {
                expertiseOrderAgree.MarkDeleted();
                dbManager.SaveDomainObject(expertiseOrderAgree);
            }

            base.DeleteOperation(obj, dbManager);
        }

        protected override void FillAssociationsOperation(ExpertiseOrder obj, IDomainDbManager dbManager)
        {
            
        }
    }
}
