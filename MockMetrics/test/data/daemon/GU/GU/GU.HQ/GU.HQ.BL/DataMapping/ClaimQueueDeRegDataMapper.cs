using System;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DataMapping
{
    public class ClaimQueueDeRegDataMapper : AbstractDataMapper<ClaimQueueDeReg>
    {
        public ClaimQueueDeRegDataMapper(IDomainContext domainContext) 
            : base(domainContext)
        {
        }

        protected override ClaimQueueDeReg RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<ClaimQueueDeReg>(id);

            return obj;
        }

        protected override ClaimQueueDeReg SaveOperation(ClaimQueueDeReg obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var qdrClone = obj.Clone();

            dbManager.SaveDomainObject(qdrClone);

            qdrClone.AcceptChanges();
            return qdrClone;
        }

        protected override void FillAssociationsOperation(ClaimQueueDeReg obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
