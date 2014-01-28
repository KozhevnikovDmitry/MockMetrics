using System;

using Common.BL.DataMapping;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.ValidationViewModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.LicenseHolderViewModel;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.LicenseDossierViewModel
{
    /// <summary>
    /// Класс ViewModel для редактирования сущности Лицензионное дело
    /// </summary>
    public class LicenseDossierVm : SmartEditableVm<LicenseDossier>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly IDomainDataMapper<LicenseHolder> _holderMapper;

        /// <summary>
        /// Класс ViewModel для редактирования сущности Лицензионное дело
        /// </summary>
        public LicenseDossierVm(IDialogUiFactory uiFactory, 
                                IDomainDataMapper<LicenseHolder> holderMapper,
                                ISmartValidateableVm<LicenseDossier> licenseDossierDataVm,
                                IEntityInfoVm<HolderRequisites> holderRequisitesInfoVm,
                                ISmartListVm<DossierFile> dossierFileListVm,
                                ISmartListVm<License> licenseListVm)
        {
            LicenseDossierDataVm = licenseDossierDataVm;
            HolderRequisitesInfoVm = holderRequisitesInfoVm;
            DossierFileListVm = dossierFileListVm;
            LicenseListVm = licenseListVm;
            _uiFactory = uiFactory;
            _holderMapper = holderMapper;
            GoToLicenseHolderCommand = new DelegateCommand(GoToLicenseHolder);
        }

        #region EditableVM
        
        /// <summary>
        /// Пересобирает поля привязки.
        /// </summary>
        protected override void Rebuild()
        {
            LicenseDossierDataVm.Initialize(Entity);
            HolderRequisitesInfoVm.Initialize(Entity.LicenseHolder.ActualRequisites);
            DossierFileListVm.Initialize(Entity.DossierFileList);
            LicenseListVm.Initialize(Entity.LicenseList);
            AvalonInteractor.RegisterCaller(DossierFileListVm as IAvalonDockCaller);
            AvalonInteractor.RegisterCaller(LicenseListVm as IAvalonDockCaller);
        }

        protected override void Save()
        {
            var validationResults = Validate();
            if (!validationResults.IsValid)
            {
                _uiFactory.ShowToolView(new ValidationsView(), new ValidationsVM(validationResults.Errors), "Ошибочно заполненные поля");
                LicenseDossierDataVm.RaiseIsValidChanged();
                DossierFileListVm.RaiseIsValidChanged();
                return;
            }

            base.Save();
        }

        #endregion

        #region Binding Properties

        public ISmartValidateableVm<LicenseDossier> LicenseDossierDataVm { get; private set; }

        /// <summary>
        /// VM для отображения данных лицензиата
        /// </summary>
        private HolderRequisitesInfoVm _holderRequisitesInfoVm;

        public IEntityInfoVm<HolderRequisites> HolderRequisitesInfoVm { get; private set; }

        public ISmartListVm<DossierFile> DossierFileListVm { get; private set; }

        public ISmartListVm<License> LicenseListVm { get; private set; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда перехода к карточке лицензиата
        /// </summary>
        public DelegateCommand GoToLicenseHolderCommand { get; private set; }

        /// <summary>
        /// Осуществляет переход к карточке лицензиата
        /// </summary>
        private void GoToLicenseHolder()
        {
            try
            {
                var holder = _holderMapper.Retrieve(Entity.LicenseHolderId);
                AvalonInteractor.RaiseOpenEditableDockable(holder.ToString(), typeof(LicenseHolder), holder);
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
