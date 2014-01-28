using System;
using System.Collections.Generic;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    public class LicenseRequisitesVm : SmartValidateableVm<LicenseRequisites>
    {
        public LicenseRequisitesVm(ISmartValidateableVm<Address> addressVm,
                                   ISmartValidateableVm<IndRequisites> indRequisitesVm,
                                   ISmartValidateableVm<JurRequisites> jurRequisitesVm)
        {
            AddressVm = addressVm;
            _indRequisitesVm = indRequisitesVm;
            _jurRequisitesVm = jurRequisitesVm;
            AddChild(_indRequisitesVm);
            AddChild(_jurRequisitesVm);
            AddChild(AddressVm);
            JurIndReqTypes = new Dictionary<bool, string>();
            JurIndReqTypes[true] = "Физическое лицо";
            JurIndReqTypes[false] = "Юридическое лицо";
        }

        public override void Initialize(LicenseRequisites entity)
        {
            base.Initialize(entity);
            AddressVm.Initialize(Entity.Address);
            _isIndividual = Entity.IndRequisites != null;
            if (Entity.JurRequisites != null)
            {
                _jurRequisitesVm.Initialize(Entity.JurRequisites);
            }
            else
            {
                _indRequisitesVm.Initialize(Entity.IndRequisites);
            }
        }

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => IsIndividual);
            RaisePropertyChanged(() => Note);
        }

        private void ChangeForm()
        {
            try
            {
                if (IsIndividual)
                {
                    if (Entity.IndRequisites == null)
                    {
                        Entity.IndRequisites = IndRequisites.CreateInstance();
                    }
                    Entity.JurRequisites = null;
                    Entity.JurRequisitesId = null;

                    _indRequisitesVm.Initialize(Entity.IndRequisites);
                    RaisePropertyChanged(() => IndRequisitesVm);
                }
                else
                {
                    if (Entity.JurRequisites == null)
                    {
                        Entity.JurRequisites = JurRequisites.CreateInstance();
                    }
                    Entity.IndRequisites = null;
                    Entity.IndRequisitesId = null;

                    _jurRequisitesVm.Initialize(Entity.JurRequisites);
                    RaisePropertyChanged(() => JurRequisitesVm);
                } 
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

        #region Binding Properties

        private bool _isIndividual;

        public bool IsIndividual
        {
            get { return _isIndividual; }
            set
            {
                if (_isIndividual != value)
                {
                    _isIndividual = value;
                    ChangeForm();
                    RaisePropertyChanged(() => IsIndividual);
                }
            }
        }

        public bool CanChangeForm
        {
            get
            {
                return Entity.Id == 0;
            }
        }

        public DateTime CreateDate
        {
            get { return Entity.CreateDate; }
            set
            {
                if (Entity.CreateDate != value)
                {
                    Entity.CreateDate = value;
                    RaisePropertyChanged(() => CreateDate);
                }
            }
        }

        public string Note
        {
            get { return Entity.Note; }
            set
            {
                if (Entity.Note != value)
                {
                    Entity.Note = value;
                    RaisePropertyChanged(() => Note);
                }
            }
        }

        public Dictionary<bool, string> JurIndReqTypes { get; set; }

        public ISmartValidateableVm<Address> AddressVm { get; private set; }

        private ISmartValidateableVm<JurRequisites> _jurRequisitesVm;

        public ISmartValidateableVm<JurRequisites> JurRequisitesVm
        {
            get
            {
                return _jurRequisitesVm.Entity != null ? _jurRequisitesVm : null;
            }
        }

        private ISmartValidateableVm<IndRequisites> _indRequisitesVm;

        public ISmartValidateableVm<IndRequisites> IndRequisitesVm
        {
            get
            {
                return _indRequisitesVm.Entity != null ? _indRequisitesVm : null;
            }
             
        }

        #endregion

    }
}
