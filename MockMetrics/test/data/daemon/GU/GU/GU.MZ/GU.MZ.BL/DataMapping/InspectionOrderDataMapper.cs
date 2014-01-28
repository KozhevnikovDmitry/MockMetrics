using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.DataMapping
{
    public class InspectionOrderDataMapper : AbstractDataMapper<InspectionOrder>
    {
        public InspectionOrderDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override InspectionOrder RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var order = dbManager.RetrieveDomainObject<InspectionOrder>(id);

            order.InspectionOrderAgreeList =
                new EditableList<InspectionOrderAgree>(
                    dbManager.GetDomainTable<InspectionOrderAgree>()
                             .Where(t => t.InspectionOrderId == order.Id)
                             .Select(t => t.Id)
                             .ToList()
                             .Select(t => dbManager.RetrieveDomainObject<InspectionOrderAgree>(t))
                             .ToList());

            order.InspectionOrderExpertList =
                new EditableList<InspectionOrderExpert>(
                    dbManager.GetDomainTable<InspectionOrderExpert>()
                             .Where(t => t.InspectionOrderId == order.Id)
                             .Select(t => t.Id)
                             .ToList()
                             .Select(t => dbManager.RetrieveDomainObject<InspectionOrderExpert>(t))
                             .ToList());

            order.InspectionHolderAddressList =
                new EditableList<InspectionHolderAddress>(
                    dbManager.GetDomainTable<InspectionHolderAddress>()
                             .Where(t => t.InspectionOrderId == order.Id)
                             .Select(t => t.Id)
                             .ToList()
                             .Select(t => dbManager.RetrieveDomainObject<InspectionHolderAddress>(t))
                             .ToList());

            return order;
        }

        protected override InspectionOrder SaveOperation(InspectionOrder obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            int id = tmp.Id;
            dbManager.SaveDomainObject(tmp);
            tmp.Id = id;

            foreach (InspectionOrderAgree t in tmp.InspectionOrderAgreeList)
            {
                t.InspectionOrderId = tmp.Id;
                dbManager.SaveDomainObject(t);
            }

            if (tmp.InspectionOrderAgreeList.DelItems != null)
            {
                foreach (var delItem in tmp.InspectionOrderAgreeList.DelItems.Cast<InspectionOrderAgree>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }

            foreach (InspectionOrderExpert t in tmp.InspectionOrderExpertList)
            {
                t.InspectionOrderId = tmp.Id;
                dbManager.SaveDomainObject(t);
            }

            if (tmp.InspectionOrderExpertList.DelItems != null)
            {
                foreach (var delItem in tmp.InspectionOrderExpertList.DelItems.Cast<InspectionOrderExpert>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }

            foreach (InspectionHolderAddress t in tmp.InspectionHolderAddressList)
            {
                t.InspectionOrderId = tmp.Id;
                dbManager.SaveDomainObject(t);
            }

            if (tmp.InspectionHolderAddressList.DelItems != null)
            {
                foreach (var delItem in tmp.InspectionHolderAddressList.DelItems.Cast<InspectionHolderAddress>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }


            return tmp;
        }

        protected override void DeleteOperation(InspectionOrder obj, IDomainDbManager dbManager)
        {
            var tmp = obj.Clone();

            foreach (var inspectionHolderAddress in tmp.InspectionHolderAddressList)
            {
                inspectionHolderAddress.MarkDeleted();
                dbManager.SaveDomainObject(inspectionHolderAddress);
            }

            foreach (var inspectionOrderAgree in tmp.InspectionOrderAgreeList)
            {
                inspectionOrderAgree.MarkDeleted();
                dbManager.SaveDomainObject(inspectionOrderAgree);
            }

            foreach (var inspectionOrderExpert in tmp.InspectionOrderExpertList)
            {
                inspectionOrderExpert.MarkDeleted();
                dbManager.SaveDomainObject(inspectionOrderExpert);
            }

            base.DeleteOperation(obj, dbManager);
        }

        protected override void FillAssociationsOperation(InspectionOrder obj, IDomainDbManager dbManager)
        {
            
        }
    }
}
