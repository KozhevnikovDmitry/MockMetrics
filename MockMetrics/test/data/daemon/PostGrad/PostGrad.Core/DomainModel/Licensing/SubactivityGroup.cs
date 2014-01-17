using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PostGrad.Core.DomainModel.Licensing
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Группа лицензируемых поддеятельностей
    /// </summary>
    [TableName("gumz.subactivity_group")]
    public abstract class SubactivityGroup : IdentityDomainObject<SubactivityGroup>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [MapField("subactivity_group_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Наименование группы лицензируемых поддеятельностей
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Наименование группы лицензируемых поддеятельностей для печати (в творительном падеже)
        /// </summary>
        [MapField("blank_name")]
        public abstract string BlankName { get; set; }
    }
}
