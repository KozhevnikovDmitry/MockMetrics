using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.DataModel.LicenseChange
{
    [TableName("gumz.changed_license")]
    public abstract class ChangedLicense : IdentityDomainObject<ChangedLicense>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.changed_license_seq")]
        [MapField("changed_license_id")]
        public abstract override int Id { get; set; }

        [MapField("change_license_session_id")]
        public abstract int ChangeLicenseSessionId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ChangeLicenseSessionId", OtherKey = "Id", CanBeNull = false)]
        public ChangeLicenseSession ChangeLicenseSession { get; set; }

        [MapField("license_id")]
        public abstract int LicenseId { get; set; }

        [NoInstance]
        [Association(ThisKey = "LicenseId", OtherKey = "Id", CanBeNull = false)]
        public License License { get; set; }
    }
}