using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.UI.View.EmployeeView;
using GU.MZ.UI.ViewModel.LinkageViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    public class LinkageStepVm : AbstractSupervisionStepVm
    {
        public LinkageStepVm(LinkageHolderVm linkageHolderVm, 
                             LinkageDossierVm linkageDossierVm, 
                             LinkageLicenseVm linkageLicenseVm)
        {
            LinkageHolderVm = linkageHolderVm;
            LinkageDossierVm = linkageDossierVm;
            LinkageLicenseVm = linkageLicenseVm;
            LinkageFileCommand = new DelegateCommand(LinkageFile, () => !IsLinkaged && IsCurrentOrPrevious);
            RejectFileCommand = new DelegateCommand(RejectFile, () => IsLinkaged && IsCurrentOrPrevious);
            ShowInventoryReportCommand = new DelegateCommand(ShowInventoryReport, () => IsCurrentOrPrevious);
        }

        public override void CustInit(SupervisionFacade superviser)
        {
            Superviser = superviser;
            IsLinkaged = DossierFile.LicenseDossierId.HasValue;
            if (IsLinkaged)
            {
                Rebuild();
            }
        }

        protected override void Rebuild()
        {
            _linkageHolderVm.Initialize(Superviser.DossierFile);
            _linkageDossierVm.Initialize(Superviser.DossierFile);
            _linkageLicenseVm.Initialize(Superviser.DossierFile);
            LinkageDossierVm.DossierChanged -= LinkageDossierVmOnDossierChanged;
            RaiseStepCommandsCanExecute();
            RaiseVmChanged();
        }

        private void Rebuild(IDossierFileLinkWrapper fileLinkWrapper)
        {
            _linkageHolderVm.Initialize(fileLinkWrapper);
            _linkageDossierVm.DossierChanged -= LinkageDossierVmOnDossierChanged;
            _linkageDossierVm.Initialize(fileLinkWrapper);
            LinkageDossierVm.DossierChanged += LinkageDossierVmOnDossierChanged;
            _linkageLicenseVm.Initialize(fileLinkWrapper);
            RaiseStepCommandsCanExecute();
            RaiseVmChanged();
        }

        private void RaiseVmChanged()
        {
            if (LinkageHolderVm != null)
            {
                RaisePropertyChanged(() => LinkageHolderVm);
                LinkageHolderVm.RaiseAllPropertyChanged();
            }
            if (LinkageDossierVm != null)
            {
                RaisePropertyChanged(() => LinkageDossierVm);
                LinkageDossierVm.RaiseAllPropertyChanged();
            }
            if (LinkageLicenseVm != null)
            {
                RaisePropertyChanged(() => LinkageLicenseVm);
                LinkageLicenseVm.RaiseAllPropertyChanged();
            }
        }

        private void LinkageDossierVmOnDossierChanged(IDossierFileLinkWrapper dossierFileLinkWrapper)
        {
            LinkageLicenseVm.Initialize(dossierFileLinkWrapper);
        }

        #region AbstractSupervisionStepVm Overrides

        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            LinkageFileCommand.RaiseCanExecuteChanged();
        }

        protected override void StepNext()
        {
            try
            {
                if (DossierFile.IsDirty)
                {
                    NoticeUser.ShowInformation("Необходимо сохранить изменения в томе перед переходом к следующему этапу.");
                    return;
                }

                var scenario = Superviser.DictionaryManager.GetDictionaryItem<Scenario>(DossierFile.ScenarioId);
                if (scenario.ScenarioType == ScenarioType.Light)
                {
                    ChooseResponsibleEmployeeVm.Initialize(DossierFile.Employee,
                                                           Superviser.GetNextScenarioStep());
                    if (UiFactory.ShowDialogView(
                        new ChooseResponsibleEmployeeView(),
                        ChooseResponsibleEmployeeVm,
                        "Переход к следующему этапу"))
                    {
                        var nextResponsibleEmployee =
                            ChooseResponsibleEmployeeVm.EmployeeList.Single(t => t.Id == ChooseResponsibleEmployeeVm.EmployeeId);

                        Superviser.StepNextWithStatus(nextResponsibleEmployee, TaskStatusType.Working);
                        RaiseStepNextRequested();
                        RaiseRebuildRequested(DossierFile);
                    }
                    return;
                }
                base.StepNext();
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }

        protected override bool CanStepNext()
        {
            return base.CanStepNext() && IsLinkaged;
        }

        #endregion


        #region Binding Properties

        private bool _isLinkaged;
        private LinkageHolderVm _linkageHolderVm;
        private LinkageDossierVm _linkageDossierVm;
        private LinkageLicenseVm _linkageLicenseVm;

        public bool IsLinkaged
        {
            get
            {
                return _isLinkaged;
            }
            set
            {
                if (_isLinkaged != value)
                {
                    _isLinkaged = value;
                    RaisePropertyChanged(() => IsLinkaged);
                }
            }
        }

        public LinkageHolderVm LinkageHolderVm
        {
            get { return _linkageHolderVm.IsInitialized? _linkageHolderVm : null; }
            private set { _linkageHolderVm = value; }
        }

        public LinkageDossierVm LinkageDossierVm
        {
            get { return _linkageDossierVm.IsInitialized ? _linkageDossierVm : null; }
            private set { _linkageDossierVm = value; }
        }

        public LinkageLicenseVm LinkageLicenseVm
        {
            get { return _linkageLicenseVm.IsInitialized ? _linkageLicenseVm : null; }
            private set { _linkageLicenseVm = value; }
        }

        #endregion


        #region Binding Commands

        /// <summary>
        /// Команда выполнения привязки тома к лицензионному делу.
        /// </summary>
        public DelegateCommand LinkageFileCommand { get; private set; }

        /// <summary>
        /// Выполненяет привязку тома к лицензионному делу.
        /// </summary>
        private void LinkageFile()
        {
            try
            {
                var dossierFileLinkWrapper = Superviser.Linkage();
                Rebuild(dossierFileLinkWrapper);
                IsLinkaged = true;
                RaiseStepCommandsCanExecute();
            }
            catch (GUException ex)
            {
                IsLinkaged = false;
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                IsLinkaged = false;
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        /// <summary>
        /// Команда отклонения заявки.
        /// </summary>
        public DelegateCommand RejectFileCommand { get; private set; }

        /// <summary>
        /// Отклоняет заявку.
        /// </summary>
        private void RejectFile()
        {
            try
            {
                throw new NotImplementedException();
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

        /// <summary>
        /// Команда открытия окна с отчётом "Опись предоставленных документов"
        /// </summary>
        public DelegateCommand ShowInventoryReportCommand { get; private set; }

        /// <summary>
        /// Открывает окно с отчётом "Опись предоставленных документов"
        /// </summary>
        private void ShowInventoryReport()
        {
            try
            {
                var report = Superviser.Inventory;
                report.InventoryId = DossierFile.Id;
                bool isDesigner = false;
#if DEBUG
                isDesigner = true;
#endif
                AvalonInteractor.RaiseOpenReportDockable(
                    string.Format("Опись документов тома №{0}", DossierFile.RegNumber), report, isDesigner);
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
