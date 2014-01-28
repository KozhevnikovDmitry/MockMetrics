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
    public class RenewalLicenseStepVm : GrantResultStepVm
    {
        public RenewalLicenseStepVm(DossierFileServiceResultVm dossierFileServiceResultVm, 
                                    IEntityInfoVm<License> renewaledLicenseVm,
                                    SupervisionFacade superviser) 
            : base(dossierFileServiceResultVm)
        {
            RenewaledLicenseVm = renewaledLicenseVm;
            RenewalLicenseCommand = new DelegateCommand(RenewalLicense, CanRenewalLicense);
            GoToLicenseCommand = new DelegateCommand(GoToLicense, CanGoToLicense);
            IsRenewaled = superviser.IsRenewalled();
        }

        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            RenewalLicenseCommand.RaiseCanExecuteChanged();
            GoToLicenseCommand.RaiseCanExecuteChanged();
        }

        protected override void Rebuild()
        {
            base.Rebuild();
            RebuildLicense();
        }

        private void RebuildLicense()
        {
            if (DossierFile.CurrentFileStep != null && IsRenewaled)
            {
                _renewaledLicenseVm.Initialize(DossierFile.License);
                RaisePropertyChanged(() => RenewaledLicenseVm);
            }
        }
        
        #region Binding Properties
        
        private bool _isRenewaled;

        public bool IsRenewaled
        {
            get { return _isRenewaled; }
            set
            {
                if (_isRenewaled != value)
                {
                    _isRenewaled = value;
                    RaisePropertyChanged(() => IsRenewaled);
                }
            }
        }

        private IEntityInfoVm<License> _renewaledLicenseVm;

        public IEntityInfoVm<License> RenewaledLicenseVm
        {
            get { return _renewaledLicenseVm.Entity == null ? null : _renewaledLicenseVm; }
            set
            {
                if (_renewaledLicenseVm != value)
                {
                    _renewaledLicenseVm = value;
                    RaisePropertyChanged(() => RenewaledLicenseVm);
                }
            }

        }

        #endregion

        #region Binding Commands

        public DelegateCommand RenewalLicenseCommand { get; private set; }

        private void RenewalLicense()
        {
            try
            {
                if (DossierFile.IsRejected)
                {
                    var dlr = NoticeUser.ShowQuestionYesNo("Заявка была отклонена. Всё равно переоформить лицензию?");
                    if (dlr != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

                try
                {
                    Superviser.RenewalLicense();
                    IsRenewaled = true;
                    RebuildLicense();
                }
                catch (NotImplementedException)
                {
                    NoticeUser.ShowWarning("Сценарий переоформления не реализован. Лицензию необходимо переоформить вручную.");
                }

                RaiseStepCommandsCanExecute();
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

        private bool CanRenewalLicense()
        {
            try
            {
                return IsCurrentOrPrevious && !IsRenewaled;
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
                return DossierFile.LicenseId != 0 && DossierFile.License != null && IsRenewaled;
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