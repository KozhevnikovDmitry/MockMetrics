using System;
using System.Windows.Controls;

using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.AvalonInteraction.InteractionEvents
{
    /// <summary>
    /// Базовый класс аргументов для событий взаимодействия AvalonDock.
    /// </summary>
    public abstract class AvalonInteractEventArgs : EventArgs
    {
        /// <summary>
        /// Флаг указывающий на то, что вкладка должна быть открыта на AvalonDockHost из другой цепочки(композиции).
        /// </summary>
        public bool IsInterHost { get; set; }
    }

    /// <summary>
    /// Базовый класс аргументов для событий открытия вкладки на AvalonDock.
    /// </summary>
    public abstract class OpenDockableEventArgs : AvalonInteractEventArgs
    {
        /// <summary>
        /// Отображаемое имя вкладки.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Аргументы события, оповещающего о необходимости открытия вкладки.
        /// </summary>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        /// <param name="isInterHost"> </param>
        protected OpenDockableEventArgs(string displayName, bool isInterHost)
        {
            this.DisplayName = displayName;
            this.IsInterHost = isInterHost;
        }
    }

    /// <summary>
    /// Аргументы события, оповещающего о необходимости открытия вкладки.
    /// </summary>
    public class OpenSimpleDockableEventArgs : OpenDockableEventArgs
    {
        /// <summary>
        /// View, которое должно быть открыто во вкладке AvalonDock.
        /// </summary>
        public UserControl View { get; private set; }

        /// <summary>
        /// Аргументы события, оповещающего о необходимости открытия вкладки.
        /// </summary>
        /// <param name="view">View, которое должно быть открыто во вкладке AvalonDock</param>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        public OpenSimpleDockableEventArgs(UserControl view, string displayName, bool isInterHost)
            : base(displayName, isInterHost)
        {
            this.View = view;
        }
    }

    /// <summary>
    /// Аргументы события, оповещающего о необходимости открытия вкладки поиска.
    /// </summary>
    public class OpenSearchDockableEventArgs : OpenDockableEventArgs
    {
        /// <summary>
        /// ViewModel для вкладки поиска.
        /// </summary>
        public ISearchVM SearchVm { get; set; }

        /// <summary>
        /// Доменный тип сущности.
        /// </summary>
        public Type DomainType { get; set; }

        /// <summary>
        /// Аргументы события, оповещающего о необходимости открытия вкладки поиска.
        /// </summary>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        /// <param name="domainType">Доменный тип сущности</param>
        /// <param name="isInterHost"> </param>
        public OpenSearchDockableEventArgs(string displayName, Type domainType, bool isInterHost)
            : base(displayName, isInterHost)
        {
            this.DomainType = domainType;
        }

        /// <summary>
        /// Аргументы события, оповещающего о необходимости открытия вкладки поиска.
        /// </summary>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        /// <param name="searchVm">ViewModel для вкладки поиска</param>
        /// <param name="isInterHost"></param>
        public OpenSearchDockableEventArgs(string displayName, ISearchVM searchVm, bool isInterHost)
            : base(displayName, isInterHost)
        {
            this.SearchVm = searchVm;
        }
    }

    /// <summary>
    /// Аргументы события, оповещающего о необходимости открытия вкладки редактирования.
    /// </summary>
    public class OpenEditableDockableEventArgs : OpenDockableEventArgs
    {
        /// <summary>
        /// Сущность для редактирования
        /// </summary>
        public IDomainObject Entity { get; set; }

        /// <summary>
        /// Доменный тип сущности
        /// </summary>
        public Type DomainType { get; set; }

        /// <summary>
        /// Аргументы события, оповещающего о необходимости открытия вкладки редактирования.
        /// </summary>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        /// <param name="entity">Сущность для редактирования</param>
        /// <param name="domainType">Доменный тип сущности</param>
        /// <param name="isInterHost"> </param>
        public OpenEditableDockableEventArgs(string displayName, IDomainObject entity, Type domainType, bool isInterHost)
            : base(displayName, isInterHost)
        {
            this.Entity = entity;
            this.DomainType = domainType;
        }
    }

    /// <summary>
    /// Аргументы события, оповещающего о необходимости открытия вкладки с отчётом.
    /// </summary>
    public class OpenReportDockableEventArgs : OpenDockableEventArgs
    {
        /// <summary>
        /// Отчёт
        /// </summary>
        public IReport Report { get; set; }

        /// <summary>
        /// Флаг режима дизайнера для отчёта
        /// </summary>
        public bool IsDesigner { get; set; }

        /// <summary>
        /// Аргументы события, оповещающего о необходимости открытия вкладки с отчётом.
        /// </summary>
        /// <param name="displayName">Отображаемое имя вкладки</param>
        /// <param name="report">Объект отчёт</param>
        /// <param name="isDesigner">Флаг режима дизайнера для отчёта</param>
        /// <param name="isInterHost"> </param>
        public OpenReportDockableEventArgs(string displayName, IReport report, bool isDesigner, bool isInterHost)
            : base(displayName, isInterHost)
        {
            Report = report;
            this.IsDesigner = isDesigner;
        }
    }

    /// <summary>
    /// Аргументы события, оповещающего смены состояния(закрытие, ребилд) вкладки редактирования.
    /// </summary>
    public class ManageEditableDockableEventArgs : AvalonInteractEventArgs
    {
        public int EntityId { get; set; }

        public Type DomainType { get; set; }

        public ManageEditableDockableEventArgs(int entityId, Type domainType, bool isInterHost)
        {
            EntityId = entityId;
            DomainType = domainType;
            IsInterHost = isInterHost;
        }
    }
}