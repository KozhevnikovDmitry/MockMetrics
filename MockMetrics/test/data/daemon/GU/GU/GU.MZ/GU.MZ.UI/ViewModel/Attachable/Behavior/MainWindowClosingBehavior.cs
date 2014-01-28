﻿using System.ComponentModel;
using System.Windows.Interactivity;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;
using GU.MZ.UI.View;

namespace GU.MZ.UI.ViewModel.Attachable.Behavior
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
            AssociatedObject.Closing += AssociatedObjectClosing;
        }

        /// <summary>
        /// Обрабатывает событие отключения поведения.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Closing -= AssociatedObjectClosing;
        }

        /// <summary>
        /// Обрабатывает событие <c>MainWindow.Closing</c>.
        /// </summary>
        /// <param name="sender">Отправитель события</param>
        /// <param name="e">Аргументы собятия</param>
        private void AssociatedObjectClosing(object sender, CancelEventArgs e)
        {
            var mainVm = AssociatedObject.DataContext as MainVm;
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