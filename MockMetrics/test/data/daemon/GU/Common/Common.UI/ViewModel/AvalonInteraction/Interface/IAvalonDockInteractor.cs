using System;
using System.Windows.Controls;

using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.UI.ViewModel.AvalonInteraction.InteractionEvents;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.AvalonInteraction.Interface
{
    /// <summary>
    /// Интерфейс класса, содержащего события и методы для взаимодействия с AvalonDockVM
    /// </summary>
    /// <remarks>
    /// ViewModel'ы участвующие во взаимодействии с  AvalonDockVM (наследующие IAvalonDockCaller)
    /// делегируют управление событиями взаимодейтсвия наследникам данного интерфейса.
    /// События пробрасываются по цепочке ViewModel'ов через объекты IAvalonDockInteractor.
    /// Чтобы зарегать дочерний IAvalonDockCaller есть метод RegisterCaller.
    /// После регистрации дочернего caller'а, все его события будут обрабатываться в данном.
    /// Под обработкой может пониматься либо проброс события дальше по цепочке, либо обработка запроса на открытие вкладки.
    /// </remarks>
    public interface IAvalonDockInteractor
    {
        #region Events

        /// <summary>
        /// Событие на открытие простой вкладки
        /// </summary>
        event OpenDockable OpenDockable;

        /// <summary>
        /// Событие на открытие вкладки поиска
        /// </summary>
        event OpenSearchDockable OpenSearchDockable;

        /// <summary>
        /// Событие на открытие эдитабельной вкладки
        /// </summary>
        event OpenEditableDockable OpenEditableDockable;

        /// <summary>
        /// Событие на открытие вкладки репорта
        /// </summary>
        event OpenReportDockable OpenReportDockable;

        /// <summary>
        /// Событие на закрытие вкладки репорта
        /// </summary>
        event ManageEditableDockable CloseEditableDockable;

        /// <summary>
        /// Событие на ребилд вкладки репорта
        /// </summary>
        event ManageEditableDockable RebuildEditableDockable; 

        #endregion

        #region Event Raisers

        void RaiseOpenDockable(object sender, OpenSimpleDockableEventArgs e);

        void RaiseOpenSearchDockable(object sender, OpenSearchDockableEventArgs e);

        void RaiseOpenEditableDockable(object sender, OpenEditableDockableEventArgs e);

        void RaiseCloseEditableDockable(object sender, ManageEditableDockableEventArgs e);

        void RaiseEditableDisplayNameChanged(object sender, ManageEditableDockableEventArgs e);

        void RaiseOpenReportDockable(object sender, OpenReportDockableEventArgs e);

        void RaiseOpenDockable(string displayName, UserControl view, bool IsInterHost = false);

        void RaiseOpenSearchDockable(string displayName, Type domainType, bool IsInterHost = false);

        void RaiseOpenSearchDockable(string displayName, ISearchVM searchVM, bool IsInterHost = false);

        void RaiseOpenEditableDockable(string displayName, Type domainType, IDomainObject entity, bool IsInterHost = false);

        void RaiseOpenReportDockable(string displayName, IReport report, bool isDisigner, bool IsInterHost = false);

        void RaiseCloseEditableDockable(Type domainType, int entityId, bool IsInterHost = false);

        void RaiseEditableDisplayNameChanged(Type domainType, int entityId, bool IsInterHost = false);

        #endregion

        #region Register Child Caller

        /// <summary>
        /// Регистрирует дочерний IAvalonDockCaller для обработки возбуждаемых им событий..
        /// </summary>
        /// <param name="caller">Дочерний IAvalonDockCaller</param>
        void RegisterCaller(IAvalonDockCaller caller);

        #endregion
    }
}
