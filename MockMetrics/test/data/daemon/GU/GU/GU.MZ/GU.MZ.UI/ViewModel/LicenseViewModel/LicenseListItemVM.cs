using System;
using Common.BL.DictionaryManagement;
using Common.Types;
using Common.Types.Exceptions;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения элемента списка лийензий.
    /// </summary>
    public class LicenseListItemVm : SmartListItemVm<License>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public LicenseListItemVm(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        public override void Initialize(License entity)
        {
            try
            {
                base.Initialize(entity);
                LicenseDataString = string.Format(
                    "№ {0}; Бланк {1}; выдана {2}",
                    Item.RegNumber,
                    Item.BlankNumber,
                    Item.GrantDate.Value.ToLongDateString());

                LicensedActivityString =
                    _dictionaryManager.GetDictionaryItem<LicensedActivity>(Item.LicensedActivityId).
                        Name;

                LicenseHolderString = Item.ActualRequisites.FullName;

                LicenseStatusString = Item.CurrentStatus.GetDescription();
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VMException(ex);
            }
        }

        #region Binding Properties

        private string _licenseDataString;

        /// <summary>
        /// Строка с информацией о лицензии
        /// </summary>
        public string LicenseDataString
        {
            get { return _licenseDataString; }
            private set
            {
                if (value != _licenseDataString)
                {
                    _licenseDataString = value;
                    RaisePropertyChanged(() => LicenseDataString);
                }
            }
        }

        private string _licensedActivityString;

        /// <summary>
        /// Строка с информацией о деятельности
        /// </summary>
        public string LicensedActivityString
        {
            get { return _licensedActivityString; }
            private set
            {
                if (value != _licensedActivityString)
                {
                    _licensedActivityString = value;
                    RaisePropertyChanged(() => LicensedActivityString);
                }
            }
        }

        private string _licenseHolderString;

        /// <summary>
        /// Строка с информацией о лицензиате
        /// </summary>
        public string LicenseHolderString
        {
            get { return _licenseHolderString; }
            private set
            {
                if (value != _licenseHolderString)
                {
                    _licenseHolderString = value;
                    RaisePropertyChanged(() => LicenseHolderString);
                }
            }
        }

        private string _licenseStatusString;

        /// <summary>
        /// Строка с информацией о статусе лицензии
        /// </summary>
        public string LicenseStatusString
        {
            get { return _licenseStatusString; }
            private set
            {
                if (value != _licenseStatusString)
                {
                    _licenseStatusString = value;
                    RaisePropertyChanged(() => LicenseStatusString);
                }
            }
        }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => LicenseDataString);
            RaisePropertyChanged(() => LicensedActivityString);
            RaisePropertyChanged(() => LicenseHolderString);
            RaisePropertyChanged(() => LicenseStatusString);
        }
    }
}
