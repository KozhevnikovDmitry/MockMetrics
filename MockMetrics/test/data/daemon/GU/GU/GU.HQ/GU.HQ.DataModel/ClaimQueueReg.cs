
using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel.Types;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Информация о принятии решения о постановке в очередь. 
    /// </summary>
    [TableName("gu_hq.claim_queue_reg")]
    public abstract class ClaimQueueReg : IdentityDomainObject<ClaimQueueReg>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор заявления, идентификатор решения
        /// </summary>
        [PrimaryKey]
        [MapField("claim_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Идентификатор причины регистрации
        /// </summary>
        [MapField("q_base_reg_type_id")]
        public abstract int? QueueBaseRegTypeId { get; set; }

        
        /// <summary>
        /// Описание основания регистрации
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "QueueBaseRegistration", OtherKey = "Id", CanBeNull = false)]
        public abstract QueueBaseRegType BaseReg { get; set; }
        

        /// <summary>
        /// Дата решения о принятии на учет (57)
        /// </summary>
        [MapField("doc_date")]
        public abstract DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер решения о принятии на учет (58)
        /// </summary>
        [MapField("doc_num")]
        public abstract string DocumetNumber { get; set; }

        /// <summary>
        /// Регистрационный номер решения органа, осуществляющего принятие на учет (администрация района) (60)
        /// </summary>
        [MapField("area_reg_num")]
        public abstract string AreaRegistrationNumber { get; set; }

    }
}
