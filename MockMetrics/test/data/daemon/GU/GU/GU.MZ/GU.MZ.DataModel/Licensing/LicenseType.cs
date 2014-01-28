using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Licensing
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Тип лицензии.
    /// </summary>
    [TableName("gumz.license_type")]
    public abstract class LicenseType : IdentityDomainObject<LicenseType>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("license_type_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Наименование типа лицензии.
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
