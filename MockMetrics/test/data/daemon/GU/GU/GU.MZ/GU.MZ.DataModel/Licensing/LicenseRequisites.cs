using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;
using Common.Types;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.DataModel.Licensing
{
    /// <summary>
    /// Класс, представляющий сущность Реквизиты лицензии
    /// </summary>
    [TableName("gumz.license_requisites")]
    [SearchClass("Реквизиты лицензии")]
    public abstract class LicenseRequisites : IdentityDomainObject<LicenseRequisites>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.license_requisites_seq")]
        [MapField("license_requisites_id")]
        public abstract override int Id { get; set; }

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

        /// <summary>
        /// Примечание.
        /// </summary>
        [MapField("note")]
        [SearchField("Примечание", SearchTypeSpec.String)]
        public abstract string Note { get; set; }

        /// <summary>
        /// Дата заведения реквизитов.
        /// </summary>
        [MapField("create_date")]
        [SearchField("Дата заведения", SearchTypeSpec.Date)]
        public abstract DateTime CreateDate { get; set; }

        /// <summary>
        /// Id сущности адрес
        /// </summary>
        [MapField("address_id")]
        public abstract int? AddressId { get; set; }

        /// <summary>
        /// Юридический адрес организации.
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "AddressId", OtherKey = "Id", CanBeNull = false)]
        public abstract Address Address { get; set; }

        /// <summary>
        /// Id сущности Лицензия
        /// </summary>
        [MapField("license_id")]
        public abstract int LicenseId { get; set; }

        /// <summary>
        /// Лицензия, которой приндлежат реквизиты.
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicenseId", OtherKey = "Id", CanBeNull = false)]
        public License License { get; set; }

        #region Proxy Properties

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
        
        #endregion

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", FullName, HeadPositionName, HeadName);
        }
    }
}
