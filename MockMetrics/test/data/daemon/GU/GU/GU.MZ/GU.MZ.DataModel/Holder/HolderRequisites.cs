using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;
using Common.Types;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.DataModel.Holder
{
    /// <summary>
    /// Класс, представляющий сущность Реквизиты лицензиата
    /// </summary>
    [TableName("gumz.holder_requisites")]
    [SearchClass("Реквизиты лицензиата")]
    public abstract class HolderRequisites : IdentityDomainObject<HolderRequisites>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.holder_requisites_seq")]
        [MapField("holder_requisites_id")]
        public abstract override int Id { get; set; }

        [MapField("create_date")]
        [SearchField("Дата заведения", SearchTypeSpec.Date)]
        public abstract DateTime CreateDate { get; set; }

        [MapField("individual_requisites_id")]
        public abstract int? IndRequisitesId { get; set; }

        [NoInstance]
        [Association(ThisKey = "IndRequisitesId", OtherKey = "Id", CanBeNull = true)]
        public abstract IndRequisites IndRequisites { get; set; }

        [MapField("juridical_requisites_id")]
        public abstract int? JurRequisitesId { get; set; }

        [NoInstance]
        [Association(ThisKey = "JurRequisitesId", OtherKey = "Id", CanBeNull = true)]
        public abstract JurRequisites JurRequisites { get; set; }

        [MapField("address_id")]
        public abstract int? AddressId { get; set; }

        [NoInstance]
        [Association(ThisKey = "AddressId", OtherKey = "Id", CanBeNull = false)]
        public abstract Address Address { get; set; }

        [MapField("license_holder_id")]
        public abstract int LicenseHolderId { get; set; }

        [NoInstance]
        [Association(ThisKey = "licenseHolderId", OtherKey = "Id", CanBeNull = false)]
        public LicenseHolder LicenseHolder { get; set; }

        [MapField("note")]
        [SearchField("Примечание", SearchTypeSpec.String)]
        public abstract string Note { get; set; }

        public virtual LicenseRequisites ToLicenseRequisites()
        {
            var result = LicenseRequisites.CreateInstance();
            result.CreateDate = DateTime.Now;
            result.Address = Address.Clone();
            if (JurRequisites != null)
            {
                result.JurRequisites = JurRequisites.Clone();
            }
            else
            {
                result.IndRequisites = IndRequisites.Clone();
            }
            result.Note = Note;

            return result;
        }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public virtual string ShortName
        {
            get
            {
                return IndRequisites != null ? IndRequisites.ShortName : JurRequisites.ShortName;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public virtual string FullName
        {
            get
            {
                return IndRequisites != null ? IndRequisites.FullName : JurRequisites.FullName;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public virtual string FirmName
        {
            get
            {
                return IndRequisites != null ? IndRequisites.FullName : JurRequisites.FirmName;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public virtual string HeadName
        {
            get
            {
                return IndRequisites != null ? IndRequisites.ShortName : JurRequisites.HeadName;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public virtual string HeadPositionName
        {
            get
            {
                return IndRequisites != null ? "Индивидуальный предприниматель" : JurRequisites.HeadPositionName;
            }
        }
    }
}
