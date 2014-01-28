using System;

using AvalonDock;

using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.WorkspaceViewModel
{
    /// <summary>
    /// Класс представлющий Модель Представления рабочей области AvalonDock конрола DockingManager.
    /// </summary>
    public class DockableVM : BaseWorkspaceVM, IDockableVM
    {
        public void Close()
        {
            if (OnProgramClosing != null)
            {
                OnProgramClosing(this);
            }
        }

        public event EventHandler<DocumentClosingEventArgs> OnClosing;

        public event Action<IDockableVM> OnProgramClosing;
        
        public void RaiseOnClosing(DocumentClosingEventArgs e)
        {
            if (OnClosing != null)
            {
                OnClosing(this, e);
            }
        }
    }
}
