using System;

using AvalonDock;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс классов ViewModel для вкладок AvalonDock
    /// </summary>
    public interface IDockableVM : IWorkspaceVM
    {
        void Close();

        event EventHandler<DocumentClosingEventArgs> OnClosing;

        event Action<IDockableVM> OnProgramClosing;

        void RaiseOnClosing(DocumentClosingEventArgs e);
    }
}
