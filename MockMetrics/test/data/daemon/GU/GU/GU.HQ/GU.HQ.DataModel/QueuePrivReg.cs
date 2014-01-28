
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
    /// решения о включении в список на внеочередное предоставление жилого помещения
    /// основания о включении в список на внеочередное предоставление жилого помещения
    /// </summary>
    [TableName("gu_hq.qp_reg")]
    public abstract class QueuePrivReg : IdentityDomainObject<QueuePrivReg>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        [PrimaryKey,Identity]
        [SequenceName("gu_hq.qp_reg_seq")]
        [MapField("qp_reg_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Идентификатор основания для регистрации в очередь внеочередников
        /// </summary>
        [MapField("qp_base_reg_type_id")]
        public abstract int? QpBaseRegTypeId { get; set; }

        /// <summary>
        /// основание регистрации в очередь внеочередников
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "QpBaseRegTypeId", OtherKey = "Id", CanBeNull = false)]
        public abstract QueuePrivBaseRegType QpBaseRegType { get; set; }

        /// <summary>
        /// Дата возникновения права на внеочередное предоставление жилого помещения (63)
        /// </summary>
        [MapField("date_law")]
        public abstract DateTime? DateLaw { get; set; }

        /// <summary>
        /// Дата решения о включении в список на внеочередное предоставление жилого помещения (64)
        /// </summary>
        [MapField("decision_date")]
        public abstract DateTime? DecisionDate { get; set; }

        /// <summary>
        /// Номер решения о включении в список на внеочередное предоставление жилого помещения (65)
        /// </summary>
        [MapField("decision_num")]
        public abstract string DecisionNum { get; set; }

        /// <summary>
        /// Дата документа - основания решения о включении в список на внеочередное предоставление жилого помещения (67)
        /// </summary>
        [MapField("doc_date")]
        public abstract DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа – основания о включении в список на внеочередное предоставление жилого помещения (68)
        /// </summary>
        [MapField("doc_num")]
        public abstract string DocumentNum { get; set; }
    }
}
