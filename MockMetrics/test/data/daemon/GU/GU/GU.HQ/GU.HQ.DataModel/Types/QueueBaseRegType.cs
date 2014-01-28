using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel.Types
{
    /// <summary>
    /// СПРАВОЧНИК: основание постановки на учет
    /// </summary>
    [TableName("gu_hq.q_base_reg_type")]
    public abstract class QueueBaseRegType : IdentityDomainObject<QueueBaseRegType>, IPersistentObject 
    {
        /// <summary>
        /// идентификатор записи
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.q_base_reg_type_seq")]
        [MapField("q_base_reg_type_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Наименование причины постановки на учет
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

        /// <summary>
        /// Номер в бумажном заявлении поданного заявителем
        /// </summary>
        [MapField("doc_num")]
        public abstract int DocNum { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
