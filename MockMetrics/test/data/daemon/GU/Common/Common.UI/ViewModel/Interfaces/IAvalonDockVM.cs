using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.UI.View;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.WorkspaceViewModel;

using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс классов Моделей-Представления для Представлений на основе AvalonDock.DockingManager.
    /// </summary>
    public interface IAvalonDockVM : INotifyPropertyChanged, IAvalonDockCaller
    {
        /// <summary>
        /// Возвращает или устанавливает активную рабочую область прилоежения.
        /// </summary>
        IDockableVM ActiveWorkspace { get; set; }

        /// <summary>
        /// Возвращает или устанавливает значения коллекции одновременно открытых рабочих областей приложения.
        /// </summary>
        ObservableCollection<IDockableVM> Workspaces { get; set; }

        /// <summary>
        /// Объект-команда сохранения всех редактируемых документов находящихся в DockingManager.
        /// </summary>
        CompositeCommand SaveAllCommand { get; }

        /// <summary>
        /// Объект-команда сохранения редактируемого документа находящегося в актвиной вкладке в DockingManager.
        /// </summary>
        DelegateCommand SaveCommand { get; }

        /// <summary>
        /// Команда оповещения об изменении активной вкладки.
        /// </summary>
        DelegateCommand NotifyActiveDocumentChangedCommand { get; }

        #region Workspaces's Management

        /// <summary>
        /// Добавляет новую вкладку в DockingManager.
        /// </summary>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        /// <param name="view">Представление отображаемое во вкладке</param>
        /// <param name="viewModel">Модель-представления для <c>view</c></param>
        void AddDockable(string displayName, UserControl view, INotifyPropertyChanged viewModel);

        /// <summary>
        /// Добавляет новую вкладку в DockingManager.
        /// </summary>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        /// <param name="view">Представление отображаемое во вкладке</param>
        void AddDockable(string displayName, UserControl view);

        /// <summary>
        /// Добавляет новую вкладку поиска.
        /// </summary>
        void AddSearchDockable(string displayName, Type domainType);

        /// <summary>
        /// Добавляет новую вкладку поиска.
        /// </summary>
        void AddSearchDockable(string displayName, ISearchVM searchVm);

        /// <summary>
        /// Добавляет новую вкладку с отчётом.
        /// </summary>
        void AddReportDockable(string displayName, IReport report);

        /// <summary>
        /// Добавляет новую вкладку редактирования.
        /// </summary>
        void AddEditableDockable(string displayName, IDomainObject entity, Type domainType);

        /// <summary>
        /// Закрывает вкладку редактирования
        /// </summary>
        /// <param name="entityKey">Ключ сущности</param>
        /// <param name="domainType">Тип сущности</param>
        void CloseEditableDockable(string entityKey, Type domainType);

        /// <summary>
        /// Закрывает вкладку редактирования
        /// </summary>
        /// <param name="entityKey">Ключ сущности</param>
        /// <param name="domainType">Тип сущности</param>
        void DisplayNameChanged(string entityKey, Type domainType);

        /// <summary>
        /// Возвращает true, если вкладка редактирования доменного объекта уже открыта
        /// </summary>
        /// <param name="entity">Доменный объект</param>
        /// <returns>True, если вкладка редактирования доменного объекта уже открыта</returns>
        bool IsAlreadyOpenned(IDomainObject entity, bool activate);
        
        /// <summary>
        /// Возвращает true, если вкладка редактирования доменного объекта уже открыта
        /// </summary>
        bool IsAlreadyOpenned(string entityKey, Type domainType, bool activate);

        #endregion
    }
}
