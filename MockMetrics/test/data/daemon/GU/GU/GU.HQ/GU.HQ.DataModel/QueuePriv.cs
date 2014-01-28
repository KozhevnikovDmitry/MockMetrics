using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Информация о внеочередном получении жилья
    /// </summary>
    [TableName("gu_hq.q_priv")]
    public abstract class QueuePriv : IdentityDomainObject<QueuePriv>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.q_priv_seq")]
        [MapField("qp_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Идентификатор заявления
        /// </summary>
        [MapField("claim_id")]
        public abstract int ClaimId { get; set; }

        /// <summary>
        /// Итдентификатор объекта "Регистрация в очереди внеочередников"
        /// </summary>
        [MapField("qp_reg_id")]
        public abstract int QueuePrivRegId { get; set; }

        /// <summary>
        /// решения и основание о включении в список на внеочередное предоставление жилого помещения
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "QueuePrivId", CanBeNull = true)]
        public abstract QueuePrivReg QueuePrivReg { get; set; }

        /// <summary>
        /// Идентификатор объекта "Исключение из очереди внеочередников"
        /// </summary>
        [MapField("qp_dereg_id")]
        public abstract int? QueuePrivDeRegId { get; set; }

        /// <summary>
        /// решение и основание решения, утраты права на внеочередное предоставление жилого помещения
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "QueuePrivId", CanBeNull = true)]
        public abstract QueuePrivDeReg QueuePrivDeReg { get; set; }
    }
}
