using Common.UI.ViewModel.SearchViewModel;

using GU.MZ.DataModel.Holder;
using GU.MZ.UI.ViewModel.LicenseHolderViewModel;

namespace GU.MZ.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения результата поиска Лицензиатов
    /// </summary>
    public class LicenseHolderSearchResultVm : AbstractSearchResultVM<LicenseHolder>    
    {
        private HolderRequisitesInfoVm _infoVm;

        public LicenseHolderSearchResultVm(LicenseHolder entity, HolderRequisitesInfoVm infoVm)
            : base(entity, null)
        {
            _infoVm = infoVm;
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _infoVm.Initialize(Result.ActualRequisites);
        }

        #region Binding Properties

        public string Inn
        {
            get
            {
                return _infoVm.Inn;
            }
        }

        public string Ogrn
        {
            get
            {
                return _infoVm.Ogrn;
            }
        }

        public string FullName
        {
            get
            {
                return _infoVm.FullName;
            }
        }

        public string FirmName
        {
            get
            {
                return _infoVm.FirmName;
            }
        }

        public string ShortName
        {
            get
            {
                return _infoVm.ShortName;
            }
        }

        public string HeadName
        {
            get
            {
                return _infoVm.HeadName;
            }
        }

        public string HeadPositionName
        {
            get
            {
                return _infoVm.HeadPositionName;
            }
        }

        public string Address
        {
            get
            {
                return _infoVm.Address;
            }
        }
        
        public string LegalFormName
        {
            get
            {
                return _infoVm.LegalFormName;
            }
        }

        #endregion
    }
}
