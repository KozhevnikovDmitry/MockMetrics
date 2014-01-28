using System;
using System.Linq;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;
using Common.Types;
using Common.Types.Exceptions;

namespace GU.DataModel
{
    /// <summary>
    /// Заявление со стороны схемы gu
    /// </summary>
    [TableName("gu.task")]
    [SearchClass("Заявление")]
    public abstract class Task : IdentityDomainObject<Task>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu.task_seq")]
        [MapField("task_id")]
        [SearchField("Номер", SearchTypeSpec.Number)]
        public abstract override int Id { get; set; }

        [MapField("service_id")]
        public abstract int ServiceId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ServiceId", OtherKey = "Id", CanBeNull = false)]
        [SearchField("Услуга", SearchTypeSpec.Dictionary)]
        public virtual Service Service { get; set; }

        [MapField("agency_id")]
        public abstract int AgencyId { get; set; }

        /// <summary>
        /// Ведомство, которое должно рассмотреть заявку.
        /// Должно быть подведомством Service.ServiceGroup.Agency
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "AgencyId", OtherKey = "Id", CanBeNull = false)]
        [SearchField("Ведомство", SearchTypeSpec.Dictionary)]
        public Agency Agency { get; set; }

        [MapField("create_date")]
        [SearchField("Дата подачи", SearchTypeSpec.Date)]
        public abstract DateTime? CreateDate { get; set; }

        [MapField("due_date")]
        [SearchField("Выполнить до", SearchTypeSpec.Date)]
        public abstract DateTime? DueDate { get; set; }

        [MapField("close_date")]
        [SearchField("Дата завершения", SearchTypeSpec.Date)]
        public abstract DateTime? CloseDate { get; set; }

        [MapField("content_id")]
        public abstract int? ContentId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ContentId", OtherKey = "Id", CanBeNull = false)]
        public abstract Content Content { get; set; }

        [MapField("task_status_type_id")]
        [SearchField("Статус", SearchTypeSpec.Enum)]
        public abstract TaskStatusType CurrentState { get; set; }

        [Association(ThisKey = "Id", OtherKey = "Id")]
        public abstract EditableList<TaskStatus> StatusList { get; set; }

        [MapField("customer_fio")]
        [SearchField("ФИО заявителя", SearchTypeSpec.String)]
        public abstract string CustomerFio { get; set; }

        [MapField("customer_phone")]
        [SearchField("Телефон заявителя", SearchTypeSpec.String)]
        public abstract string CustomerPhone { get; set; }

        [MapField("customer_email")]
        [SearchField("Email заявителя", SearchTypeSpec.String)]
        public abstract string CustomerEmail { get; set; }

        [MapField("customer_note")]
        [SearchField("Примечение", SearchTypeSpec.String)]
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

        #region Proxy

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public virtual ContentNode RootContentNode
        {
            get
            {
                if (Content == null)
                {
                    throw new BLLException("Контент заявки не найдена");
                }

                if (Content.RootContentNodes == null)
                {
                    throw new BLLException("Контент заявки не определён");
                }

                var root = Content.RootContentNodes.FirstOrDefault();

                if (root == null)
                {
                    throw new BLLException("Контент заявки пустой");
                }

                return root;
            }
        }

        #endregion
    }
}
