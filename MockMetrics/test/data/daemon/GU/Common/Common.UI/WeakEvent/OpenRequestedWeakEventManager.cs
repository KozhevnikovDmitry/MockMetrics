using Common.UI.ViewModel.Interfaces;

namespace Common.UI.WeakEvent
{
    /// <summary>
    /// Менеждер слабых событий <c>ISearchVM.ChooseResultRequested</c>.
    /// </summary>
    public class OpenRequestedWeakEventManager : WeakEventManagerBase<OpenRequestedWeakEventManager, ISearchVM>
    {
        /// <summary>
        /// Инициирует подписку на событие <c>ISearchVM.ChooseResultRequested</c> источника
        /// </summary>
        /// <param name="source">Источник события</param>
        protected override void StartListening(ISearchVM source)
        {
            source.ChooseResultRequested += DeliverEvent;
        }

        /// <summary>
        /// Прекращает подписку на  событие <c>ISearchVM.ChooseResultRequested</c> источника
        /// </summary>
        /// <param name="source">Источник события</param>
        protected override void StopListening(ISearchVM source)
        {
            source.ChooseResultRequested -= DeliverEvent;
        }
    }
}
