using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.Types;

using GU.MZ.DataModel.Holder;

namespace GU.MZ.DataModel.Person
{
    /// <summary>
    /// Класс представляющий состояние эксперта - Юридическое лицо
    /// </summary>
    [TableName("gumz.juridical_expert")]
    [SearchClass("Юридическое лицо")]
    public abstract class JuridicalExpertState : IdentityDomainObject<JuridicalExpertState>, IExpertState
    {
        /// <summary>
        /// Класс представляющий состояние эксперта - Юридическое лицо
        /// </summary>
        protected JuridicalExpertState()
        {
            this.LegalFormId = 1;
            this.Address = Address.CreateInstance();
        }

        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("expert_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [MapField("inn")]
        [SearchField("ИНН", SearchTypeSpec.String)]
        public abstract string Inn { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [MapField("ogrn")]
        [SearchField("ОГРН", SearchTypeSpec.String)]
        public abstract string Ogrn { get; set; }

        /// <summary>
        /// Полное наименование организации.
        /// </summary>
        [MapField("full_name")]
        [SearchField("Полное имя", SearchTypeSpec.String)]
        public abstract string FullName { get; set; }

        /// <summary>
        /// Короткое наименование организации.
        /// </summary>
        [MapField("short_name")]
        [SearchField("Краткое имя", SearchTypeSpec.String)]
        public abstract string ShortName { get; set; }

        /// <summary>
        /// Фирменное наименование организации.
        /// </summary>
        [MapField("firm_name")]
        [SearchField("Фирменное имя", SearchTypeSpec.String)]
        public abstract string FirmName { get; set; }

        /// <summary>
        /// Наименование должности руководителя организации.
        /// </summary>
        [MapField("head_position_name")]
        [SearchField("Наименование исп. органа", SearchTypeSpec.String)]
        public abstract string HeadPositionName { get; set; }

        /// <summary>
        /// Фамилия и инициалы руководителя организации.
        /// </summary>
        [MapField("head_name")]
        [SearchField("Исполнительный орган", SearchTypeSpec.String)]
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
        [SearchField("ОПФ", SearchTypeSpec.String)]
        public abstract LegalForm LegalForm { get; set; }

        /// <summary>
        /// Id сущности адрес
        /// </summary>
        [MapField("address_id")]
        public abstract int? AddressId { get; set; }

        /// <summary>
        /// Юридический адрес организации.
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "AddressId", OtherKey = "Id", CanBeNull = false)]
        public abstract Address Address { get; set; }

        /// <summary>
        /// Возвращает имя эксперта
        /// </summary>
        /// <returns>Имя эксперта</returns>
        public string GetName()
        {
            return this.ShortName;
        }

        /// <summary>
        /// Возвращает рабочие данные эксперта
        /// </summary>
        /// <returns>Рабочие данные эксперта</returns>
        public string GetWorkdata()
        {
            return string.Format("ИНН: {0}; ОГРН: {1}", this.Inn, this.Ogrn);
        }
    }
}
