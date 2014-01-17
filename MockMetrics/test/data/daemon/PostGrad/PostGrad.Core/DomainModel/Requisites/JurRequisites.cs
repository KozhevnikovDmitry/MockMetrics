using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using PostGrad.Core.DomainModel.Holder;

namespace PostGrad.Core.DomainModel.Requisites
{
    [TableName("gumz.juridical_requisites")]
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
        public abstract string FullName { get; set; }

        /// <summary>
        /// Короткое наименование организации.
        /// </summary>
        [MapField("short_name")]
        public abstract string ShortName { get; set; }

        /// <summary>
        /// Фирменное наименование организации.
        /// </summary>
        [MapField("firm_name")]
        public abstract string FirmName { get; set; }

        /// <summary>
        /// Наименование должности руководителя организации.
        /// </summary>
        [MapField("head_position_name")]
        public abstract string HeadPositionName { get; set; }

        /// <summary>
        /// Фамилия и инициалы руководителя организации.
        /// </summary>
        [MapField("head_name")]
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
        public abstract LegalForm LegalForm { get; set; }

        [MapField("note")]
        public abstract string Note { get; set; }
    }
}