using System.Windows.Controls;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimQueuePrivListVM : AbstractListVM<QueuePriv>
    {
        public ClaimQueuePrivListVM(Claim claim) :
            base(claim.QueuePrivList)
        {
        }

        protected override void SetListOptions()
        {
            base.SetListOptions();
            CanAddItems = false;
            HorisontalScrollVisibility = ScrollBarVisibility.Disabled;
        }
    }
}