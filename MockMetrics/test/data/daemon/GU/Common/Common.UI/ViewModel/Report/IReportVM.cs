using System.ComponentModel;
using System.Windows.Forms.Integration;

namespace Common.UI.ViewModel.Report
{
    /// <summary>
    /// Класс ViewModel для отображения просмотрщика отчётов на Windows Forms
    /// </summary>
    public interface IReportVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Контрол для отображения Windows Forms компонентов.
        /// </summary>
        WindowsFormsHost WindowsFormsHost { get; }
    }
}
