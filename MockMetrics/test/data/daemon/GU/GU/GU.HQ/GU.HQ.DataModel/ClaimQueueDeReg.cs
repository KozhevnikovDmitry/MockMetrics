using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel.Types;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Информация о снятии заявления с учета
    /// </summary>
    [TableName("gu_hq.claim_queue_dereg")]
    public abstract class ClaimQueueDeReg : IdentityDomainObject<ClaimQueueDeReg>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.claim_queue_dereg_seq")]
        [MapField("claim_queue_dereg_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Идентифиатор заявления
        /// </summary>
        [MapField("claim_id")]
        public abstract int ClaimId { get; set; }

        /// <summary>
        /// Идентификатор причины снятия с регистраци
        /// </summary>
        [MapField("q_base_dereg_type_id")]
        public abstract int QueueBaseDeRegTypeId { get; set; }

        /// <summary>
        /// Причина снятия с регистраци
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "QueueBaseDeRegistration", OtherKey = "Id", CanBeNull = false)]
        public abstract QueueBaseDeRegType QueueBaseDeRegType { get; set; }

        /// <summary>
        /// Дата документа снятия с регистраци
        /// </summary>
        [MapField("doc_date")]
        public abstract DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа снятия с регистрации
        /// </summary>
        [MapField("doc_num")]
        public abstract string DocumentNum { get; set; }
    }
}
