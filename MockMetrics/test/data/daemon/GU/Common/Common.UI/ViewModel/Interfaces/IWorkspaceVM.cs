using System.ComponentModel;
using System.Windows.Controls;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс классов  моделей-представления для отображения рабочих пространств.
    /// </summary>
    public interface IWorkspaceVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Возвращает или устанавливает наименование рабочего пространства.
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Возвращает или устанавливает Представление рабочего пространства.
        /// </summary>
        UserControl View { get; set; }
    }
}
