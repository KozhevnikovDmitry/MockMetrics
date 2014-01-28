using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Licensing
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Статус объект с номенклатурой.
    /// </summary>
    [TableName("gumz.license_object_status")]
    public abstract class LicenseObjectStatus : IdentityDomainObject<LicenseObjectStatus>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [MapField("license_object_status_id")]
        public abstract override int Id { get; set; }
        
        /// <summary>
        /// Наименование лицензируемой деятельности.
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }
    }
}
