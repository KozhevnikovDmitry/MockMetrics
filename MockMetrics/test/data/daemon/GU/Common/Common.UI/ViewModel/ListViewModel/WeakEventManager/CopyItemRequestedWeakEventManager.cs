using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent;

namespace Common.UI.ViewModel.ListViewModel.WeakEventManager
{
    /// <summary>
    /// Менеждер слабых событий <c>IListItemVM.CopyItemRequested</c>.
    /// </summary>
    public class CopyItemRequestedWeakEventManager : WeakEventManagerBase<CopyItemRequestedWeakEventManager, IListItemVM>
    {
        protected override void StartListening(IListItemVM source)
        {
            source.CopyItemRequested += DeliverEvent;
        }

        protected override void StopListening(IListItemVM source)
        {
            source.CopyItemRequested -= DeliverEvent;
        }
    }
}
