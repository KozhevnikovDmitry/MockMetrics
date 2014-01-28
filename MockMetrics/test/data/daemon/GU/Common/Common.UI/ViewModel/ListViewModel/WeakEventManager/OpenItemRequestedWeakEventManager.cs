using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent;

namespace Common.UI.ViewModel.ListViewModel.WeakEventManager
{
    /// <summary>
    /// Менеждер слабых событий <c>IListItemVM.OpenItemRequested</c>.
    /// </summary>
    public class OpenItemRequestedWeakEventManager : WeakEventManagerBase<OpenItemRequestedWeakEventManager, IListItemVM>
    {
        protected override void StartListening(IListItemVM source)
        {
            source.OpenItemRequested += DeliverEvent;
        }

        protected override void StopListening(IListItemVM source)
        {
            source.OpenItemRequested -= DeliverEvent;
        }
    }
}
