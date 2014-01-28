using System.Windows;
using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    public class StandartOrderEventSubscriber : BaseDomainObjectEventSubscriber<StandartOrder>
    {
        public override void PropertyChangedSubscribe(StandartOrder sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.DetailList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.DetailList, listener);
                foreach (var detail in sourceObject.DetailList)
                {
                    PropertyChangedWeakEventManager.AddListener(detail, listener);
                }
            }

            if (sourceObject.AgreeList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.AgreeList, listener);
                foreach (var agree in sourceObject.AgreeList)
                {
                    PropertyChangedWeakEventManager.AddListener(agree, listener);
                }
            }
        }
    }
}
