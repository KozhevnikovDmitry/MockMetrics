using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.DossierFileViewModel;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    public class GrantNewLicenseStepVm : GrantResultStepVm
    {
        private readonly IEntityInfoVmFactory _entityInfoVmFactory;

        public GrantNewLicenseStepVm(DossierFileServiceResultVm dossierFileServiceResultVm, 
                                     IEntityInfoVmFactory entityInfoVmFactory,
                                     IEntityInfoVm<License> newLicenseVm) 
            : base(dossierFileServiceResultVm)
        {
            NewLicenseVm = newLicenseVm;
            _entityInfoVmFactory = entityInfoVmFactory;
            GrantNewLicenseCommand = new DelegateCommand(GrantNewLicense, CanGrantNewLicense);
            GoToLicenseCommand = new DelegateCommand(GoToLicense, CanGoToLicense);
        }

        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            GrantNewLicenseCommand.RaiseCanExecuteChanged();
            GoToLicenseCommand.RaiseCanExecuteChanged();
        }

        protected override void Rebuild()
        {
            base.Rebuild();
            RebuildLicense();
        }

        private void RebuildLicense()
        {
            HasLicense = DossierFile.License != null;

            if (DossierFile.CurrentFileStep != null && HasLicense)
            {
                _newLicenseVm.Initialize(DossierFile.License);
                RaisePropertyChanged(() => NewLicenseVm);
            }
        }

        #region Binding Properties

        private bool _hasLicense;

        public bool HasLicense
        {
            get { return _hasLicense; }
            set
            {
                if (_hasLicense != value)
                {
                    _hasLicense = value;
                    RaisePropertyChanged(() => HasLicense);
                }
            }

        }

        private IEntityInfoVm<License> _newLicenseVm;

        public IEntityInfoVm<License> NewLicenseVm
        {
            get { return _newLicenseVm.Entity == null? null : _newLicenseVm; }
            set
            {
                if (_newLicenseVm != value)
                {
                    _newLicenseVm = value;
                    RaisePropertyChanged(() => NewLicenseVm);
                }
            }

        }

        #endregion

        #region Binding Commands

        public DelegateCommand GrantNewLicenseCommand { get; private set; }

        private void GrantNewLicense()
        {
            try
            {
                if (DossierFile.IsRejected)
                {
                    var dlr = NoticeUser.ShowQuestionYesNo("Заявка была отклонена. Всё равно завести лицензию?");
                    if (dlr != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

                Superviser.GetNewLicense();
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

        private bool CanGrantNewLicense()
        {
            try
            {
                return IsCurrentOrPrevious && !HasLicense;
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
                return HasLicense && DossierFile.LicenseId != 0 && DossierFile.License != null;
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