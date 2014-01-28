
using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel.Types
{
    /// <summary>
    /// СПРАВОЧНИК: основания для постановки в очередь внеочередников
    /// </summary>
    [TableName("gu_hq.qp_base_reg_type")]
    public abstract class QueuePrivBaseRegType : IdentityDomainObject<QueuePrivBaseRegType>, IPersistentObject
    {
        /// <summary>
        /// Уникальный идентификатор объекта
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.qp_base_reg_type_seq")]
        [MapField("qp_base_reg_type_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// наимнеование категории
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// дата начала действия категории
        /// </summary>
        [MapField("date1")]
        public abstract DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия категории
        /// </summary>
        [MapField("date2")]
        public abstract DateTime DateEnd { get; set; }
    }
}
