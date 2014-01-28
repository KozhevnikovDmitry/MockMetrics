using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Holder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseHolderViewModel
{
    public class HolderRequisitesInfoVm : EntityInfoVm<HolderRequisites>
    {
        private readonly IDictionaryManager _dictionaryManager;
        
        public HolderRequisitesInfoVm(IDomainDataMapper<HolderRequisites> entityMapper, IDictionaryManager dictionaryManager) 
            : base(entityMapper)
        {
            _dictionaryManager = dictionaryManager;
        }

        public override void Initialize(HolderRequisites entity)
        {
            base.Initialize(entity);
            if (Entity.JurRequisites != null)
            {
                LegalFormName =
                    _dictionaryManager.GetDictionaryItem<LegalForm>(Entity.JurRequisites.LegalFormId).Name;
            }
            else
            {
                LegalFormName = "Индивидуальные предприниматель";
            }
        }

        #region Binding Properties

        private string _requisitesOriginInfo;

        public string RequisitesOriginInfo
        {
            get
            {
                return _requisitesOriginInfo;
            }
            set
            {
                if (_requisitesOriginInfo != value)
                {
                    _requisitesOriginInfo = value;
                    RaisePropertyChanged(() => RequisitesOriginInfo);
                }
            }
        }

        public string Inn
        {
            get
            {
                return Entity.LicenseHolder.Inn;
            }
        }

        public string Ogrn
        {
            get
            {
                return Entity.LicenseHolder.Ogrn;
            }
        }

        public string FullName
        {
            get
            {
                return Entity.FullName;
            }
        }

        public string ShortName
        {
            get
            {
                return Entity.ShortName;
            }
        }

        public string FirmName
        {
            get
            {
                return Entity.ShortName;
            }
        }

        private string _legalFormName;

        public string LegalFormName
        {
            get
            {
                return _legalFormName;
            }
            private set
            {
                if (_legalFormName != value)
                {
                    _legalFormName = value;
                    RaisePropertyChanged(() => LegalFormName);
                }
            }
             
        }

        public string HeadName
        {
            get
            {
                return Entity.HeadName;
            }
        }

        public string HeadPositionName
        {
            get
            {
                return Entity.HeadPositionName;
            }
        }

        public string Address
        {
            get
            {
                return Entity.Address.ToLongString();
            }
        }

        #endregion
    }
}
