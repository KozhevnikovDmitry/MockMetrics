using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.Licensing
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Адрес.
    /// </summary>
    [TableName("gumz.licensed_subactivity")]
    public abstract class LicensedSubactivity : IdentityDomainObject<LicensedSubactivity>, IPersistentObject
    {
        /// <summary>
        /// Значение первичного ключа. 
        /// </summary>
        [PrimaryKey]
        [MapField("licensed_subactivity_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Наименование поддеятельности
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Наименование поддеятельности для печати (в творительном падеже)
        /// </summary>
        [MapField("blank_name")]
        public abstract string BlankName { get; set; }

        /// <summary>
        /// Id связной сущности Лицензируемая деятельность
        /// </summary>
        [MapField("licensed_activity_id")]
        public abstract int LicensedActivityId { get; set; }

        /// <summary>
        /// Id сущности группа лицензируемых поддеятельностей
        /// </summary>
        [MapField("subactivity_group_id")]
        public abstract int SubactivityGroupId { get; set; }

        /// <summary>
        /// Группа лицензируемых поддеятельностей
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "SubactivityGroupId", OtherKey = "Id", CanBeNull = false)]
        public abstract SubactivityGroup SubactivityGroup { get; set; }

        public override string ToString()
        {
            return this.Name;
        }


    }
}
