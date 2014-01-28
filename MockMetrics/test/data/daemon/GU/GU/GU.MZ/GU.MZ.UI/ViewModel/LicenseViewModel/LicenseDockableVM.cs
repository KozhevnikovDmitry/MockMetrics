using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel.ValidationViewModel;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.BL.Reporting.Mapping.MappingException;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    /// <summary>
    /// Класс VM для редактирования данных сущности Лицензия
    /// </summary>
    public class LicenseDockableVm : SmartEditableVm<License>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly LicenseBlankReport _licenseBlankReport;

        /// <summary>
        /// Класс VM для редактирования данных сущности Лицензия
        /// </summary>
        /// <param name="uiFactory"></param>
        /// <param name="licenseBlankReport"></param>
        public LicenseDockableVm(IDialogUiFactory uiFactory,
                                 LicenseBlankReport licenseBlankReport, 
                                 ISmartValidateableVm<License> licenseDataVm,
                                 LicenseObjectsVm licenseObjectsVm,
                                 LicenseStatusesVm licenseStatusesVm,
                                 ISmartListVm<LicenseRequisites> licenseRequisitesVm)
        {
            _uiFactory = uiFactory;
            _licenseBlankReport = licenseBlankReport;
            LicenseDataVm = licenseDataVm;
            LicenseObjectsVm = licenseObjectsVm;
            LicenseStatusesVm = licenseStatusesVm;
            LicenseRequisitesVm = licenseRequisitesVm;
        }

        /// <summary>
        /// Пересобирает дочерние ViewModel'ы
        /// </summary>
        protected override void Rebuild()
        {
            PrintLicenseCommand = new DelegateCommand(PrintLicense);
            LicenseDataVm.Initialize(Entity);
            LicenseObjectsVm.Initialize(Entity.LicenseObjectList);
            LicenseObjectsVm.License = Entity;
            LicenseStatusesVm.Initialize(Entity.LicenseStatusList);
            LicenseStatusesVm.License = Entity;
            LicenseRequisitesVm.Initialize(Entity.LicenseRequisitesList);
        }

        #region Overrides of EditableVM<License>

        /// <summary>
        /// Сохраняет изменения в редактируемой лицензии.
        /// </summary>
        protected override void Save()
        {
            var validationResults =  Validate();
            if (!validationResults.IsValid)
            {
                _uiFactory.ShowToolView(new ValidationsView(), new ValidationsVM(validationResults.Errors), "Ошибочно заполненные поля");
                LicenseDataVm.RaiseIsValidChanged();
                return; 
            }

            base.Save();
        }

        #endregion

        #region Binding Properties
        
        public ISmartValidateableVm<License> LicenseDataVm { get; private set; }

        public LicenseObjectsVm LicenseObjectsVm { get; private set; }

        public LicenseStatusesVm LicenseStatusesVm { get; private set; }

        public ISmartListVm<LicenseRequisites> LicenseRequisitesVm { get; private set; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда формирования бланка лицензии
        /// </summary>
        public DelegateCommand PrintLicenseCommand { get; private set; }

        /// <summary>
        /// Формирует и отображает бланк лицензии
        /// </summary>
        private void PrintLicense()
        {
            try
            {
                if (!LicenseDataVm.IsValid || !LicenseObjectsVm.IsValid)
                {
                    LicenseDataVm.RaiseAllPropertyChanged();
                    LicenseObjectsVm.RaiseItemsValidatingPropertyChanged();
                    return;
                }

                bool isDebug = false;
#if DEBUG
                isDebug = true;
#endif

                try
                {
                    _licenseBlankReport.SetLicense(Entity);
                    AvalonInteractor.RaiseOpenReportDockable(
                        string.Format("Бланк лицензии №{0}", Entity.RegNumber),
                       _licenseBlankReport,
                        isDebug);
                }
                catch (RetrieveLicenseDataWithoutActiveObjectsException ex)
                {
                    NoticeUser.ShowWarning(ex.Message);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #endregion
    }
}
