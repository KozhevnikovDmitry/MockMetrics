using System;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.BL.DataMapping
{
    public class QueuePrivRegDataMapping : AbstractDataMapper<QueuePrivReg>
    {
        public QueuePrivRegDataMapping(IDomainContext domainContext) 
            : base(domainContext)
        {
        }

        protected override QueuePrivReg RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<QueuePrivReg>(id);

            obj.QpBaseRegType = dbManager.RetrieveDomainObject<QueuePrivBaseRegType>(obj.QpBaseRegTypeId);

            return obj;
        }

        protected override QueuePrivReg SaveOperation(QueuePrivReg obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var qprClone = obj.Clone();

            dbManager.SaveDomainObject(qprClone);

            qprClone.AcceptChanges();
            return qprClone;
        }

        protected override void FillAssociationsOperation(QueuePrivReg obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
