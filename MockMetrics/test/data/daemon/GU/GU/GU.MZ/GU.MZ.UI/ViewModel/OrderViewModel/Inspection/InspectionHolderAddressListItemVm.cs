using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Inspection
{
    public class InspectionHolderAddressListItemVm : SmartListItemVm<InspectionHolderAddress>
    {
        #region Binding Properties

        public string Address
        {
            get
            {
                return Entity.Address;
            }
            set
            {
                if (Entity.Address != value)
                {
                    Entity.Address = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }

        #endregion
    }
}
