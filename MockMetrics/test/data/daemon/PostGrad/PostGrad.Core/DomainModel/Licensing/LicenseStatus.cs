using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.Licensing
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Статус лицензии.
    /// </summary>
    [TableName("gumz.license_status")]
    public abstract class LicenseStatus : IdentityDomainObject<LicenseStatus>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("license_status_id")]
        public abstract override int Id { get; set; }

        [MapField("license_status_type_id")]
        public abstract LicenseStatusType LicenseStatusType { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("note")]
        public abstract string Note { get; set; }

        [MapField("license_id")]
        public abstract int LicenseId { get; set; }

        [NoInstance]
        [Association(ThisKey = "LicenseId", OtherKey = "Id", CanBeNull = false)]
        public License License { get; set; }
    }
}
