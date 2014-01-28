using System;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Licensing
{
    /// <summary>
    /// Класс представляющий объект лицензии.
    /// </summary>
    [TableName("gumz.license_object")]
    public abstract class LicenseObject : IdentityDomainObject<LicenseObject>, IPersistentObject
    {
        /// <summary>
        /// Класс представляющий объект лицензии.
        /// </summary>
        protected LicenseObject()
        {
            this.Address = Address.CreateInstance();
            this.LicenseObjectStatusId = 1;
            this.ObjectSubactivityList = new EditableList<ObjectSubactivity>();
        }

        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.license_object_seq")]
        [MapField("license_object_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Наименование объекта лицензии
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Номер решения лицензирующего органа о предоставлении лицензии
        /// </summary>
        [MapField("grant_order_reg_number")]
        public abstract string GrantOrderRegNumber { get; set; }

        /// <summary>
        /// Дата решения лицензирующего органа о предоставлении лицензии
        /// </summary>
        [MapField("grant_order_stamp")]
        public abstract DateTime? GrantOrderStamp { get; set; }

        /// <summary>
        /// Id сущности Статус объекта с номенклатурой
        /// </summary>
        [MapField("license_object_status_id")]
        public abstract int LicenseObjectStatusId { get; set; }

        /// <summary>
        /// Статус объекта с номенклатурой
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicenseObjectStatusId", OtherKey = "Id", CanBeNull = false)]
        public abstract LicenseObjectStatus LicenseObjectStatus { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [MapField("note")]
        public abstract string Note { get; set; }
        
        /// <summary>
        /// Id сущности адрес
        /// </summary>
        [MapField("address_id")]
        public abstract int AddressId { get; set; }

        /// <summary>
        /// Адрес объекта лицензии
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "AddressId", OtherKey = "Id", CanBeNull = false)]
        public abstract Address Address { get; set; }

        /// <summary>
        /// Id лицензии
        /// </summary>
        [MapField("license_id")]
        public abstract int LicenseId { get; set; }

        /// <summary>
        /// Лицензия
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicenseId", OtherKey = "Id", CanBeNull = false)]
        public License License { get; set; }

        /// <summary>
        /// Список поддеятельностей на объекте с номенклатурой
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseObjectId", CanBeNull = false)]
        public abstract EditableList<ObjectSubactivity> ObjectSubactivityList { get; set; }

        public bool AddressEquals(LicenseObject newLicObject)
        {
            if (newLicObject.Address.Country.Trim().ToUpper() == Address.Country.Trim().ToUpper() &&
                newLicObject.Address.CountryRegion.Trim().ToUpper() == Address.CountryRegion.Trim().ToUpper() &&
                newLicObject.Address.Area.Trim().ToUpper() == Address.Area.Trim().ToUpper() &&
                newLicObject.Address.City.Trim().ToUpper() == Address.City.Trim().ToUpper() &&
                newLicObject.Address.Street.Trim().ToUpper() == Address.Street.Trim().ToUpper() &&
                newLicObject.Address.House.Trim().ToUpper() == Address.House.Trim().ToUpper() &&
                newLicObject.Address.Build.Trim().ToUpper() == Address.Build.Trim().ToUpper() &&
                newLicObject.Address.Flat.Trim().ToUpper() == Address.Flat.Trim().ToUpper())
            {
                return true;
            }

            return false;
        }
    }
}
