using System.Linq;
using System.Windows.Interactivity;
using Common.UI.View;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;
using GU.ZLP.View;

namespace GU.ZLP.ViewModel.Attachable.Behavior
{
    /// <summary>
    /// Класс-поведения для главного окна приложения, отвечающий за обработку открытых несохранённых документов.
    /// </summary>
    public class MainWindowClosingBehavior : Behavior<MainWindow>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += AssociatedObject_Closing;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Closing -= AssociatedObject_Closing;
        }

        void AssociatedObject_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainVM mainVm = AssociatedObject.DataContext as MainVM;
            foreach (var modVm in mainVm.ModuleVMs)
            {
                if (modVm.View.DataContext is IAvalonDockVM)
                {
                    var avDockVm = modVm.View.DataContext as IAvalonDockVM;
                    foreach (var workVm in avDockVm.Workspaces)
                    {
                        if (workVm is EditableDockableVM)
                        {
                            var editableVm = workVm as EditableDockableVM;
                            e.Cancel = !editableVm.EditableDataContext.OnClosing(editableVm.DisplayName);
                        }
                        if (e.Cancel) break;
                    }
                    if (e.Cancel) break;
                }
            }
        }
    }
}
