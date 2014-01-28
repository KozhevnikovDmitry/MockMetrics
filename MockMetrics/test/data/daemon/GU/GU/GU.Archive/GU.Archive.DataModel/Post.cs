using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;
using BLToolkit.TypeBuilder;
using Common.Types;
using GU.DataModel;

namespace GU.Archive.DataModel
{
    /// <summary>
    /// Корреспонденция
    /// </summary>
    [TableName("gu_archive.post")]
    [SearchClass("Корреспонденция")]
    public abstract class Post : IdentityDomainObject<Post>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_archive.post_seq")]
        [MapField("post_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        [MapField("registration_num")]
        [SearchField("Регистрационный номер", SearchTypeSpec.String)]
        public abstract string RegistrationNum { get; set; }

        [MapField("stamp")]
        [SearchField("Дата регистрации", SearchTypeSpec.Date)]
        public abstract DateTime Stamp { get; set; }

        /// <summary>
        /// Тип корреспонденции
        /// </summary>
        [MapField("type_id")]
        [SearchField("Тип корреспонденции", SearchTypeSpec.Enum)]
        public abstract PostType PostType { get; set; }

        /// <summary>
        /// Способ доставки корреспонденции
        /// </summary>
        [MapField("delivery_type_id")]
        [SearchField("Способ доставки корреспонденции", SearchTypeSpec.Enum)]
        public abstract DeliveryType DeliveryType { get; set; }

        /// <summary>
        /// тип запроса
        /// </summary>
        [MapField("request_type_id")]
        public abstract RequestType RequestType { get; set; }

        /// <summary>
        /// кто "автор" корреспонденции
        /// </summary>
        [MapField("author_id")]
        public abstract int AuthorId { get; set; }

        [Association(ThisKey = "AuthorId", OtherKey = "Id", CanBeNull = false)]
        public Author Author { get; set; }

        /// <summary>
        /// Организация из которой приплыла Корреспонденция
        /// </summary>
        [MapField("organization_id")]
        public abstract int? OrganizationId { get; set; }

        [Association(ThisKey = "OrganizationId", OtherKey = "Id", CanBeNull = false)]
        public Organization Organization { get; set; }

        [MapField("note")]
        public abstract string Note { get; set; }

        /// <summary>
        /// Список исполнителей
        /// </summary>
        [Association(ThisKey = "Id", OtherKey = "PostId")]
        public abstract EditableList<PostExecutor> Executors { get; set; }

        [MapField("task_id")]
        public abstract int? TaskId { get; set; }

        [NoInstance]
        [Association(ThisKey = "TaskId", OtherKey = "Id", CanBeNull = true)]
        public abstract Task Task { get; set; }

        public override string ToString()
        {
            return string.Format("Корреспонденция, №{0}", RegistrationNum);
        }
    }
}
