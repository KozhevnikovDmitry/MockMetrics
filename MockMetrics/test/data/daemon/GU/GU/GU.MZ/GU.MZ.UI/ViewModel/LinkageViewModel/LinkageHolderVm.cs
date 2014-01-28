using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Common.Types;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.LinkageViewModel
{
    public class LinkageHolderVm : NotificationObject
    {
        private IDossierFileLinkWrapper _fileLinkWrapper;
        private readonly IEntityInfoVm<HolderRequisites> _fromTaskReqVm;
        private readonly IEntityInfoVm<HolderRequisites> _fromRegistrReqVm;

        public LinkageHolderVm(IEntityInfoVm<HolderRequisites> fromTaskReqVm, IEntityInfoVm<HolderRequisites> fromRegistrReqVm)
        {
            IsInitialized = false;
            _fromTaskReqVm = fromTaskReqVm;
            _fromRegistrReqVm = fromRegistrReqVm;
        }

        public void Initialize(IDossierFileLinkWrapper fileLinkWrapper)
        {
            _fileLinkWrapper = fileLinkWrapper;
            RequisitesVms = new Dictionary<string, IEntityInfoVm<HolderRequisites>>();

            if (_fileLinkWrapper.AvailableRequisites.ContainsKey(RequisitesOrigin.FromTask))
            {
                _fromTaskReqVm.Initialize(_fileLinkWrapper.AvailableRequisites[RequisitesOrigin.FromTask]);
                RequisitesVms[RequisitesOrigin.FromTask.GetDescription()] = _fromTaskReqVm;
            }

            if (_fileLinkWrapper.AvailableRequisites.ContainsKey(RequisitesOrigin.FromRegistr))
            {
                _fromRegistrReqVm.Initialize(_fileLinkWrapper.AvailableRequisites[RequisitesOrigin.FromRegistr]);
                RequisitesVms[RequisitesOrigin.FromRegistr.GetDescription()] =_fromRegistrReqVm;
            }

            SelectedRequisitesVm = RequisitesVms.First().Value;

            if (_fileLinkWrapper.IsHolderDataDoubtfull)
            {
                Warning = "ИНН или ОГРН лицензиата и заявления совпадают неполностью";
            }

            IsInitialized = true;
        }

        public void Initialize(DossierFile dossierFile)
        {
            _fileLinkWrapper = null;
            _requisitesVms = new Dictionary<string, IEntityInfoVm<HolderRequisites>>();
            _fromRegistrReqVm.Initialize(dossierFile.HolderRequisites);
            RequisitesVms[RequisitesOrigin.FromRegistr.GetDescription()] = _fromRegistrReqVm;
            SelectedRequisitesVm = RequisitesVms.First().Value;
            IsInitialized = true;
        }

        public void RaiseAllPropertyChanged()
        {
            RaisePropertyChanged(() => RequisitesVms);
            RaisePropertyChanged(() => SelectedRequisitesVm);
        }

        public bool IsInitialized { get; private set; }

        #region Binding Properties

        private IEntityInfoVm<HolderRequisites> _selectedRequisitesVm;

        public IEntityInfoVm<HolderRequisites> SelectedRequisitesVm
        {
            get { return _selectedRequisitesVm; }
            set
            {
                if (_selectedRequisitesVm != value)
                {
                    _selectedRequisitesVm = value;
                    RaisePropertyChanged(() => SelectedRequisitesVm);
                    OnSelectedRequisitesChanged();
                }
            }
        }

        private void OnSelectedRequisitesChanged()
        {
            if (_fileLinkWrapper != null)
            {
                _fileLinkWrapper.SelectedRequisites = SelectedRequisitesVm == null ? null : SelectedRequisitesVm.Entity;
            }
        }

        private string _warning;

        public string Warning
        {
            get { return _warning; }
            set
            {
                if (_warning != value)
                {
                    _warning = value;
                    RaisePropertyChanged(() => Warning);
                    RaisePropertyChanged(() => HasWarning);
                }
            }
        }

        private Dictionary<string, IEntityInfoVm<HolderRequisites>> _requisitesVms;

        public Dictionary<string, IEntityInfoVm<HolderRequisites>> RequisitesVms
        {
            get { return _requisitesVms; }
            set
            {
                if (_requisitesVms != value)
                {
                    _requisitesVms = value;
                    RaisePropertyChanged(() => RequisitesVms);
                }
            }
        }

        public bool HasWarning
        {
            get
            {
                return !string.IsNullOrEmpty(Warning);
            }
        }

        #endregion
    }
}
