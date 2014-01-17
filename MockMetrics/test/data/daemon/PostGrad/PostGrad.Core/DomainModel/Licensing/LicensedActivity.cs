using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.Licensing
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Тип лицензируемой деятельности.
    /// </summary>
    [TableName("gumz.licensed_activity")]
    public abstract class LicensedActivity : IdentityDomainObject<LicensedActivity>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("licensed_activity_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Наименование лицензируемой деятельности.
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Наименование лицензируемой деятельности для бланка лицензии.
        /// В творительнос падеже.
        /// </summary>
        [MapField("blank_name")]
        public abstract string BlankName { get; set; }

        /// <summary>
        /// Дополнительная информация по виду лицензируемой деятельности
        /// </summary>
        [MapField("additional_info")]
        public abstract string AdditionalInfo { get; set; }

        /// <summary>
        /// Id сущности Группа услуг
        /// </summary>
        [MapField("service_group_id")]
        public abstract int ServiceGroupId { get; set; }

        /// <summary>
        /// Кодовый номер деятельности в отделе Лицензирования.
        /// </summary>
        [MapField("code_number")]
        public abstract string Code { get; set; }

        /// <summary>
        /// Группа услуг
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ServiceGroupId", OtherKey = "Id", CanBeNull = true)]
        public abstract ServiceGroup ServiceGroup { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
