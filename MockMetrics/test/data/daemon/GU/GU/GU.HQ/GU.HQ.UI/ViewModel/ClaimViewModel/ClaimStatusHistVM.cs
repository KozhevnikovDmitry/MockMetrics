using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Common.Types;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimStatusHistVM : AbstractListVM<ClaimStatusHist>
    {
        public ClaimStatusHistVM(Claim claim) 
            : base(claim.ClaimStatusHist)
        { 
        }

        protected override void SetListOptions()
        {
            base.SetListOptions();
            CanAddItems = false;
            CanSort = true;
            SortDirection = ListSortDirection.Descending;
            var item = ListItemVMs.FirstOrDefault() as ClaimStatusHistItemVM;
            if (item != null)
            {
                SortProperties.Add(Util.GetPropertyName(() => item.DateSetStatus));
            }
        }
    }
}