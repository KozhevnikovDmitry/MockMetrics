
using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Уведомления о постановке на учет
    /// </summary>
    [TableName("gu_hq.notice")]
    public abstract class Notice : IdentityDomainObject<Notice>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.notice_seq")]
        [MapField("notice_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Идентификатор заявления
        /// </summary>
        [MapField("claim_id")]
        public abstract int ClaimId { get; set; }

        /// <summary>
        /// Дата уведомления
        /// </summary>
        [MapField("doc_date")]
        public abstract DateTime DocumentDate { get; set; }

        /// <summary>
        /// Номер уведомления
        /// </summary>
        [MapField("doc_num")]
        public abstract string DocumentNumber { get; set; }
    }
}
