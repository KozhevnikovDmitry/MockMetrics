using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using AvalonDock;

using Common.DA.Interface;
using Common.UI.View;
using Common.UI.ViewModel.Interfaces;
using Common.UI.Views;

namespace Common.UI.ViewModel.WorkspaceViewModel
{
    /// <summary>
    /// Класс фабрика экземпляров View и ViewModel рабочих областей приложения.
    /// </summary>
    internal class WorkspaceFactory
    {
        /// <summary>
        /// Возвращает окно диалога.
        /// </summary>
        /// <param name="view">View, отображаемое в окне</param>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        /// <param name="resizeMode">Режим изменения размеров окна</param>
        /// <param name="sizeToContent">Режим привязки размеров к контенту</param>
        /// <param name="showInTaskbar">Режим отображения в панели задач</param>
        /// <param name="size">Размер окна</param>
        /// <param name="minSize">Минимальные размер окна</param>
        /// <returns>Окно диалога</returns>
        public DialogView CreateDialogView(UserControl view,
                                           INotifyPropertyChanged viewModel,
                                           string displayName,
                                           ResizeMode resizeMode = ResizeMode.NoResize,
                                           SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
                                           bool showInTaskbar = false, 
                                           Size? size = null,
                                           Size? minSize = null) 
        {
            view.DataContext = viewModel;
            IDialogVM dialogVm = new DialogVM() { View = view, DisplayName = displayName, DialogResizeMode = resizeMode, DialogSizeToContentMode = sizeToContent, ShowInTaskBar = showInTaskbar };
            DialogView dialogView = new DialogView() { DataContext = dialogVm };
            SetOwnerFor(dialogView);
            SetSizeFor(dialogView, size, minSize);
            return dialogView;
        }

        /// <summary>
        /// Возвращает окно просто рабочей области.
        /// </summary>
        /// <param name="view">View, отображаемое в окне</param>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        /// <param name="size">Размер окна</param>
        /// <param name="minSize">Минимальные размер окна</param>
        /// <returns>Окно рабочей области</returns>
        public ToolView CreateToolView(UserControl view,
                                       INotifyPropertyChanged viewModel,
                                       string displayName,
                                       Size? size = null,
                                       Size? minSize = null)
        {
            view.DataContext = viewModel;
            ToolVM toolVm = new ToolVM() { View = view, DisplayName = displayName };
            ToolView toolView = new ToolView() { DataContext = toolVm };
            SetOwnerFor(toolView);
            SetSizeFor(toolView, size, minSize);
            return toolView;
        }

        /// <summary>
        /// Возвращает окно диалога с функцией валидации.
        /// </summary>
        /// <param name="view">View, отображаемое в окне</param>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        /// <param name="resizeMode">Режим изменения размеров окна</param>
        /// <param name="sizeToContent">Режим привязки размеров к контенту</param>
        /// <param name="size">Размер окна</param>
        /// <param name="minSize">Минимальные размер окна</param>
        /// <returns>Окно диалога</returns>
        public DialogView CreateValidateableDialogView(UserControl view,
                                                       IValidateableVM viewModel,
                                                       string displayName,
                                                       ResizeMode resizeMode = ResizeMode.NoResize,
                                                       SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
                                                       Size? size = null,
                                                       Size? minSize = null)
        {
            IDialogVM dialogVm = new ValidateableDialogVM(view, viewModel) { DisplayName = displayName, DialogResizeMode = resizeMode, DialogSizeToContentMode = sizeToContent };
            DialogView dialogView = new DialogView() { DataContext = dialogVm };
            SetOwnerFor(dialogView);
            SetSizeFor(dialogView, size, minSize);
            return dialogView;
        }

        /// <summary>
        /// Возвращает окно диалога с функцией подтверждения.
        /// </summary>
        /// <param name="view">View, отображаемое в окне</param>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        /// <param name="resizeMode">Режим изменения размеров окна</param>
        /// <param name="sizeToContent">Режим привязки размеров к контенту</param>
        /// <param name="showInTaskbar">Режим отображения в панели задач</param>
        /// <param name="size">Размер окна</param>
        /// <param name="minSize">Минимальные размер окна</param>
        /// <returns>Окно диалога</returns>
        public DialogView CreateConfirmableDialogView(UserControl view, 
                                                      IConfirmableVM viewModel, 
                                                      string displayName, 
                                                      ResizeMode resizeMode = ResizeMode.NoResize, 
                                                      SizeToContent sizeToContent = SizeToContent.WidthAndHeight, 
                                                      bool showInTaskbar = false, 
                                                      Size? size = null, 
                                                      Size? minSize = null)
        {
            IDialogVM dialogVm = new ConfirmableDialogVM(view, viewModel) { DisplayName = displayName, DialogResizeMode = resizeMode, DialogSizeToContentMode = sizeToContent, ShowInTaskBar = showInTaskbar };
            DialogView dialogView = new DialogView() { DataContext = dialogVm };
            SetOwnerFor(dialogView);
            SetSizeFor(dialogView, size, minSize);
            return dialogView;
        }
        
        /// <summary>
        /// Возвращает окно диалога поиска доменных объектов.
        /// </summary>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        /// <param name="resizeMode">Режим изменения размеров окна</param>
        /// <param name="sizeToContent">Режим привязки размеров к контенту</param>
        /// <param name="size">Размер окна</param>
        /// <param name="minSize">Минимальные размер окна</param>
        /// <returns>Окно диалога</returns>
        public DialogView CreateSearchDialogView(ISearchVM viewModel,
                                                 string displayName,
                                                 ResizeMode resizeMode = ResizeMode.NoResize,
                                                 SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
                                                 Size? size = null,
                                                 Size? minSize = null)
        {
            SearchDialogVM dialogVm = new SearchDialogVM(viewModel) { DisplayName = displayName, DialogResizeMode = resizeMode, DialogSizeToContentMode = sizeToContent };
            dialogVm.SearchVm.ChooseResultRequested += (r, t) => dialogVm.OkCommand.Execute();
            DialogView dialogView = new DialogView() { DataContext = dialogVm };
            SetOwnerFor(dialogView);
            SetSizeFor(dialogView, size, minSize);
            return dialogView;
        }
       
        /// <summary>
        /// Возвращает вкладку с рабочей областью.
        /// </summary>
        /// <param name="view">View, отображаемое во вкладке</param>
        /// <param name="displayName">Отображаемое имя вкладкет</param>
        /// <returns>Вкладка с рабочей областью</returns>
        public IDockableVM CreateDockableVM(UserControl view,
                                            string displayName)
        {
            IDockableVM dockVm = new DockableVM { View = view, DisplayName = displayName };
            return dockVm;
        }

        /// <summary>
        /// Возвращает вкладку с рабочей областью редактирования персистентного объекта.
        /// </summary>
        /// <param name="view">View, отображаемое во вкладке</param>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        /// <param name="displayName">Отображаемое имя вкладкет</param>
        /// <returns>Вкладка с рабочей областью</returns>
        public EditableDockableVM CreateEditableDockableVM(UserControl view, 
                                                           IEditableVM viewModel,
                                                           string displayName)
        {
            view.DataContext = viewModel;
            return new EditableDockableVM(viewModel) { View = view, DisplayName = displayName };
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel для редактирования данных шаблонного объекта поиска.
        /// </summary>
        /// <param name="view">View редактирования шаблонного объекта</param>
        /// <param name="viewModel">ViewModel редактирования шаблонного объекта</param>
        /// <param name="searchObject">Шаблонный объект</param>
        /// <param name="displayName">Отображаемое имя области редактирования</param>
        /// <param name="templateWidth">Ширина области редактирования</param>
        /// <returns>ViewModel для редактирования данных шаблонного объекта поиска</returns>
        public ISearchTemplateVM CreateSearchTemplateVM(UserControl view, 
                                                        INotifyPropertyChanged viewModel,
                                                        IDomainObject searchObject,
                                                        string displayName,
                                                        int templateWidth)
        {
            view.DataContext = viewModel;
            view.Width = templateWidth;
            ISearchTemplateVM searchTemplateVm = new SearchTemplateVM(view, searchObject) { DisplayName = displayName };
            return searchTemplateVm;
        }

        /// <summary>
        /// Назначает окно хозяина для окна диалога.
        /// </summary>
        /// <param name="window">Окно без хозяина</param>
        private void SetOwnerFor(Window window)
        {
            if (Application.Current != null && 
                window != Application.Current.MainWindow)
            {
                window.Owner =  Application.Current.MainWindow;
            }
        }

        /// <summary>
        /// Назначает размеры окну.
        /// </summary>
        /// <param name="window">Окно, которому назначаются размеры</param>
        /// <param name="size">Размеры окна</param>
        /// <param name="minSize">Минимальные размеры окна</param>
        private void SetSizeFor(Window window, Size? size, Size? minSize)
        {
            if (size.HasValue)
            {
                window.Height = size.Value.Height;
                window.Width = size.Value.Width;
            }
            if (minSize.HasValue)
            {
                window.MinHeight = minSize.Value.Height;
                window.MinWidth = minSize.Value.Width;
            }
        }
    }
}
