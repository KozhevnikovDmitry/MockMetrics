using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using PostGrad.Core.DomainModel.Holder;

namespace PostGrad.Core.DomainModel.Person
{
    /// <summary>
    /// Класс представляющий состояние эксперта - Юридическое лицо
    /// </summary>
    [TableName("gumz.juridical_expert")]
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
        public abstract string Inn { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [MapField("ogrn")]
        public abstract string Ogrn { get; set; }

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
