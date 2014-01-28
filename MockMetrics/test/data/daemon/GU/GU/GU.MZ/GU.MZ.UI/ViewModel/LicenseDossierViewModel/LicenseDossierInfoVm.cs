using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseDossierViewModel
{
    public class LicenseDossierInfoVm : EntityInfoVm<LicenseDossier>
    {
        private readonly IDictionaryManager _dictionaryManager;
        
        public LicenseDossierInfoVm(IDomainDataMapper<LicenseDossier> entityMapper, IDictionaryManager dictionaryManager) 
            : base(entityMapper)
        {
            _dictionaryManager = dictionaryManager;
        }

        #region Binding Properties

        public string DossierCommonString
        {
            get
            {
                return string.Format("№{0}; {1}", Entity.RegNumber,
                    _dictionaryManager.GetDictionaryItem<LicensedActivity>(Entity.LicensedActivityId));
            }
        }

        public string CreateDateString
        {
            get
            {
                var statusString = Entity.IsActive ? "Дело активно" : "Дело было закрыто";
                return string.Format("Заведено {0} {1}; {2}", Entity.CreateDate.ToLongDateString(), Entity.CreateDate.ToLongTimeString(), statusString);
            }
        }

        public string HolderDataString
        {
            get { return string.Format("{0}; ИНН [{1}]; ОГРН [{2}]", Entity.LicenseHolder.ActualRequisites.FullName, Entity.LicenseHolder.Inn, Entity.LicenseHolder.Ogrn); }
        }

        public string RegNumber
        {
            get
            {
                return Entity.RegNumber;
            }
        }

        public string CreateDate
        {
            get
            {
                return  Entity.CreateDate.ToLongDateString();
            }
        }

        public string LicensedActivity
        {
            get { return _dictionaryManager.GetDictionaryItem<LicensedActivity>(Entity.LicensedActivityId).Name; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Дело №{0} от {1}", Entity.RegNumber, Entity.CreateDate.ToLongDateString());
        }
    }
}