using System;
using System.Collections.Generic;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    /// <summary>
    /// Заявление со стороны схемы gu
    /// </summary>
    [TableName("gu.task")]
    public class Task : IDomainObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu.task_seq")]
        [MapField("task_id")]
        public int Id { get; set; }

        [MapField("content_id")]
        public int? ContentId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ContentId", OtherKey = "Id", CanBeNull = false)]
        public Content Content { get; set; }

        [MapField("agency_id")]
        public int AgencyId { get; set; }

        [MapField("service_id")]
        public int ServiceId { get; set; }

        [MapField("task_status_type_id")]
        public int TaskStatusTypeId { get; set; }

        [MapField("customer_fio")]
        public string CustomerFio { get; set; }


        [MapField("create_date")]
        public DateTime? CreateDate { get; set; }

        public override string ToString()
        {
            string val = string.Format("Заявка №{0}", this.Id);
            if (this.CreateDate.HasValue)
            {
                val += string.Format(" от {0} {1}", this.CreateDate.Value.ToLongDateString(), this.CreateDate.Value.ToShortTimeString());
            }
            return val;
        }
    }
}
