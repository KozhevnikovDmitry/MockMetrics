using System.Windows;

using Common.UI.ViewModel.Attachables.Properties;

using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс класса ViewModel для окна диалога OK\CANCEL.
    /// </summary>
    public interface IDialogVM : IWorkspaceVM
    {
        /// <summary>
        /// Флаг указывающий на результат диалога.
        /// </summary>
        bool IsOkResult { get; }

        /// <summary>
        /// Команда проставления положительного результата диалога.
        /// </summary>
        DelegateCommand OkCommand { get; }

        /// <summary>
        /// Поле привязки для закрытия диалога. 
        /// </summary>
        /// <remarks>
        /// Работает в связке с AttachedProperty <see cref="CloseViewBehavior"/>
        /// </remarks>
        bool CloseView { get; set; }

        /// <summary>
        /// Возвращает или устанавливает режим изменения размеров окна
        /// </summary>
        ResizeMode DialogResizeMode { get; set; }

        /// <summary>
        /// Возвращает или устанавливает режим привязки размеров к контенту
        /// </summary>
        SizeToContent DialogSizeToContentMode { get; set; }
    }
}
