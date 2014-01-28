using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.DeclarerViewModel
{
    public class DeclarerBaseRegSummaryVM : HqValidateableVM
    {
        private readonly Claim _claim;

        public DeclarerBaseRegSummaryVM (Claim claim)
        {
            _claim = claim;
            DeclarerBaseRegVm = new DeclarerBaseRegVM(_claim);
        }

        public override void RaiseValidatingPropertyChanged()
        {
            DeclarerBaseRegVm.RaiseItemsValidatingPropertyChanged();
        }

        /// <summary>
        /// Информация о основаниях учета указанных заявителем
        /// </summary>
        private DeclarerBaseRegVM _declarerBaseRegVm;
        public DeclarerBaseRegVM DeclarerBaseRegVm
        {
            get
            {
                return _declarerBaseRegVm;
            }
            set
            {
                if (_declarerBaseRegVm != value)
                {
                    _declarerBaseRegVm = value;
                    RaisePropertyChanged(() => DeclarerBaseRegVm);
                }
            }
        }

        /// <summary>
        /// Иное основание для учета
        /// </summary>
        public string OtherBaseReg
        {
            get { return _claim.DeclarerBaseReg == null ? "" : _claim.DeclarerBaseReg.OtherBaseReg; }
            set
            {
                if (value != null && !_claim.DeclarerBaseReg.OtherBaseReg.Equals(value))
                    _claim.DeclarerBaseReg.OtherBaseReg = value;
            }
        }
    }
}
