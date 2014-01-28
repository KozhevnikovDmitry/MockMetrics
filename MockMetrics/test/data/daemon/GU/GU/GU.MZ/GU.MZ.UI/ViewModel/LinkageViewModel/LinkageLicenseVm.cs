using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Common.Types;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.ViewModel;
using License = GU.MZ.DataModel.Licensing.License;

namespace GU.MZ.UI.ViewModel.LinkageViewModel
{
    public class LinkageLicenseVm : NotificationObject
    {
        private IDossierFileLinkWrapper _fileLinkWrapper;
        private readonly IEntityInfoVmFactory _uiFactory;

        public LinkageLicenseVm(IEntityInfoVmFactory uiFactory)
        {
            IsInitialized = false;
            _uiFactory = uiFactory;
        }

        public void Initialize(IDossierFileLinkWrapper fileLinkWrapper)
        {
            _fileLinkWrapper = fileLinkWrapper;
            HasLicense = (!_fileLinkWrapper.DossierFile.IsNewLicense && _fileLinkWrapper.License != null) || _fileLinkWrapper.DossierFile.License != null;

            if (HasLicense)
            {
                LicenseVms = new List<IEntityInfoVm<License>> { _uiFactory.GetEntityInfoVm(_fileLinkWrapper.License) };

                foreach (var license in _fileLinkWrapper.LicenseDossier
                    .LicenseList
                    .Where(t => t.Id != _fileLinkWrapper.License.Id)
                    .Where(t => t.LicensedActivityId == _fileLinkWrapper.DossierFile.LicensedActivity.Id))
                {
                    LicenseVms.Add(_uiFactory.GetEntityInfoVm(license));
                }

                SelectedLicenseVm = LicenseVms.First();
            }

            IsInitialized = true;
        }

        public void Initialize(DossierFile dossierFile)
        {
            _fileLinkWrapper = null;
            HasLicense = dossierFile.License != null;

            if (HasLicense)
            {
                LicenseVms = new List<IEntityInfoVm<License>>();
                LicenseVms.Add(_uiFactory.GetEntityInfoVm(dossierFile.License));
                IsInitialized = true;
                SelectedLicenseVm = LicenseVms.First();
            }

            IsInitialized = true;
        }

        public void RaiseAllPropertyChanged()
        {
            RaisePropertyChanged(() => LicenseVms);
            RaisePropertyChanged(() => SelectedLicenseVm);
        }

        public bool IsInitialized { get; private set; }

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

        public List<IEntityInfoVm<License>> LicenseVms { get; set; }

        private IEntityInfoVm<License> _selectedLicenseVm;

        public IEntityInfoVm<License> SelectedLicenseVm
        {
            get
            {
                return _selectedLicenseVm;
            }
            set
            {
                if (!Equals(value, _selectedLicenseVm))
                {
                    _selectedLicenseVm = value;
                    RaisePropertyChanged(() => SelectedLicenseVm);
                    OnSelectedLicenseChanged();
                }
            }
        }

        private void OnSelectedLicenseChanged()
        {
            if (_fileLinkWrapper != null)
            {
                _fileLinkWrapper.License = SelectedLicenseVm.Entity;
            }
        }

        #endregion
    }
}
