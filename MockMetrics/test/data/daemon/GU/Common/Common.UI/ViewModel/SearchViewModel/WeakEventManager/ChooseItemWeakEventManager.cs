using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent;

namespace Common.UI.ViewModel.SearchViewModel.WeakEventManager
{
    /// <summary>
    /// Менеждер слабых событий <c>ISelectableItemVM.ChooseResultRequested</c>.
    /// </summary>
    public class ChooseResultRequestedWeakEventManager : WeakEventManagerBase<ChooseResultRequestedWeakEventManager, ISelectableItemVM>
    {
        /// <summary>
        /// Инициирует подписку на событие <c>ISelectableItemVM.ChooseResultRequested</c> источника
        /// </summary>
        /// <param name="source">Источник события</param>
        protected override void StartListening(ISelectableItemVM source)
        {
            source.ChooseResultRequested += DeliverEvent;
        }

        /// <summary>
        /// Прекращает подписку на  событие <c>ISelectableItemVM.ChooseResultRequested</c> источника
        /// </summary>
        /// <param name="source">Источник события</param>
        protected override void StopListening(ISelectableItemVM source)
        {
            source.ChooseResultRequested -= DeliverEvent;
        }
    }
}
