using Common.UI;
using GU.UI.ViewModel.TaskViewModel;

namespace GU.Trud.UI.ViewModel
{
    public class TrudManagementVM : BaseTaskManagementVM
    {
        public TrudManagementVM()
            : base(new SingletonDockableUiFactory())
        {
            
        }
    }
}

