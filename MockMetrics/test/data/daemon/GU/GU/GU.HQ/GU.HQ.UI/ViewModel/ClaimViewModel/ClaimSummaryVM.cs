using GU.HQ.DataModel;
using GU.HQ.UI.ViewModel.DeclarerViewModel;


namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimSummaryVM
    {

        public ClaimSummaryVM (Claim claim)
        {
            _declarerShortVm = new DeclarerShortVM(claim);
            _claimSummaryShortVm = new ClaimSummaryShortVM(claim);
        }

        #region Binding Properties

        private DeclarerShortVM _declarerShortVm;
        public DeclarerShortVM DeclarerShortVm
        {
            get { return _declarerShortVm; }
        }

        private ClaimSummaryShortVM _claimSummaryShortVm;
        public ClaimSummaryShortVM ClaimSummaryShortVm        
        {
            get { return _claimSummaryShortVm; }
        }
        
        #endregion Binding Properties
    }
}
