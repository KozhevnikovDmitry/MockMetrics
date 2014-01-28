using System.ComponentModel;

namespace Common.UI.WeakEvent
{
    /// <summary>
    /// Менеждер слабых событий <c>PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)</c>.
    /// </summary>
    /// <remarks>
    /// Использовать для источников реализующих <c>INotifyPropertyChanged</c>.
    /// </remarks>
    public class PropertyChangedWeakEventManager : WeakEventManagerBase<PropertyChangedWeakEventManager, INotifyPropertyChanged>
    {
        protected override void StartListening(INotifyPropertyChanged source)
        {
            source.PropertyChanged += DeliverEvent;
        }

        protected override void StopListening(INotifyPropertyChanged source)
        {
            source.PropertyChanged -= DeliverEvent;
        }
    }
}
