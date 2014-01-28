using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel.Types 
{
    /// <summary>
    /// Справочник причин снятия с учета
    /// </summary>
    [TableName("gu_hq.q_base_dereg_type")]
    public abstract class QueueBaseDeRegType : IdentityDomainObject<QueueBaseDeRegType>, IPersistentObject
    {
        /// <summary>
        /// идентификатор строки 
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.q_base_dereg_type_seq")]
        [MapField("q_base_dereg_type_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Наименование причины
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Дата начала действия причины
        /// </summary>
        [MapField("date1")]
        public abstract DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия причины
        /// </summary>
        [MapField("date2")]
        public abstract DateTime DateEnd { get; set; }
    }
}
