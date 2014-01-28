using System;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.BL.DataMapping
{
    /// <summary>
    /// информация о постановки заявления в очередь на жильё
    /// </summary>
    public class ClaimQueueRegDataMapper : AbstractDataMapper<ClaimQueueReg>
    {
        public ClaimQueueRegDataMapper(IDomainContext domainContext) : base(domainContext)
        {
        }

        /// <summary>
        /// получить объект по идентификтаору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        protected override ClaimQueueReg RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<ClaimQueueReg>(id);

            obj.BaseReg = dbManager.RetrieveDomainObject<QueueBaseRegType>(obj.QueueBaseRegTypeId);

            return obj;
        }

        /// <summary>
        /// сохранить объект
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        /// <param name="forceSave"></param>
        /// <returns></returns>
        protected override ClaimQueueReg SaveOperation(ClaimQueueReg obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var qrClone = obj.Clone();

            dbManager.SaveDomainObject(qrClone);

            qrClone.AcceptChanges();
            return qrClone;
        }

        protected override void FillAssociationsOperation(ClaimQueueReg obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
