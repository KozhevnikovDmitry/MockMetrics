using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using Common.Types;

namespace GU.DataModel.Inquiry
{
    /// <summary>
    /// Заявление со стороны схемы gu
    /// </summary>
    [TableName("gu.inquiry")]
    [SearchClass("Запрос")]
    public abstract class Inquiry : IdentityDomainObject<Inquiry>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu.inquiry_seq")]
        [MapField("inquiry_id")]
        [SearchField("Номер", SearchTypeSpec.Number)]
        public abstract override int Id { get; set; }

        [MapField("inquiry_type_id")]
        public abstract int InquiryTypeId { get; set; }

        [NoInstance]
        [Association(ThisKey = "InquiryTypeId", OtherKey = "Id", CanBeNull = false)]
        [SearchField("Вид запроса", SearchTypeSpec.Dictionary)]
        public InquiryType InquiryType { get; set; }

        [MapField("create_date")]
        [SearchField("Дата подачи", SearchTypeSpec.Date)]
        public abstract DateTime? CreateDate { get; set; }

        [MapField("due_date")]
        [SearchField("Выполнить до", SearchTypeSpec.Date)]
        public abstract DateTime? DueDate { get; set; }

        [MapField("close_date")]
        [SearchField("Дата завершения", SearchTypeSpec.Date)]
        public abstract DateTime? CloseDate { get; set; }

        [MapField("request_content_id")]
        public abstract int? RequestContentId { get; set; }

        [NoInstance]
        [Association(ThisKey = "RequestContentId", OtherKey = "Id", CanBeNull = false)]
        public abstract Content RequestContent { get; set; }

        [MapField("response_content_id")]
        public abstract int? ResponseContentId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ResponseContentId", OtherKey = "Id", CanBeNull = true)]
        public abstract Content ResponseContent { get; set; }

        [MapField("inquiry_status_type_id")]
        [SearchField("Статус", SearchTypeSpec.Enum)]
        public abstract InquiryStatusType CurrentState { get; set; }

        [Association(ThisKey = "Id", OtherKey = "Id")]
        public abstract EditableList<InquiryStatus> StatusList { get; set; }

        [MapField("task_id")]
        public abstract int? TaskId { get; set; }

        [NoInstance]
        [Association(ThisKey = "TaskId", OtherKey = "Id", CanBeNull = true)]
        public abstract Task Task { get; set; }
    }
}
