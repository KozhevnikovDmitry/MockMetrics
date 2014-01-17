using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using PostGrad.Core.DomainModel.Holder;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.Core.DomainModel.Dossier
{
    /// <summary>
    /// Класс представляющий сущность Лицензионное дело
    /// </summary>
    [TableName("gumz.license_dossier")]
    public abstract class LicenseDossier : IdentityDomainObject<LicenseDossier>, IPersistentObject
    {
        protected LicenseDossier()
        {
            LicenseList = new EditableList<License>();
            DossierFileList = new EditableList<DossierFile>();
        }

        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.license_dossier_seq")]
        [MapField("license_dossier_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Регистрационный номер лицензионного дела
        /// </summary>
        [MapField("reg_number")]
        public abstract string RegNumber { get; set; }

        /// <summary>
        /// Дата заведения дела
        /// </summary>
        [MapField("create_date")]
        public abstract DateTime CreateDate { get; set; }

        /// <summary>
        /// Флаг активности\закрытости дела.
        /// </summary>
        [MapField("is_active")]
        public abstract bool IsActive { get; set; }

        /// <summary>
        /// Id сущности Лицензиат
        /// </summary>
        [MapField("license_holder_id")]
        public abstract int LicenseHolderId { get; set; }

        /// <summary>
        /// Лицензиат дела.
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicenseHolderId", OtherKey = "Id", CanBeNull = false)]
        public abstract LicenseHolder LicenseHolder { get; set; }

        /// <summary>
        /// Id сущности Вид лицензируемой деятельности.
        /// </summary>
        [MapField("licensed_activity_id")]
        public abstract int LicensedActivityId { get; set; }

        /// <summary>
        /// Вид лицензируемой деятельноссти
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicensedActivityId", OtherKey = "Id", CanBeNull = false)]
        public abstract LicensedActivity LicensedActivity { get; set; }

        /// <summary>
        /// Список лицензий входящих в лицензионное дело.
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseDossierId", CanBeNull = false)]
        public abstract EditableList<License> LicenseList { get; set; }

        /// <summary>
        /// Список томов лицензионного дела
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseDossierId", CanBeNull = true)]
        public abstract EditableList<DossierFile> DossierFileList { get; set; }

        public override string ToString()
        {
            return string.Format("Дело №{0} от {1}", this.RegNumber, this.CreateDate.ToLongDateString());
        }

        public static LicenseDossier CreateInstance(LicensedActivity licensedActivity, LicenseHolder holder)
        {
            var dossier = CreateInstance();
            dossier.IsActive = true;
            dossier.LicenseList = new EditableList<License>();
            dossier.LicenseHolder = holder;
            dossier.LicenseHolderId = holder.Id;
            dossier.LicensedActivity = licensedActivity;
            dossier.LicensedActivityId = licensedActivity.Id;
            dossier.CreateDate = DateTime.Today;
            dossier.RegNumber = string.Format("ЛО-24-{0}-{1}", licensedActivity.Code, holder.Inn);
            return dossier;
        }
    }
}
