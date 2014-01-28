using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel.ValidationViewModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseHolderViewModel
{
    /// <summary>
    /// Класс ViewModel для редактирования сущности Лицензиат
    /// </summary>
    public class LicenseHolderVm : SmartEditableVm<LicenseHolder>
    {
        private readonly IDialogUiFactory _uiFactory;

        /// <summary>
        /// Класс ViewModel для редактирования сущности Лицензиат
        /// </summary>
        public LicenseHolderVm(IDialogUiFactory uiFactory, 
                               ISmartValidateableVm<LicenseHolder> licenseHolderDataVm, 
                               ISmartValidateableVm<HolderRequisites> holderRequisitesDataVm)
        {
            _uiFactory = uiFactory;
            LicenseHolderDataVm = licenseHolderDataVm;
            HolderRequisitesDataVm = holderRequisitesDataVm;
        }

        #region Overrides of EditableVM<License>

        /// <summary>
        /// Сохраняет изменения в редактируемой лицензии.
        /// </summary>
        protected override void Save()
        {
            var validationResults = Validate();
            if (!validationResults.IsValid)
            {
                _uiFactory.ShowToolView(new ValidationsView(), new ValidationsVM(validationResults.Errors), "Ошибочно заполненные поля");
                LicenseHolderDataVm.RaiseIsValidChanged();
                HolderRequisitesDataVm.RaiseIsValidChanged();
                return;
            }

            base.Save();
        }

        /// <summary>
        /// Пересобирает поля привязки.
        /// </summary>
        protected override void Rebuild()
        {
            LicenseHolderDataVm.Initialize(Entity);
            HolderRequisitesDataVm.Initialize(Entity.ActualRequisites);
            //DossierListVm.Initialize(Entity.DossierList);
        }

        #endregion

        #region Binding Properties

        public ISmartValidateableVm<LicenseHolder> LicenseHolderDataVm { get; private set; }

        public ISmartValidateableVm<HolderRequisites> HolderRequisitesDataVm { get; private set; }

        public ISmartListVm<LicenseDossier> DossierListVm { get; private set; }

        #endregion
    }
}
