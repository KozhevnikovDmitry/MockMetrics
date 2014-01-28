using GU.MZ.DataModel.Holder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseHolderViewModel
{
    public class LicenseHolderDataVm : SmartValidateableVm<LicenseHolder>
    {
        #region Binding Properties

        /// <summary>
        /// ИНН лицензиата
        /// </summary>
        public string Inn
        {
            get
            {
                return Entity.Inn;
            }
            set
            {
                if (Entity.Inn != value)
                {
                    Entity.Inn = value;
                    RaisePropertyChanged(() => Inn);
                }
            }
        }

        /// <summary>
        /// ОГРН лицензиата
        /// </summary>
        public string Ogrn
        {
            get
            {
                return Entity.Ogrn;
            }
            set
            {
                if (Entity.Ogrn != value)
                {
                    Entity.Ogrn = value;
                    RaisePropertyChanged(() => Ogrn);
                }
            }
        }
         

        #endregion
    }
}
