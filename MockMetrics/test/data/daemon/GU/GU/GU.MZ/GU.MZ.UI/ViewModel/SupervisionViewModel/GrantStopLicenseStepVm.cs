using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.DossierFileViewModel;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    public class GrantStopLicenseStepVm : GrantResultStepVm
    {
        public GrantStopLicenseStepVm(DossierFileServiceResultVm dossierFileServiceResultVm, IEntityInfoVm<License> licenseInfoVm) 
            : base(dossierFileServiceResultVm)
        {
            StoppedLicenseVm = licenseInfoVm;
            GrantStopLicenseCommand = new DelegateCommand(GrantStopLicense, CanGrantStopLicense);
            GoToLicenseCommand = new DelegateCommand(GoToLicense, CanGoToLicense);
        }

        public override void CustInit(SupervisionFacade superviser)
        {
            base.CustInit(superviser);
            HasStoppedLicense = DossierFile.License != null && DossierFile.License.IsStopped;
        }

        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            GrantStopLicenseCommand.RaiseCanExecuteChanged();
            GoToLicenseCommand.RaiseCanExecuteChanged();
        }

        protected override void Rebuild()
        {
            base.Rebuild();
            RebuildLicense();
        }

        private void RebuildLicense()
        {
            if (DossierFile.CurrentFileStep != null && HasStoppedLicense)
            {
                _stoppedLicenseVm.Initialize(DossierFile.License);
                RaisePropertyChanged(() => StoppedLicenseVm);
            }
        }

        #region Binding Properties

        private bool _hasStoppedLicense;

        public bool HasStoppedLicense
        {
            get { return _hasStoppedLicense; }
            set
            {
                if (_hasStoppedLicense != value)
                {
                    _hasStoppedLicense = value;
                    RaisePropertyChanged(() => HasStoppedLicense);
                }
            }

        }

        private IEntityInfoVm<License> _stoppedLicenseVm;

        public IEntityInfoVm<License> StoppedLicenseVm
        {
            get { return _stoppedLicenseVm.Entity == null? null : _stoppedLicenseVm; }
            private set { _stoppedLicenseVm = value; }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand GrantStopLicenseCommand { get; private set; }

        private void GrantStopLicense()
        {
            try
            {
                if (DossierFile.IsRejected)
                {
                    var dlr = NoticeUser.ShowQuestionYesNo("Заявка была отклонена. Всё равно прекратить действие лицензии?");
                    if (dlr != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

                Superviser.StopLicense();
                HasStoppedLicense = true;
                RebuildLicense();
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

        private bool CanGrantStopLicense()
        {
            try
            {
                return IsCurrentOrPrevious && DossierFile.License != null && !HasStoppedLicense;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
                return false;
            }
        }

        public DelegateCommand GoToLicenseCommand { get; private set; }

        private void GoToLicense()
        {
            try
            {
                AvalonInteractor.RaiseOpenEditableDockable(DossierFile.License.ToString(), typeof(License), DossierFile.License);
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

        private bool CanGoToLicense()
        {
            try
            {
                return HasStoppedLicense && DossierFile.LicenseId != 0 && DossierFile.License != null;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
                return false;
            }
        }

        #endregion
    }
}
