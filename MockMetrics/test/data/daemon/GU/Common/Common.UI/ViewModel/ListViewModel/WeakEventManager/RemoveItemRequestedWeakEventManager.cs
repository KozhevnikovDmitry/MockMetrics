using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent;

namespace Common.UI.ViewModel.ListViewModel.WeakEventManager
{
    /// <summary>
    /// Менеждер слабых событий <c>IListItemVM.RemoveItemRequested</c>.
    /// </summary>
    public class RemoveItemRequestedWeakEventManager : WeakEventManagerBase<RemoveItemRequestedWeakEventManager, IListItemVM>
    {
        protected override void StartListening(IListItemVM source)
        {
            source.RemoveItemRequested += DeliverEvent;
        }

        protected override void StopListening(IListItemVM source)
        {
            source.RemoveItemRequested -= DeliverEvent;
        }
    }
}
