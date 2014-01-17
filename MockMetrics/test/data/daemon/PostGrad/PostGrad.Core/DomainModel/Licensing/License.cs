using System;
using System.Linq;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Holder;

namespace PostGrad.Core.DomainModel.Licensing
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Лицензия.
    /// </summary>
    [TableName("gumz.license")]
    public abstract class License : IdentityDomainObject<License>, IPersistentObject
    {
        /// <summary>
        /// Класс, предназначенный для хранения данных сущности Лицензия.
        /// </summary>
        protected License()
        {
            GrantDate = DateTime.Today;
            DueDate = DateTime.Today;
            GrantOrderStamp = DateTime.Today;
            LicenseObjectList = new EditableList<LicenseObject>();
            LicenseRequisitesList = new EditableList<LicenseRequisites>();
            LicenseStatusList = new EditableList<LicenseStatus>();
            LicensedActivityId = 1;
            AcceptChanges();
        }
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.license_seq")]
        [MapField("license_id")]
        public abstract override int Id { get; set; }

        #region License Data

        /// <summary>
        /// Регистрационный номер лицензии
        /// </summary>
        [MapField("reg_number")]
        public abstract string RegNumber { get; set; }

        /// <summary>
        /// Дата выдачи лицензии
        /// </summary>
        [MapField("grant_date")]
        public abstract DateTime? GrantDate { get; set; }

        /// <summary>
        /// Дата окончания срока действия лицензии
        /// </summary>
        [MapField("due_date")]
        public abstract DateTime? DueDate { get; set; }

        /// <summary>
        /// Id сущности Лицензируемая Деятельность
        /// </summary>
        [MapField("licensed_activity_id")]
        public abstract int LicensedActivityId { get; set; }

        /// <summary>
        /// Тип текущего статуса лицензии
        /// </summary>
        [MapField("license_status_type_id")]
        public abstract LicenseStatusType CurrentStatus { get; set; }

        /// <summary>
        /// Лицензируемая Деятельность
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicensedActivityId", OtherKey = "Id", CanBeNull = false)]
        public abstract LicensedActivity LicensedActivity { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [MapField("note")]
        public abstract string Note { get; set; }
        
        #endregion

        #region Blank Data

        /// <summary>
        /// Номер бланка лицензии
        /// </summary>
        [MapField("blank_number")]
        public abstract string BlankNumber { get; set; }

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
        /// Имя главы лицензирующей организации
        /// </summary>
        [MapField("licensiar_head_name")]
        public abstract string LicensiarHeadName { get; set; }

        /// <summary>
        /// Должность главы лицензирующей организации
        /// </summary>
        [MapField("licensiar_head_position")]
        public abstract string LicensiarHeadPosition { get; set; }
        
        #endregion

        #region Associations

        /// <summary>
        /// Id сущности Лицензионное дело
        /// </summary>
        [MapField("license_dossier_id")]
        public abstract int LicenseDossierId { get; set; }

        /// <summary>
        /// Лиценщионное дело
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicenseDossierId", OtherKey = "Id", CanBeNull = false)]
        public abstract LicenseDossier LicenseDossier { get; set; }

        /// <summary>
        /// Список объектов с номенклатурой лицензии
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseId", CanBeNull = false)]
        public abstract EditableList<LicenseObject> LicenseObjectList { get; set; }

        /// <summary>
        /// Список реквизитов лицензии
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseId", CanBeNull = false)]
        public abstract EditableList<LicenseRequisites> LicenseRequisitesList { get; set; }

        /// <summary>
        /// Список статусов лицензии
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseId", CanBeNull = false)]
        public abstract EditableList<LicenseStatus> LicenseStatusList { get; set; }

        #endregion

        #region Proxy Properties

        [NoInstance]
        [MapIgnore]
        [CloneIgnore]
        public virtual LicenseHolder LicenseHolder
        {
            get
            {
                if (LicenseDossier == null)
                {
                    return null;
                }

                return LicenseDossier.LicenseHolder;
            }
        }

        [NoInstance]
        [MapIgnore]
        [CloneIgnore]
        public virtual LicenseRequisites ActualRequisites
        {
            get
            {
                if (LicenseRequisitesList == null
                   || !LicenseRequisitesList.Any())
                {
                    return null;
                }

                return LicenseRequisitesList.OrderBy(t => t.CreateDate).Last();
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format("Лицензия №{0}", RegNumber);
        }

        #endregion
    }
}
