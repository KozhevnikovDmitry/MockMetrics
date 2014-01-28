using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.EventSubscriber
{
    public class DeclarerEventSubscriber :  BaseDomainObjectEventSubscriber<Person>
    {
        public override void PropertyChangedSubscribe(Person sourceObject, System.Windows.IWeakEventListener listener)
        {

            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.Addresses != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.Addresses, listener);

                foreach (var address in sourceObject.Addresses)
                {
                    PropertyChangedWeakEventManager.AddListener(address, listener);
                }
            }

            if (sourceObject.Documents != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.Documents, listener);

                foreach (var doc in (sourceObject.Documents))
                {
                    PropertyChangedWeakEventManager.AddListener(doc,listener);
                }
            }
        }
    }
}
