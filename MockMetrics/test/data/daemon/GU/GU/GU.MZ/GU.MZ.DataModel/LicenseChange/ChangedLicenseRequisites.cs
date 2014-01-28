using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.DataModel.LicenseChange
{
    [TableName("gumz.changed_license_requisites")]
    public abstract class ChangedLicenseRequisites : IdentityDomainObject<ChangedLicenseRequisites>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.changed_license_requisites_seq")]
        [MapField("changed_license_requisites_id")]
        public abstract override int Id { get; set; }

        [MapField("change_license_session_id")]
        public abstract int ChangeLicenseSessionId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ChangeLicenseSessionId", OtherKey = "Id", CanBeNull = false)]
        public ChangeLicenseSession ChangeLicenseSession { get; set; }

        [MapField("license_requisites_id")]
        public abstract int LicenseRequisitesId { get; set; }

        [NoInstance]
        [Association(ThisKey = "LicenseRequisitesId", OtherKey = "Id", CanBeNull = false)]
        public LicenseRequisites LicenseRequisites { get; set; }
    }
}