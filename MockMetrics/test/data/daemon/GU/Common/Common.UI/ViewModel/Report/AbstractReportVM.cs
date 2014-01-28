using System.Windows.Forms.Integration;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.Report
{
    /// <summary>
    /// Базовый класс для классов ViewModel для отображения просмотрщика отчётов на Windows Forms. 
    /// </summary>
    public class AbstractReportVM : NotificationObject, IReportVM
    {
        /// <summary>
        /// Базовый класс для классов ViewModel для отображения просмотрщика отчётов на Windows Forms. 
        /// </summary>
        public AbstractReportVM()
        {
            WindowsFormsHost = new WindowsFormsHost();
        }

        /// <summary>
        /// Контрол для отображения Windows Forms компонентов.
        /// </summary>
        public WindowsFormsHost WindowsFormsHost { get; set; }
    }
}
