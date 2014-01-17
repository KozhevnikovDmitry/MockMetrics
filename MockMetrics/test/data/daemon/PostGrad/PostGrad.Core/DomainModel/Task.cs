using System;
using System.Threading.Tasks;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel
{
    /// <summary>
    /// Заявление со стороны схемы gu
    /// </summary>
    [TableName("gu.task")]
    public abstract class Task : IdentityDomainObject<Task>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu.task_seq")]
        [MapField("task_id")]
        public abstract override int Id { get; set; }

        [MapField("service_id")]
        public abstract int ServiceId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ServiceId", OtherKey = "Id", CanBeNull = false)]
        public virtual Service Service { get; set; }

        [MapField("agency_id")]
        public abstract int AgencyId { get; set; }

        [MapField("create_date")]
        public abstract DateTime? CreateDate { get; set; }

        [MapField("due_date")]
        public abstract DateTime? DueDate { get; set; }

        [MapField("close_date")]
        public abstract DateTime? CloseDate { get; set; }

        [MapField("content_id")]
        public abstract int? ContentId { get; set; }

        [MapField("task_status_type_id")]
        public abstract TaskStatusType CurrentState { get; set; }

        [Association(ThisKey = "Id", OtherKey = "Id")]
        public abstract EditableList<TaskStatus> StatusList { get; set; }

        [MapField("customer_fio")]
        public abstract string CustomerFio { get; set; }

        [MapField("customer_phone")]
        public abstract string CustomerPhone { get; set; }

        [MapField("customer_email")]
        public abstract string CustomerEmail { get; set; }

        [MapField("customer_note")]
        public abstract string Note { get; set; }

        [MapField("auth_code")]
        public abstract string AuthCode { get; set; }

        public override string ToString()
        {
            string val = string.Format("Заявка №{0}", Id);
            if (CreateDate.HasValue)
            {
                val += string.Format(" от {0}", CreateDate.Value.ToLongDateString());
            }
            return val;
        }
    }
}
