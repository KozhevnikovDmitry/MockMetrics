using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.DataModel.LicenseChange
{
    [TableName("gumz.change_license_session")]
    public abstract class ChangeLicenseSession : IdentityDomainObject<ChangeLicenseSession>, IPersistentObject
    {
        public ChangeLicenseSession()
        {
            ChangedLicenseRequisitesList = new EditableList<ChangedLicenseRequisites>();
            ChangedLicenseObjectList = new EditableList<ChangedLicenseObject>();
            ChangedLicenseList = new EditableList<ChangedLicense>();
        }

        [PrimaryKey, Identity]
        [SequenceName("gumz.change_license_session_seq")]
        [MapField("change_license_session_id")]
        public abstract override int Id { get; set; }

        [MapField("license_id")]
        public abstract int LicenseId { get; set; }

        [NoInstance]
        [Association(ThisKey = "LicenseId", OtherKey = "Id", CanBeNull = false)]
        public License License { get; set; }

        [MapField("dossier_file_id")]
        public abstract int DossierFileId { get; set; }

        [NoInstance]
        [Association(ThisKey = "DossierFileId", OtherKey = "Id", CanBeNull = false)]
        public DossierFile DossierFile { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseChangeSessionId", CanBeNull = true)]
        public abstract EditableList<ChangedLicenseRequisites> ChangedLicenseRequisitesList { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseChangeSessionId", CanBeNull = true)]
        public abstract EditableList<ChangedLicenseObject> ChangedLicenseObjectList { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseChangeSessionId", CanBeNull = true)]
        public abstract EditableList<ChangedLicense> ChangedLicenseList { get; set; }

        public void Add(LicenseRequisites requisites)
        {
            var changedLicenseRequisites = ChangedLicenseRequisites.CreateInstance();
            changedLicenseRequisites.ChangeLicenseSession = this;
            changedLicenseRequisites.LicenseRequisites = requisites;
            changedLicenseRequisites.LicenseRequisitesId = requisites.Id;

            ChangedLicenseRequisitesList.Add(changedLicenseRequisites);
        }

        public void Add(LicenseObject licenseObject)
        {
            var changedLicenseObject = ChangedLicenseObject.CreateInstance();
            changedLicenseObject.ChangeLicenseSession = this;
            changedLicenseObject.LicenseObject = licenseObject;
            changedLicenseObject.LicenseObjectId = licenseObject.Id;

            ChangedLicenseObjectList.Add(changedLicenseObject);
        }

        public void Add(License license)
        {
            var changedLicense = ChangedLicense.CreateInstance();
            changedLicense.ChangeLicenseSession = this;
            changedLicense.License = license;
            changedLicense.LicenseId = license.Id;

            ChangedLicenseList.Add(changedLicense);
        }
    }
}