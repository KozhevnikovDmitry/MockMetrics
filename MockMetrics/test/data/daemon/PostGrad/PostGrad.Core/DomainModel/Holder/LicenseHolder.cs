using System.Linq;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using PostGrad.Core.DomainModel.Dossier;

namespace PostGrad.Core.DomainModel.Holder
{
    /// <summary>
    /// Класс представляющий сущность Лицензиат, владелец лицензии
    /// </summary>
    [TableName("gumz.license_holder")]
    public abstract class LicenseHolder : IdentityDomainObject<LicenseHolder>, IPersistentObject
    {
        /// <summary>
        /// Класс представляющий сущность Лицензиат, владелец лицензии
        /// </summary>
        protected LicenseHolder()
        {
            this.RequisitesList = new EditableList<HolderRequisites>();
            DossierList = new EditableList<LicenseDossier>();
        }

        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.license_holder_seq")]
        [MapField("license_holder_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [MapField("inn")]
        public abstract string Inn { get; set; }

        /// <summary>
        /// ОГРН(ИП)
        /// </summary>
        [MapField("ogrn")]
        public abstract string Ogrn { get; set; }
        
        /// <summary>
        /// Список лицензионных дел
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseHolderId", CanBeNull = false)]
        public abstract EditableList<LicenseDossier> DossierList { get; set; }
        
        /// <summary>
        /// Список версий реквизитов лицензиата
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "LicenseHolderId", CanBeNull = false)]
        public abstract EditableList<HolderRequisites> RequisitesList { get; set; }

        #region ProxyProperties

        [NoInstance]
        [MapIgnore]
        [CloneIgnore]
        public virtual HolderRequisites ActualRequisites
        {
            get
            {
                if (RequisitesList == null
                    || !RequisitesList.Any())
                {
                    return null;
                }

                return RequisitesList.OrderBy(t => t.CreateDate).Last();
            }
        }

        #endregion

        public override string ToString()
        {
            string holderString;
            if (RequisitesList != null && RequisitesList.Count != 0)
            {
                var actualRequisites = RequisitesList.OrderByDescending(t => t.CreateDate).First();
                holderString = actualRequisites.ShortName;
            }
            else
            {
                holderString = "Ошибка, реквизиты не заданы.";
            }
            return holderString;
        }
    }
}
