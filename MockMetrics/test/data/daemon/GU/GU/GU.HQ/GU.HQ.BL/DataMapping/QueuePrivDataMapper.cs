using System;
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DataMapping
{
    public class QueuePrivDataMapper : AbstractDataMapper<QueuePriv>
    {
        public QueuePrivDataMapper(IDomainContext domainContext) 
            : base(domainContext)
        {
        }

        protected override QueuePriv RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<QueuePriv>(id);

            // основание и решение о регистрации
            obj.QueuePrivReg = dbManager.RetrieveDomainObject<QueuePrivReg>(obj.QueuePrivRegId);

            // основание и решение о снятие с регистрации
            if(obj.QueuePrivDeRegId != null)
                obj.QueuePrivDeReg = dbManager.RetrieveDomainObject<QueuePrivDeReg>(obj.QueuePrivDeRegId);

            return obj;
        }

        protected override QueuePriv SaveOperation(QueuePriv obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            throw new NotImplementedException();
        }

        protected override void FillAssociationsOperation(QueuePriv obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
