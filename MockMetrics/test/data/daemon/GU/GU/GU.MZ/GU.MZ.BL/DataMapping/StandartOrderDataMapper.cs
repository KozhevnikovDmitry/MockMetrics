using System;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.DataMapping
{
    public class StandartOrderDataMapper : AbstractDataMapper<StandartOrder>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public StandartOrderDataMapper(IDomainContext domainContext, IDictionaryManager dictionaryManager)
            : base(domainContext)
        {
            _dictionaryManager = dictionaryManager;
        }

        protected override StandartOrder RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var order = dbManager.RetrieveDomainObject<StandartOrder>(id);

            order.AgreeList
               = new EditableList<StandartOrderAgree>(dbManager.GetDomainTable<StandartOrderAgree>()
                                                              .Where(t => t.OrderId == order.Id)
                                                              .Select(t => t.Id)
                                                              .ToList()
                                                              .Select(t => dbManager.RetrieveDomainObject<StandartOrderAgree>(t))
                                                              .ToList());

            order.DetailList
                = new EditableList<StandartOrderDetail>(dbManager.GetDomainTable<StandartOrderDetail>()
                                                               .Where(t => t.OrderId == order.Id)
                                                               .Select(t => t.Id)
                                                               .ToList()
                                                               .Select(t => dbManager.RetrieveDomainObject<StandartOrderDetail>(t))
                                                               .ToList());

            foreach (var standartOrderDetail in order.DetailList)
            {
                standartOrderDetail.StandartOrder = order;
            }

            order.OrderOption = _dictionaryManager.GetDictionaryItem<StandartOrderOption>(order.OrderOptionId);

            return order;
        }

        protected override StandartOrder SaveOperation(StandartOrder obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();
            dbManager.SaveDomainObject(tmp);

            foreach (StandartOrderAgree agree in tmp.AgreeList)
            {
                agree.OrderId = tmp.Id;
                dbManager.SaveDomainObject(agree);
            }


            if (tmp.AgreeList.DelItems != null)
            {
                foreach (var delItem in tmp.AgreeList.DelItems.Cast<StandartOrderAgree>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }

            foreach (StandartOrderDetail detail in tmp.DetailList)
            {
                detail.OrderId = tmp.Id;
                dbManager.SaveDomainObject(detail);
                detail.StandartOrder = tmp;
            }


            if (tmp.DetailList.DelItems != null)
            {
                foreach (var delItem in tmp.DetailList.DelItems.Cast<StandartOrderDetail>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }

            return tmp;
        }

        protected override void DeleteOperation(StandartOrder obj, IDomainDbManager dbManager)
        {
            var tmp = obj.Clone();

            foreach (var standartOrderDetail in tmp.DetailList)
            {
                standartOrderDetail.MarkDeleted();
                dbManager.SaveDomainObject(standartOrderDetail);
            }

            foreach (var standartOrderAgree in tmp.AgreeList)
            {
                standartOrderAgree.MarkDeleted();
                dbManager.SaveDomainObject(standartOrderAgree);
            }

            base.DeleteOperation(obj, dbManager);
        }

        protected override void FillAssociationsOperation(StandartOrder obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}