using Common.UI;
using GU.UI.ViewModel.TaskViewModel;

namespace GU.MZ.DS.UI.ViewModel
{
    /// <summary>
    /// Класс VM для View модуля работы с заявлениями.
    /// </summary>
    public class TaskModuleVM : BaseTaskManagementVM
    {
        /// <summary>
        /// Класс VM для модуля работы с заявлениями.
        /// </summary>
        public TaskModuleVM()
            : base(new SingletonDockableUiFactory())
        {
            
        }
    }
}
