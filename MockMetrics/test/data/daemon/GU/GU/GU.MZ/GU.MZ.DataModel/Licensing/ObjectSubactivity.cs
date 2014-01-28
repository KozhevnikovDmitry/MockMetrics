using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Licensing
{
    /// <summary>
    /// Класс, предназначенный для хранения сущностей Поддеятельность на объекте с номенклатурой  
    /// </summary>
    [TableName("gumz.object_subactivity")] 
    public abstract class ObjectSubactivity : IdentityDomainObject<ObjectSubactivity>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.object_subactivity_seq")] 
        [MapField("object_subactivity_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Id сущности объект с номенклатурой
        /// </summary>
        [MapField("license_object_id")]
        public abstract int LicenseObjectId { get; set; }

        /// <summary>
        /// Объект с номенклатурой
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicenseObjectId", OtherKey = "Id", CanBeNull = false)]
        public abstract LicenseObject LicenseObject { get; set; }

        /// <summary>
        /// Id сущности лицензируемая поддеятельность
        /// </summary>
        [MapField("licensed_subactivity_id")]
        public abstract int LicensedSubactivityId { get; set; }

        /// <summary>
        /// Лицензируемая поддеятельность
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicensedSubactivityId", OtherKey = "Id", CanBeNull = false)]
        public abstract LicensedSubactivity LicensedSubactivity { get; set; }
    }
}
