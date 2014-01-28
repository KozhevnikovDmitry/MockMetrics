using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Inspection
{
    public class InspectionOrderAgreeListItemVm : SmartListItemVm<InspectionOrderAgree>
    {
        #region Binding Properties

        public string EmployeeName
        {
            get
            {
                return Entity.EmployeeName;
            }
            set
            {
                if (Entity.EmployeeName != value)
                {
                    Entity.EmployeeName = value;
                    RaisePropertyChanged(() => EmployeeName);
                }
            }
        }

        public string EmployeePosition
        {
            get
            {
                return Entity.EmployeePosition;
            }
            set
            {
                if (Entity.EmployeePosition != value)
                {
                    Entity.EmployeePosition = value;
                    RaisePropertyChanged(() => EmployeePosition);
                }
            }
        }

        #endregion
    }
}
