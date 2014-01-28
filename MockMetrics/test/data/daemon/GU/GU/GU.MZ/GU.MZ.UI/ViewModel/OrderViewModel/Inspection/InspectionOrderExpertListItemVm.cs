using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Inspection
{
    public class InspectionOrderExpertListItemVm : SmartListItemVm<InspectionOrderExpert>
    {
        #region Binding Properties

        public string ExpertName
        {
            get
            {
                return Entity.ExpertName;
            }
            set
            {
                if (Entity.ExpertName != value)
                {
                    Entity.ExpertName = value;
                    RaisePropertyChanged(() => ExpertName);
                }
            }
        }

        public string ExpertPosition
        {
            get
            {
                return Entity.ExpertPosition;
            }
            set
            {
                if (Entity.ExpertPosition != value)
                {
                    Entity.ExpertPosition = value;
                    RaisePropertyChanged(() => ExpertPosition);
                }
            }
        }

        #endregion
    }
}
