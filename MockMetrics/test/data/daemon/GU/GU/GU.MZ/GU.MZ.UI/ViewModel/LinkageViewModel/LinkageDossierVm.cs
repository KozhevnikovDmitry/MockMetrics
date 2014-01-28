using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Common.Types;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.LinkageViewModel
{
    public class LinkageDossierVm : NotificationObject
    {
        private DossierFile _dossierFile;
        private IDossierFileLinkWrapper _fileLinkWrapper;
        private readonly IEntityInfoVmFactory _uiFactory;
        private readonly LicenseDossierRepository _dossierRepository;

        public LinkageDossierVm(IEntityInfoVmFactory uiFactory, LicenseDossierRepository dossierRepository)
        {
            IsInitialized = false;
            _uiFactory = uiFactory;
            _dossierRepository = dossierRepository;
        }

        public void Initialize(IDossierFileLinkWrapper fileLinkWrapper)
        {
            _fileLinkWrapper = fileLinkWrapper;
            _dossierFile = _fileLinkWrapper.DossierFile;
            LicenseDossierVms = new List<IEntityInfoVm<LicenseDossier>>();
            LicenseDossierVms.Add(_uiFactory.GetEntityInfoVm(_fileLinkWrapper.LicenseDossier));

            foreach (var dossier in _fileLinkWrapper.LicenseHolder
                                                   .DossierList
                                                   .Where(t => !t.Equals(_fileLinkWrapper.LicenseDossier))
                                                   .Where(t => t.IsActive)
                                                   .Where(t => t.LicensedActivityId == _fileLinkWrapper.DossierFile.LicensedActivity.Id))
            {
                LicenseDossierVms.Add(_uiFactory.GetEntityInfoVm(dossier));
            }
            SelectedDossierVm = LicenseDossierVms.First();

            InventoryRegNumber = _fileLinkWrapper.DossierFile.DocumentInventory.RegNumber;
            
            IsInitialized = true;
        }

        public void Initialize(DossierFile dossierFile)
        {
           _fileLinkWrapper = null;
            _dossierFile = dossierFile;
            LicenseDossierVms = new List<IEntityInfoVm<LicenseDossier>>();
            LicenseDossierVms.Add(_uiFactory.GetEntityInfoVm(dossierFile.LicenseDossier));
            IsInitialized = true;
            SelectedDossierVm = LicenseDossierVms.First();
            InventoryRegNumber = dossierFile.DocumentInventory.RegNumber;
        }

        public void RaiseAllPropertyChanged()
        {
            RaisePropertyChanged(() => LicenseDossierVms);
            RaisePropertyChanged(() => SelectedDossierVm);
        }

        public bool IsInitialized { get; private set; }

        #region Events

        public event Action<IDossierFileLinkWrapper> DossierChanged;

        protected virtual void OnDossierChanged(IDossierFileLinkWrapper obj)
        {
            Action<IDossierFileLinkWrapper> handler = DossierChanged;
            if (handler != null) handler(obj);
        }

        #endregion

        #region Binding Properties

        public List<IEntityInfoVm<LicenseDossier>> LicenseDossierVms { get; set; }

        private IEntityInfoVm<LicenseDossier> _selectedDossierVm;

        public IEntityInfoVm<LicenseDossier> SelectedDossierVm
        {
            get { return _selectedDossierVm; }
            set
            {
                if (_selectedDossierVm != value)
                {
                    _selectedDossierVm = value;
                    RaisePropertyChanged(() => SelectedDossierVm);
                    OnSelectedDossierChanged();
                    ChangeFileRegNumber();
                }
            }
             
        }

        private void OnSelectedDossierChanged()
        {
            if (_fileLinkWrapper != null)
            {
                _fileLinkWrapper.LicenseDossier = SelectedDossierVm.Entity;
                OnDossierChanged(_fileLinkWrapper);
            }
        }

        private void ChangeFileRegNumber()
        {
            try
            {
                if (_fileLinkWrapper != null)
                {
                    FileRegNumber = _dossierRepository.GetNextFileRegNumber(SelectedDossierVm.Entity);
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

        public int FileRegNumber
        {
            get { return _dossierFile.RegNumber; }
            set
            {
                if (_dossierFile.RegNumber != value)
                {
                    _dossierFile.RegNumber = value;
                    RaisePropertyChanged(() => FileRegNumber);
                }
            }
        }

        public int InventoryRegNumber { get; private set; }

        #endregion
    }
}
