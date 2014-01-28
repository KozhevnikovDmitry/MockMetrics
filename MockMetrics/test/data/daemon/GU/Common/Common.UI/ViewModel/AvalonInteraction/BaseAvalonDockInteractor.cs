using System;
using System.Windows;
using System.Windows.Controls;

using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.UI.ViewModel.AvalonInteraction.AvalonInteractionException;
using Common.UI.ViewModel.AvalonInteraction.InteractionEvents;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent;

namespace Common.UI.ViewModel.AvalonInteraction
{
    /// <summary>
    /// Базовый класс, содержащий события и методы для взаимодействия с AvalonDockVM
    /// </summary>
    public abstract class BaseAvalonDockInteractor : IAvalonDockInteractor
    {
        /// <summary>
        /// Объект, который подставляется в качестве паблишера событий взаимодействия
        /// </summary>
        private readonly object _sender;

        /// <summary>
        /// Базовый класс, содержащий события и методы для взаимодействия с AvalonDockVM
        /// </summary>
        /// <param name="sender">Паблишер событий взаимодействия</param>
        protected BaseAvalonDockInteractor(object sender)
        {
            _sender = sender;
            _openDockableListener = new WeakEventListener<OpenSimpleDockableEventArgs>(RaiseOpenDockable);
            _openSearchDockableListener = new WeakEventListener<OpenSearchDockableEventArgs>(RaiseOpenSearchDockable);
            _openEditableDockableListener = new WeakEventListener<OpenEditableDockableEventArgs>(RaiseOpenEditableDockable);
            _openReportDockableListener = new WeakEventListener<OpenReportDockableEventArgs>(RaiseOpenReportDockable);
            _closeEditableDockableListener =
                new WeakEventListener<ManageEditableDockableEventArgs>(RaiseCloseEditableDockable);
            _rebuildEditableDockableListener =
                new WeakEventListener<ManageEditableDockableEventArgs>(RaiseEditableDisplayNameChanged);
        }

        /// <summary>
        /// Регистрирует дочерний элемент цепочки взаимодействия с AvalonDockVM
        /// </summary>
        /// <param name="caller"></param>
        public void RegisterCaller(IAvalonDockCaller caller)
        {
            if (caller.AvalonInteractor == null)
            {
                throw new NullCallerInteractorException();
            }

            AvalonInteractWeakEventManager.AddListener(caller, _openDockableListener);
            AvalonSearchInteractWeakEventManager.AddListener(caller, _openSearchDockableListener);
            AvalonOpenEditableInteractWeakEventManager.AddListener(caller, _openEditableDockableListener);
            AvalonReportInteractWeakEventManager.AddListener(caller, _openReportDockableListener);
            AvalonRebuildEditableInteractWeakEventManager.AddListener(caller, _rebuildEditableDockableListener);
            AvalonCloseEditableInteractWeakEventManager.AddListener(caller, _closeEditableDockableListener);
        }

        #region Events

        /// <summary>
        /// Событие на открытие простой вкладки
        /// </summary>
        public event OpenDockable OpenDockable;

        /// <summary>
        /// Событие на открытие вкладки поиска
        /// </summary>
        public event OpenSearchDockable OpenSearchDockable;

        /// <summary>
        /// Событие на открытие эдитабельной вкладки
        /// </summary>
        public event OpenEditableDockable OpenEditableDockable;

        /// <summary>
        /// Событие на открытие вкладки репорта
        /// </summary>
        public event OpenReportDockable OpenReportDockable;

        public event ManageEditableDockable CloseEditableDockable;
        public event ManageEditableDockable RebuildEditableDockable;

        #endregion

        #region Event Raisers

        public virtual void RaiseOpenDockable(object sender, OpenSimpleDockableEventArgs e)
        {
            OpenDockable(_sender, e);
        }

        public virtual void RaiseOpenSearchDockable(object sender, OpenSearchDockableEventArgs e)
        {
            OpenSearchDockable(_sender, e);
        }

        public virtual void RaiseOpenEditableDockable(object sender, OpenEditableDockableEventArgs e)
        {
            OpenEditableDockable(_sender, e);
        }

        public virtual void RaiseCloseEditableDockable(object sender, ManageEditableDockableEventArgs e)
        {
            CloseEditableDockable(_sender, e);
        }

        public virtual void RaiseEditableDisplayNameChanged(object sender, ManageEditableDockableEventArgs e)
        {
            RebuildEditableDockable(_sender, e);
        }

        public virtual void RaiseOpenReportDockable(object sender, OpenReportDockableEventArgs e)
        {
            OpenReportDockable(_sender, e);
        }

        public void RaiseOpenDockable(string displayName, UserControl view, bool IsInterHost = false)
        {
            RaiseOpenDockable(_sender, new OpenSimpleDockableEventArgs(view, displayName, IsInterHost));
        }

        public void RaiseOpenSearchDockable(string displayName, Type domainType, bool IsInterHost = false)
        {
            RaiseOpenSearchDockable(_sender, new OpenSearchDockableEventArgs(displayName, domainType, IsInterHost));
        }

        public void RaiseOpenSearchDockable(string displayName, ISearchVM searchVM, bool IsInterHost = false)
        {
            RaiseOpenSearchDockable(_sender, new OpenSearchDockableEventArgs(displayName, searchVM, IsInterHost));
        }

        public void RaiseOpenEditableDockable(string displayName, Type domainType, IDomainObject entity, bool IsInterHost = false)
        {
            RaiseOpenEditableDockable(_sender, new OpenEditableDockableEventArgs(displayName, entity, domainType, IsInterHost));
        }

        public void RaiseOpenReportDockable(string displayName, IReport report, bool isDisigner, bool IsInterHost = false)
        {
            RaiseOpenReportDockable(_sender, new OpenReportDockableEventArgs(displayName, report, isDisigner, IsInterHost));
        }

        public void RaiseCloseEditableDockable(Type domainType, int entityId, bool IsInterHost = false)
        {
            RaiseCloseEditableDockable(_sender, new ManageEditableDockableEventArgs(entityId, domainType, IsInterHost));
        }


        public void RaiseEditableDisplayNameChanged(Type domainType, int entityId, bool IsInterHost = false)
        {
            RaiseEditableDisplayNameChanged(_sender, new ManageEditableDockableEventArgs(entityId, domainType, IsInterHost));
        }

        #endregion

        #region Listeners

        private readonly IWeakEventListener _openDockableListener;

        private readonly IWeakEventListener _openSearchDockableListener;

        private readonly IWeakEventListener _openEditableDockableListener;

        private readonly IWeakEventListener _openReportDockableListener;

        private readonly IWeakEventListener _closeEditableDockableListener;

        private readonly IWeakEventListener _rebuildEditableDockableListener;

        #endregion
    }
}