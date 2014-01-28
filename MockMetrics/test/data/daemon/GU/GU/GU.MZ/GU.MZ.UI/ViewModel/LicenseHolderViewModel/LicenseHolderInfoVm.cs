using Common.BL.DataMapping;
using GU.MZ.DataModel.Holder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseHolderViewModel
{
    public class LicenseHolderInfoVm : EntityInfoVm<LicenseHolder>
    {
        private readonly HolderRequisitesInfoVm _requisitesVm;

        public LicenseHolderInfoVm(IDomainDataMapper<LicenseHolder> entityMapper, HolderRequisitesInfoVm requisitesVm) 
            : base(entityMapper)
        {
            _requisitesVm = requisitesVm;
        }

        public override void Initialize(LicenseHolder entity)
        {
            base.Initialize(entity);
            EntityMapper.FillAssociations(Entity);
            _requisitesVm.Initialize(Entity.ActualRequisites);
        }

        #region Binding Properties

        private string _requisitesOriginInfo;

        public string RequisitesOriginInfo
        {
            get
            {
                return _requisitesVm.RequisitesOriginInfo;
            }
            set
            {
                if (_requisitesVm.RequisitesOriginInfo != value)
                {
                    _requisitesVm.RequisitesOriginInfo = value;
                    RaisePropertyChanged(() => RequisitesOriginInfo);
                }
            }
        }

        public string Inn
        {
            get
            {
                return _requisitesVm.Inn;
            }
        }

        public string Ogrn
        {
            get
            {
                return _requisitesVm.Ogrn;
            }
        }

        public string FullName
        {
            get
            {
                return _requisitesVm.FullName;
            }
        }

        public string ShortName
        {
            get
            {
                return _requisitesVm.ShortName;
            }
        }

        public string FirmName
        {
            get
            {
                return _requisitesVm.ShortName;
            }
        }

        private string _legalFormName;

        public string LegalFormName
        {
            get
            {
                return _requisitesVm.LegalFormName;
            }
        }

        public string HeadName
        {
            get
            {
                return _requisitesVm.HeadName;
            }
        }

        public string HeadPositionName
        {
            get
            {
                return _requisitesVm.HeadPositionName;
            }
        }

        public string Address
        {
            get
            {
                return _requisitesVm.Address;
            }
        }

        #endregion
    }
}
