using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using Common.BL;
using Common.BL.ReportMapping;
using Common.BL.Search.SearchSpecification;
using Common.DA;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI.Report;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using Common.UI.ViewModel.WorkspaceViewModel;
using Common.UI.Views;

using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI
{
    /// <summary>
    /// Фасад UI-слоя
    /// </summary>
    public static class UIFacade
    {
        #region Get ViewModel Instances

        /// <summary>
        /// Возвращает экземпляр ViewModel для отображения данных доменного объекта в элементе списка.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="item">Отображаемый доменный объект</param>
        /// <returns>ViewModel для отображения данных доменного обеъкта в элементе списка</returns>
        public static IListItemVM<T> GetListItemVM<T>(T item) where T : IDomainObject
        {
            return UIContainer.Instance.ResolveListItemVMType(item);
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel для отображения данных доменного объекта в элементе списка на странице поиска.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="item">Отображаемый доменный объект</param>
        /// <returns>ViewModel для отображения данных доменного обеъкта в элементе списка на странице поиска</returns>
        public static ISearchResultVM<T> GetSearchResultVM<T>(T item) where T : IDomainObject
        {
            return UIContainer.Instance.ResolveSearchResultVMType(item);
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel для окна поиска доменных объектов
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <returns>ViewModel для окна поиска</returns>
        public static ISearchVM<T> GetSearchVM<T>() where T : IPersistentObject
        {
            return UIContainer.Instance.ResolveSearchVMType<T>();
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel для окна редактирования доменного объекта.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="entity">Редактируемый объект</param>
        /// <param name="isEditable">Флаг возможноти редактирования данных объекта</param>
        /// <returns>ViewModel для окна редактирования доменного объекта</returns>
        public static IEditableVM<T> GetEditableVM<T>(T entity, bool isEditable = true) where T : DomainObject<T>, IPersistentObject
        {
            return UIContainer.Instance.ResolveEditableVM(entity, isEditable);
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel для редактирования данных шаблонного объекта поиска.
        /// </summary>
        /// <param name="searchTemplateView">View редактирования шаблонного объекта</param>
        /// <param name="searchTemplateVm">ViewModel редактирования шаблонного объекта</param>
        /// <param name="searchObject">Шаблонный объект</param>
        /// <param name="displayName">Отображаемое имя области редактирования</param>
        /// <param name="templateWidth">Ширина области редактирования</param>
        /// <returns>ViewModel для редактирования данных шаблонного объекта поиска</returns>
        public static ISearchTemplateVM CreateSearchTemplateVM(UserControl searchTemplateView,
                                                               INotifyPropertyChanged searchTemplateVm,
                                                               IDomainObject searchObject,
                                                               string displayName,
                                                               int templateWidth = 400)
        {
            var factory = new WorkspaceFactory();
            return factory.CreateSearchTemplateVM(searchTemplateView, searchTemplateVm, searchObject, displayName, templateWidth);
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel для редактирования настроек настраиваемого поиска.
        /// </summary>
        /// <param name="searchPreset">Настройки поиска</param>
        /// <param name="searchTypes">Список доменных типов, участвующих в поиске</param>
        /// <param name="templateWidth">Ширина области редактирования</param>
        /// <returns>ViewModel для редактирования настроек настраиваемого поиска</returns>
        public static ISearchPresetVM CreateSearchPresetVM(SearchPreset searchPreset, 
                                                           List<Type> searchTypes,
                                                           int templateWidth = 400 )
        {
            return new SearchPresetVM(searchPreset, searchTypes);
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel с функционалом отображения валидации доменного объекта типа T.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="entity">Доменный объект</param>
        /// <returns>экземпляр ViewModel с функционалом отображения валидации доменного объекта</returns>
        public static IDomainValidateableVM<T> GetDomainValidateableVM<T>(T entity) where T : IDomainObject
        {
            return UIContainer.Instance.ResolveDomainValidateableVM(entity);
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel с функционалом отображения валидации доменного объекта типа T.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="entity">Доменный объект</param>
        /// <param name="tag">Ключ для ресолва</param>
        /// <returns>экземпляр ViewModel с функционалом отображения валидации доменного объекта</returns>
        public static IDomainValidateableVM<T> GetDomainValidateableVM<T>(T entity, string tag) where T : IDomainObject
        {
            return UIContainer.Instance.ResolveDomainValidateableVM(entity, tag);
        }

        #endregion


        #region Get View Instances

        /// <summary>
        /// Возвращает View редактирования для доменных объектов типа domainType.
        /// </summary>
        /// <param name="domainType">Доменный тип</param>
        /// <returns>View редактирования для доменных объектов</returns>
        public static UserControl GetEditableView(Type domainType)
        {
            return UIContainer.Instance.ResolveEditableView(domainType);
        }

        #endregion
        

        #region Register Types

        /// <summary>
        /// Регистрирует класс стратегии бизнес-логики, которые испольузются классами ViewModel'ами
        /// </summary>
        /// <param name="container">Контейнер bl-классов</param>
        public static void InitializeUI(IDomainLogicContainer container)
        {
            UIContainer.Instance.InitializeUIContainer(container);
        }

        /// <summary>
        /// Регистрирует классы ViewModel'ы в контейнере.
        /// </summary>
        /// <param name="uiAssemblies">Список сборок с классами  ViewModel'ами</param>
        public static void RegisterVMTypes(List<Assembly> uiAssemblies)
        {
            UIContainer.Instance.RegisterVMTypes(uiAssemblies);
        }

        /// <summary>
        /// Регистрирует тип TView для редактирования доменных объектов типа TEntity.
        /// </summary>
        /// <typeparam name="TView">Тип View</typeparam>
        /// <typeparam name="TEntity">Доменный тип</typeparam>
        public static void RegisterEditableView<TView, TEntity>()
            where TView : UserControl
            where TEntity : IPersistentObject
        {
            UIContainer.Instance.RegisterEditableView<TView, TEntity>();
        }

        /// <summary>
        /// Регистрирует свойства доменных объектов, по которым может проводиться поиск.
        /// </summary>
        /// <param name="dmAssemblies">Список сборок с классами доменных моделей</param>
        public static void RegisterSearchProperties(IEnumerable<Assembly> dmAssemblies)
        {
            UIContainer.Instance.RegidterSearchProperties(dmAssemblies);
        }

        /// <summary>
        /// Регистрирует список пресетов поиска
        /// </summary>
        /// <param name="presetList">Список пресетов поиска</param>
        public static void RegisterSearchPresetList(List<SearchPresetSpec> presetList)
        {
            UIContainer.Instance.RegisterSearchPresetList(presetList);
        }

        /// <summary>
        /// Регистрирует тип ViewModel
        /// </summary>
        /// <typeparam name="TInterface">Тип интерфейса ViewModel</typeparam>
        /// <typeparam name="TViewModel">Конкретный тип ViewModel</typeparam>
        /// <param name="tag">Ключ для ресолва</param>
        public static void RegisterType<TInterface, TViewModel>(string tag)
            where TInterface : INotifyPropertyChanged
            where TViewModel : INotifyPropertyChanged
        {
            UIContainer.Instance.RegisterType<TInterface, TViewModel>(tag);
        }

        #endregion


        #region Show Dialogs
        
        /// <summary>
        /// Отображает модальное окно. 
        /// </summary>
        /// <param name="view">View, отображаемое в окне</param>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        public static void ShowToolView(UserControl view,
                                        NotificationObject viewModel,
                                        string displayName)
        {
            try
            {
                view.DataContext = viewModel;
                ToolVM toolVm = new ToolVM() { View = view, DisplayName = displayName };
                ToolView toolView = new ToolView { DataContext = toolVm, Owner = Application.Current.MainWindow };
                toolView.ShowDialog();
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VMException("Ошибка при создании диалога.", ex);
            }
        }

        /// <summary>
        /// Отображает диалоговое окно. Возвращает результата диалога: OK - true, Cancel - false.
        /// </summary>
        /// <param name="view">View, отображаемое в окне</param>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        /// <param name="resizeMode">Режим изменения размеров окна</param>
        /// <param name="sizeToContent">Режим привязки размеров к контенту</param>
        /// <param name="showInTaskbar">Режим отображения в панели задач</param>
        /// <returns>Результата диалога</returns>
        public static bool ShowDialogView(UserControl view,
                                          INotifyPropertyChanged viewModel,
                                          string displayName,
                                          ResizeMode resizeMode = ResizeMode.NoResize,
                                          SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
                                          bool showInTaskbar = false)
        {
            WorkspaceFactory factory = new WorkspaceFactory();
            var dialog = factory.CreateDialogView(view, viewModel, displayName, resizeMode, sizeToContent, showInTaskbar);
            dialog.ShowDialog();
            return (dialog.DataContext as IDialogVM).IsOkResult;
        }

        /// <summary>
        /// Отображает диалоговое окно с валидируемыми данными. Возвращает результата диалога: OK - true, Cancel - false.
        /// </summary>
        /// <param name="view">View, отображаемое в окне</param>
        /// <param name="viewModel">ViewModel отображаемого View с функцией валидации</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        /// <param name="resizeMode">Режим изменения размеров окна</param>
        /// <param name="sizeToContent">Режим привязки размеров к контенту</param>
        /// <returns>Результата диалога</returns>
        public static bool ShowValidateableDialogView(UserControl view,
                                                      IValidateableVM viewModel,
                                                      string displayName,
                                                      ResizeMode resizeMode = ResizeMode.NoResize,
                                                      SizeToContent sizeToContent = SizeToContent.WidthAndHeight)
        {
            WorkspaceFactory factory = new WorkspaceFactory();
            var dialog = factory.CreateValidateableDialogView(view, viewModel, displayName, resizeMode, sizeToContent);
            dialog.ShowDialog();
            return (dialog.DataContext as IDialogVM).IsOkResult;
        }

        /// <summary>
        /// Отображает диалоговое окно с проверкой возможности подтверждения. Возвращает результата диалога: OK - true, Cancel - false.
        /// </summary>
        /// <param name="view">View, отображаемое в окне</param>
        /// <param name="viewModel">ViewModel отображаемого View с функцией проверки подтвердждения</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        /// <param name="resizeMode">Режим изменения размеров окна</param>
        /// <param name="sizeToContent">Режим привязки размеров к контенту</param>
        /// <param name="showInTaskbar">Режим отображения в панели задач</param>
        /// <returns>Результата диалога</returns>
        public static bool ShowConfirmableDialogView(UserControl view,
                                                     IConfirmableVM viewModel,
                                                     string displayName,
                                                     ResizeMode resizeMode = ResizeMode.NoResize,
                                                     SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
                                                     bool showInTaskbar = false)
        {
            WorkspaceFactory factory = new WorkspaceFactory();
            var dialog = factory.CreateConfirmableDialogView(view, viewModel, displayName, resizeMode: resizeMode, sizeToContent: sizeToContent, showInTaskbar: showInTaskbar);
            dialog.ShowDialog();
            return (dialog.DataContext as IDialogVM).IsOkResult;
        }

        /// <summary>
        /// Отображает диалоговое окно поиска доменных объектов. Возвращает результата диалога: OK - true, Cancel - false.
        /// </summary>
        /// <param name="viewModel">ViewModel отображаемого поиска</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        /// <param name="resizeMode">Режим изменения размеров окна</param>
        /// <param name="sizeToContent">Режим привязки размеров к контенту</param>
        /// <returns>Результата диалога</returns>
        public static bool ShowSearchDialogView(ISearchVM viewModel,
                                                string displayName,
                                                ResizeMode resizeMode = ResizeMode.NoResize,
                                                SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
                                                Size? size = null,
                                                Size? minSize = null)
        {
            WorkspaceFactory factory = new WorkspaceFactory();
            var dialog = factory.CreateSearchDialogView(viewModel, displayName, resizeMode, sizeToContent, size, minSize);
            dialog.ShowDialog();
            return (dialog.DataContext as IDialogVM).IsOkResult;
        }

        #endregion


        #region Show Report

        /// <summary>
        /// Вовзвращает View отображения отчёта.
        /// </summary>
        /// <typeparam name="T">Тип данных для отчёта</typeparam>
        /// <param name="reportFilePath">Путь к файлу отчёта</param>
        /// <param name="dataName">Имя для сущности с данными</param>
        /// <param name="data">Данные для отчёта</param>
        /// <param name="isDesigner">Флаг указывающий на необходимость открытия отчёта в режиме дизайнера</param>
        /// <returns>View отображения отчёта</returns>
        public static UserControl GetReportPresenter<T>(string reportFilePath, string dataName, T data, bool isDesigner)
        {
            return new StiReportEngine().GetReportPresenter(reportFilePath, dataName, data, isDesigner);
        }

        /// <summary>
        /// Вовзвращает View отображения отчёта.
        /// </summary>
        /// <param name="report">Объект отчёта</param>
        /// <param name="isDesigner">Флаг указывающий на необходимость открытия отчёта в режиме дизайнера</param>
        /// <returns>View отображения отчёта</returns>
        public static UserControl GetReportPresenter(IReport report, bool isDesigner)
        {
            var data = report.RetrieveData();
            return new StiReportEngine().GetReportPresenter(report.ViewPath, report.DataAlias, data, isDesigner);
        }

        #endregion
    }
}
