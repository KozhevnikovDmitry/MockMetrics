using System.Collections.Generic;
using Common.BL.DictionaryManagement;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    public class JurRequisitesVm : DomainValidateableVM<JurRequisites>
    {
        public JurRequisitesVm(JurRequisites entity, IDomainValidator<JurRequisites> domainValidator, IDictionaryManager dictionaryManager, bool isValidateable = true) 
            : base(entity, domainValidator, isValidateable)
        {
            Initialize(dictionaryManager);
        }

        private void Initialize(IDictionaryManager dictionaryManager)
        {
            LegalForms = dictionaryManager.GetDictionary<LegalForm>();
        }

        #region Binding Properties

        public string FullName
        {
            get { return Entity.FullName; }
            set
            {
                if (Entity.FullName != value)
                {
                    Entity.FullName = value;
                    RaisePropertyChanged(() => FullName);
                }
            }
        }

        public string ShortName
        {
            get { return Entity.ShortName; }
            set
            {
                if (Entity.ShortName != value)
                {
                    Entity.ShortName = value;
                    RaisePropertyChanged(() => ShortName);
                }
            }
        }

        public string FirmName
        {
            get { return Entity.FirmName; }
            set
            {
                if (Entity.FirmName != value)
                {
                    Entity.FirmName = value;
                    RaisePropertyChanged(() => FirmName);
                }
            }
        }

        public string HeadName
        {
            get { return Entity.HeadName; }
            set
            {
                if (Entity.HeadName != value)
                {
                    Entity.HeadName = value;
                    RaisePropertyChanged(() => HeadName);
                }
            }
        }

        public string HeadPositionName
        {
            get { return Entity.HeadPositionName; }
            set
            {
                if (Entity.HeadPositionName != value)
                {
                    Entity.HeadPositionName = value;
                    RaisePropertyChanged(() => HeadPositionName);
                }
            }
        }

        public int LegalFormId
        {
            get
            {
                return Entity.LegalFormId;
            }
            set
            {
                if (Entity.LegalFormId != value)
                {
                    Entity.LegalFormId = value;
                    RaisePropertyChanged(() => LegalFormId);
                }
            }
        }

        public List<LegalForm> LegalForms { get; set; }

        #endregion
    }
}
