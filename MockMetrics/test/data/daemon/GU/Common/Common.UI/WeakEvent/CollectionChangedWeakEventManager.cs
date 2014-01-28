using System.Collections.Specialized;

namespace Common.UI.WeakEvent
{
    /// <summary>
    /// Менеждер слабых событий <c>NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)</c>.
    /// </summary>
    /// <remarks>
    /// Использовать для источников реализующих <c>INotifyCollectionChanged</c>.
    /// </remarks>
    public class CollectionChangedWeakEventManager : WeakEventManagerBase<CollectionChangedWeakEventManager, INotifyCollectionChanged>
    {
        protected override void StartListening(INotifyCollectionChanged source)
        {
            source.CollectionChanged += DeliverEvent;
        }

        protected override void StopListening(INotifyCollectionChanged source)
        {
            source.CollectionChanged -= DeliverEvent;
        }
    }
}
