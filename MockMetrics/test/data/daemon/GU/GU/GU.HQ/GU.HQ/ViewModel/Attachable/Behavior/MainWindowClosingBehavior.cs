using System.ComponentModel;
using System.Linq;
using System.Windows.Interactivity;

using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;

using GU.HQ.View;

namespace GU.HQ.ViewModel.Attachable.Behavior
{
    /// <summary>
    /// Класс-поведения для главного окна приложения, отвечающий за обработку открытых несохранённых документов.
    /// </summary>
    public class MainWindowClosingBehavior : Behavior<MainWindow>
    {
        /// <summary>
        /// Обрабатывает событие внедрения поведения.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Closing += this.AssociatedObjectClosing;
        }

        /// <summary>
        /// Обрабатывает событие отключения поведения.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Closing -= this.AssociatedObjectClosing;
        }

        /// <summary>
        /// Обрабатывает событие <c>MainWindow.Closing</c>.
        /// </summary>
        /// <param name="sender">Отправитель события</param>
        /// <param name="e">Аргументы собятия</param>
        private void AssociatedObjectClosing(object sender, CancelEventArgs e)
        {
            var mainVm = this.AssociatedObject.DataContext as MainVM;
            if (mainVm == null)
            {
                return;
            }

            foreach (var modVm in mainVm.ModuleVmList)
            {
                if (modVm.View.DataContext is IAvalonDockVM)
                {
                    var dockVm = modVm.View.DataContext as IAvalonDockVM;
                    foreach (var workVm in dockVm.Workspaces)
                    {
                        if (workVm is EditableDockableVM)
                        {
                            var editableVm = workVm as EditableDockableVM;
                            e.Cancel = !editableVm.EditableDataContext.OnClosing(editableVm.DisplayName);
                        }

                        if (e.Cancel)
                        {
                            break;
                        }
                    }

                    if (e.Cancel)
                    {
                        break;
                    }
                }
            }
        }
    }
}
