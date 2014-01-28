using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using Common.Types;
using GU.MZ.DataModel.Holder;

namespace GU.MZ.DataModel.Requisites
{
    [TableName("gumz.juridical_requisites")]
    [SearchClass("Юридическое лицо")]
    public abstract class JurRequisites : IdentityDomainObject<JurRequisites>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.juridical_requisites_seq")]
        [MapField("juridical_requisites_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Полное наименование организации.
        /// </summary>
        [MapField("full_name")]
        [SearchField("Полное наименование", SearchTypeSpec.String)]
        public abstract string FullName { get; set; }

        /// <summary>
        /// Короткое наименование организации.
        /// </summary>
        [MapField("short_name")]
        [SearchField("Краткое наименование", SearchTypeSpec.String)]
        public abstract string ShortName { get; set; }

        /// <summary>
        /// Фирменное наименование организации.
        /// </summary>
        [MapField("firm_name")]
        [SearchField("Фирменное наименование", SearchTypeSpec.String)]
        public abstract string FirmName { get; set; }

        /// <summary>
        /// Наименование должности руководителя организации.
        /// </summary>
        [MapField("head_position_name")]
        [SearchField("Должность руководителя", SearchTypeSpec.String)]
        public abstract string HeadPositionName { get; set; }

        /// <summary>
        /// Фамилия и инициалы руководителя организации.
        /// </summary>
        [MapField("head_name")]
        [SearchField("ФИО руководителя", SearchTypeSpec.String)]
        public abstract string HeadName { get; set; }

        /// <summary>
        /// Id организационно-правовой формы
        /// </summary>
        [MapField("legal_form_id")]
        public abstract int LegalFormId { get; set; }

        /// <summary>
        /// Организационно-правовая форма.
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LegalFormId", OtherKey = "Id", CanBeNull = false)]
        [SearchField("Организационно-правовая форма", SearchTypeSpec.Dictionary)]
        public abstract LegalForm LegalForm { get; set; }

        [MapField("note")]
        [SearchField("Примечание", SearchTypeSpec.String)]
        public abstract string Note { get; set; }
    }
}