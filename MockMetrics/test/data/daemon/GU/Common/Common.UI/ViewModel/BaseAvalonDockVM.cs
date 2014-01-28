using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

using AvalonDock;
using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.UI.Interface;
using Common.UI.View;
using Common.UI.View.SearchView;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel
{
    /// <summary>
    /// Базовый класс Модель-Представлений, для Представлений на основе AvalonDock.DocDockingManager.
    /// </summary>
    public abstract class BaseAvalonDockVM : NotificationObject, IAvalonDockVM
    {
        private readonly IDockableUiFactory _uiFactory;

        /// <summary>
        /// Базовый класс Модель-Представлений, для Представлений на основе AvalonDock.DocDockingManager.
        /// </summary>
        protected BaseAvalonDockVM(IDockableUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
            AvalonInteractor = new UltimateAvalonDockInteractor(this);
            Workspaces = new ObservableCollection<IDockableVM>();
            Workspaces.CollectionChanged += OnWorkspacesCollectionChanged;
            SaveAllCommand = new CompositeCommand(false);
            SaveCommand = new DelegateCommand(Save, IsActiveVMEditable);
            NotifyActiveDocumentChangedCommand = new DelegateCommand(NotifyActiveDocumentChanged);
        }

        #region Workspaces's Management

        /// <summary>
        /// Добавляет новую вкладку в DockingManager.
        /// </summary>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        /// <param name="view">Представление отображаемое во вкладке</param>
        /// <param name="viewModel">Модель-представления для <c>view</c></param>
        public void AddDockable(string displayName, UserControl view, INotifyPropertyChanged viewModel)
        {
            view.DataContext = viewModel;
            AddDockable(displayName, view);
        }

        /// <summary>
        /// Добавляет новую вкладку в DockingManager.
        /// </summary>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        /// <param name="view">Представление отображаемое во вкладке</param>
        public void AddDockable(string displayName, UserControl view)
        {
            var factory = new WorkspaceFactory();
            var workspace = factory.CreateDockableVM(view, displayName);
            workspace.OnClosing += OnWorkspaceClosing;
            workspace.OnProgramClosing += OnWorkspaceClosing;
            Workspaces.Add(workspace);
            ActiveWorkspace = workspace;
        }

        public void AddSearchDockable(string displayName, Type domainType)
        {
            var viewModel = _uiFactory.GetSearchVmType(domainType);
            AddSearchDockable(displayName, viewModel);
        }

        public void AddSearchDockable(string displayName, ISearchVM searchVm)
        {
            var view = new SearchView();
            view.DataContext = searchVm;
            AvalonInteractor.RegisterCaller(searchVm as IAvalonDockCaller);
            AddDockable(displayName, view);
        }


        public void AddReportDockable(string displayName, IReport report)
        {
            var view = _uiFactory.GetReportPresenter(report);
            AddDockable(displayName, view);
        }

        /// <summary>
        /// Добавляет новую вкладку содержащую редактируемый документ в DockingManager.
        /// </summary>
        public void AddEditableDockable(string displayName, IDomainObject entity, Type domainType)
        {
            var view = _uiFactory.GetEditableView(domainType);
            var viewModel = _uiFactory.GetEditableVm(domainType, entity);
            AvalonInteractor.RegisterCaller(viewModel as IAvalonDockCaller);
            SaveAllCommand.RegisterCommand(viewModel.SaveCommand);
            var factory = new WorkspaceFactory();
            var workspace = factory.CreateEditableDockableVM(view, viewModel, displayName);
            workspace.OnClosing += OnWorkspaceClosing;
            workspace.OnProgramClosing += OnWorkspaceClosing;
            Workspaces.Add(workspace);
            ActiveWorkspace = workspace;
        }

        public void CloseEditableDockable(string entityKey, Type domainType)
        {
            var workspace = GetEditableDockableVm(entityKey, domainType);
            Workspaces.Remove(workspace);
        }

        public void DisplayNameChanged(string entityKey, Type domainType)
        {
            var workspace = GetEditableDockableVm(entityKey, domainType);
            workspace.OnEditableDataContextIsDirtyChanged();
        }

        /// <summary>
        /// Удаляет вкладку из DockingManager
        /// </summary>
        /// <param name="viewModel">Модель представления удаляемой вкладки</param>
        protected void RemoveEditableDockable(IEditableVM viewModel)
        {
            SaveAllCommand.UnregisterCommand(viewModel.SaveCommand);
        }

        /// <summary>
        /// Подготавливает рабочую область к завершнию работы, возвращает флаг готовности к завершению работы.
        /// </summary>
        /// <param name="workspaceVM">Модель представления рабочей области</param>
        /// <returns>Флаг готовности к завершению работы</returns>
        protected virtual bool PrepareWorkspaceClosing(IDockableVM workspaceVM)
        {
            if (workspaceVM is EditableDockableVM)
            {
                var edvm = workspaceVM as EditableDockableVM;
                return edvm.EditableDataContext.OnClosing(edvm.DisplayName);
            }
            return true;
        }

        /// <summary>
        /// Возвращает истину, если представление для редактирования сущности уже открыто.  
        /// </summary>
        /// <typeparam name="T">Тип модели представления</typeparam>
        /// <param name="key">Первичный ключ сущности</param>
        /// <returns></returns>
        protected bool IsAlreadyOpenned<T>(string key) where T : IEditableVM
        {
            return IsAlreadyOpenned(key, typeof (T), true);
        }

        /// <summary>
        /// Возвращает истину, если представление для редактирования сущности уже открыто.  
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        public bool IsAlreadyOpenned(IDomainObject entity, bool activate)
        {
            return IsAlreadyOpenned(entity.GetKeyValue(), entity.GetType(), activate);
        }

        /// <summary>
        /// Возвращает true, если вкладка редактирования доменного объекта уже открыта
        /// </summary>
        public bool IsAlreadyOpenned(string entityKey, Type domainType, bool activate)
        {
            var workspace = GetEditableDockableVm(entityKey, domainType);
            if (workspace != null)
            {
                if (activate)
                {
                    ActiveWorkspace = workspace;
                }
                return true;
            }

            return false;
        }

        private EditableDockableVM GetEditableDockableVm(string entityKey, Type domainType)
        {
            if (entityKey.Trim() == "0")
            {
                return null;
            }

            return Workspaces.OfType<EditableDockableVM>()
                                      .Where(w => w.EditableDataContext.GetEntityKeyValue().Trim() == entityKey.Trim())
                                      .SingleOrDefault(w => w.EditableDataContext.GetEntityType().IsAssignableFrom(domainType));
        }

        /// <summary>
        /// Возвращает или устанавливает ViewModel рабочей области на активной вкладке.
        /// </summary>
        protected object ActiveWorkspaceVM
        {
            get
            {
                return ActiveWorkspace != null ? ActiveWorkspace.View.DataContext : null;
            }
            set
            {
                if (ActiveWorkspace != null)
                {
                    ActiveWorkspace.View.DataContext = value;
                }
            }
        }

        #endregion

        #region Binding Properties

        /// <summary>
        /// Коллекция объектов Представлений - одновременно открытых рабочих областей приложения.
        /// </summary>
        protected ObservableCollection<IDockableVM> _workspaces;

        /// <summary>
        /// Возвращает или устанавливает значения коллекции одновременно открытых рабочих областей приложения.
        /// </summary>
        public ObservableCollection<IDockableVM> Workspaces
        {
            get
            {
                return _workspaces;
            }
            set
            {
                if (_workspaces == null || !_workspaces.Equals(value))
                {
                    _workspaces = value;
                    RaisePropertyChanged(() => Workspaces);
                }
            }
        }

        private IDockableVM _activeWorkspace;

        /// <summary>
        /// Возвращает или устанавливает активную рабочую область приложения.
        /// </summary>
        public IDockableVM ActiveWorkspace
        {
            get
            {
                return _activeWorkspace;
            }
            set
            {
                if (!Equals(value, _activeWorkspace))
                {
                    _activeWorkspace = value;
                    RaisePropertyChanged(() => ActiveWorkspace);
                    NotifyActiveDocumentChanged();
                }
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда сохранения изменений на всех открытых рабочих областях.
        /// </summary>
        public CompositeCommand SaveAllCommand { get; protected set; }

        /// <summary>
        /// Команда сохранения изменений на активной рабочей области.
        /// </summary>
        public DelegateCommand SaveCommand { get; protected set; }

        /// <summary>
        /// Команда оповещения об изменении активной вкладки.
        /// </summary>
        public DelegateCommand NotifyActiveDocumentChangedCommand { get; protected set; }

        /// <summary>
        /// Сохраняет изменения на активной рабочей области.
        /// </summary>
        protected void Save()
        {
            EditableDockableVM dockVm = ActiveWorkspace as EditableDockableVM;
            dockVm.EditableDataContext.SaveCommand.Execute();
        }

        /// <summary>
        /// Возвращает флаг, указывающий на то, является ли рабочая область на активной вкладке областью редактирования доменного объекта.
        /// </summary>
        /// <returns>Флаг, указывающий на то, является ли рабочая область на активной вкладке областью редактирования доменного объекта</returns>
        protected bool IsActiveVMEditable()
        {
            if (ActiveWorkspace != null)
            {
                if (ActiveWorkspace is EditableDockableVM)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Возвращает флаг, указывающий на то, является ли рабочая область на активной вкладке областью редактирования доменного объекта типа Т.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <returns>Флаг, указывающий на то, является ли рабочая область на активной вкладке областью редактирования доменного объекта типа Т</returns>
        protected bool IsActiveVMEditableOfDomainType<T>() where T : class
        {
            if (IsActiveVMEditable())
            {
                if ((ActiveWorkspace as EditableDockableVM).EditableDataContext.GetEntityType() == typeof(T))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Оповещает об изменении активной вкладки.
        /// </summary>
        protected virtual void NotifyActiveDocumentChanged()
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region ObservableCollection EventHandling

        /// <summary>
        /// Обрабатывает события изменения состава коллекции рабочих областей приложения.
        /// </summary>
        /// <param name="sender">Хозяин события</param>
        /// <param name="e">Аргумента события</param>
        private void OnWorkspacesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.Cast<IDockableVM>())
                {
                    item.OnClosing -= OnWorkspaceClosing;
                    item.OnProgramClosing -= OnWorkspaceClosing;

                    if (item is EditableDockableVM)
                    {
                        if (item.Equals(ActiveWorkspace))
                        {
                            ActiveWorkspace = null;
                        }

                        RemoveEditableDockable((item as EditableDockableVM).EditableDataContext);
                    }
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие инициирования закрытия рабочей области. 
        /// </summary>
        /// <param name="sender">Хозяин события</param>
        /// <param name="e">Аргумента события</param>
        private void OnWorkspaceClosing(object sender, DocumentClosingEventArgs e)
        {
            if (e.Document.Content is IDockableVM)
            {
                e.Cancel = !PrepareWorkspaceClosing(e.Document.Content as IDockableVM);
            }
        }

        private void OnWorkspaceClosing(IDockableVM dockableVm)
        {
            if (PrepareWorkspaceClosing(dockableVm))
            {
                Workspaces.Remove(dockableVm);
            }
        }

        #endregion

        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; protected set; }

        #endregion
    }
}
