
using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Решение органа, осуществляющего постановку на учет, и основание об исключении из списка на внеочередное предоставление жилого помещения:
    /// </summary>
    [TableName("gu_hq.qp_dereg")]
    public abstract class QueuePrivDeReg : IdentityDomainObject<QueuePrivDeReg>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор жилья
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.qp_dereg_seq")]
        [MapField("qp_dereg_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Дата документа – основания утраты права на внеочередное предоставление жилого помещения (74)
        /// </summary>
        [MapField("doc_date")]
        public abstract DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа – основания утраты права на внеочередное предоставление жилого помещения (73)
        /// </summary>
        [MapField("doc_num")]
        public abstract string DocumentNum { get; set; }

        /// <summary>
        /// Дата решения об исключении из списка на внеочередное предоставление жилого помещения (76)
        /// </summary>
        [MapField("decision_date")]
        public abstract DateTime? DecisionDate { get; set; }

        /// <summary>
        /// Дата решения об исключении из списока на внеочередное предоставление жилого помещения (77)
        /// </summary>
        [MapField("decision_num")]
        public abstract string DecisionNum { get; set; }

        /// <summary>
        /// примечание  (75)
        /// </summary>
        [MapField("note")]
        public abstract string Note { get; set; }
    }
}
