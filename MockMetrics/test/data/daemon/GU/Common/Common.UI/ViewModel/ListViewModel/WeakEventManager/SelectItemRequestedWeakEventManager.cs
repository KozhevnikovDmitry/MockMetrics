using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent;

namespace Common.UI.ViewModel.ListViewModel.WeakEventManager
{
    /// <summary>
    /// Менеждер слабых событий <c>IListItemVM.SelectItemRequested</c>.
    /// </summary>
    public class SelectItemRequestedWeakEventManager : WeakEventManagerBase<SelectItemRequestedWeakEventManager, IListItemVM>
    {
        protected override void StartListening(IListItemVM source)
        {
            source.SelectItemRequested += DeliverEvent;
        }

        protected override void StopListening(IListItemVM source)
        {
            source.SelectItemRequested -= DeliverEvent;
        }
    }
}
