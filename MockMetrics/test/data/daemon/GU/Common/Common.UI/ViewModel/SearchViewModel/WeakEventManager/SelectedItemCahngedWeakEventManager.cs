using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent;

namespace Common.UI.ViewModel.SearchViewModel.WeakEventManager
{
    /// <summary>
    /// Менеждер слабых событий <c>ISelectableItemVM.SelectedResultChanged</c>.
    /// </summary>
    public class SelectedResultCahngedWeakEventManager : WeakEventManagerBase<SelectedResultCahngedWeakEventManager, ISelectableItemVM>
    {
        protected override void StartListening(ISelectableItemVM source)
        {
            source.SelectedResultChanged += DeliverEvent;
        }

        protected override void StopListening(ISelectableItemVM source)
        {
            source.SelectedResultChanged -= DeliverEvent;
        }
    }
}
