using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.WeakEvent;

namespace Common.UI.ViewModel.AvalonInteraction.InteractionEvents
{
    /// <summary>
    /// Менеджер слабых событий запроса на открытие простой вкладки на AvalonDock.
    /// </summary>
    public class AvalonInteractWeakEventManager : WeakEventManagerBase<AvalonInteractWeakEventManager, IAvalonDockCaller>
    {
        protected override void StartListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.OpenDockable += this.DeliverEvent;
        }

        protected override void StopListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.OpenDockable -= this.DeliverEvent;
        }
    }

    /// <summary>
    /// Менеджер слабых событий запроса на открытие вкладки поиска на AvalonDock.
    /// </summary>
    public class AvalonSearchInteractWeakEventManager : WeakEventManagerBase<AvalonSearchInteractWeakEventManager, IAvalonDockCaller>
    {
        protected override void StartListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.OpenSearchDockable += this.DeliverEvent;
        }

        protected override void StopListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.OpenSearchDockable -= this.DeliverEvent;
        }
    }

    /// <summary>
    /// Менеджер слабых событий запроса на открытие вкладки редактирования доменного объекта на AvalonDock.
    /// </summary>
    public class AvalonOpenEditableInteractWeakEventManager : WeakEventManagerBase<AvalonOpenEditableInteractWeakEventManager, IAvalonDockCaller>
    {
        protected override void StartListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.OpenEditableDockable += this.DeliverEvent;
        }

        protected override void StopListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.OpenEditableDockable -= this.DeliverEvent;
        }
    }

    /// <summary>
    /// Менеджер слабых событий запроса на открытие вклакди с отчётом на на AvalonDock.
    /// </summary>
    public class AvalonReportInteractWeakEventManager : WeakEventManagerBase<AvalonReportInteractWeakEventManager, IAvalonDockCaller>
    {
        protected override void StartListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.OpenReportDockable += this.DeliverEvent;
        }

        protected override void StopListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.OpenReportDockable -= this.DeliverEvent;
        }
    }
    
    /// <summary>
    /// Менеджер слабых событий запроса на закрытия вкладки редактирования доменного объекта на AvalonDock.
    /// </summary>
    public class AvalonCloseEditableInteractWeakEventManager : WeakEventManagerBase<AvalonCloseEditableInteractWeakEventManager, IAvalonDockCaller>
    {
        protected override void StartListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.CloseEditableDockable += this.DeliverEvent;
        }

        protected override void StopListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.CloseEditableDockable -= this.DeliverEvent;
        }
    }

    /// <summary>
    /// Менеджер слабых событий запроса на ребилда вкладки редактирования доменного объекта на AvalonDock.
    /// </summary>
    public class AvalonRebuildEditableInteractWeakEventManager : WeakEventManagerBase<AvalonRebuildEditableInteractWeakEventManager, IAvalonDockCaller>
    {
        protected override void StartListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.RebuildEditableDockable += this.DeliverEvent;
        }

        protected override void StopListening(IAvalonDockCaller source)
        {
            source.AvalonInteractor.RebuildEditableDockable -= this.DeliverEvent;
        }
    }


}
