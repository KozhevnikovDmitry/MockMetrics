using System.Windows;
using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    public class ExpertiseOrderEventSubsЫcriber : BaseDomainObjectEventSubscriber<ExpertiseOrder>
    {
        public override void PropertyChangedSubscribe(ExpertiseOrder sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.ExpertiseOrderAgreeList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.ExpertiseOrderAgreeList, listener);
                foreach (var agree in sourceObject.ExpertiseOrderAgreeList)
                {
                    PropertyChangedWeakEventManager.AddListener(agree, listener);
                }
            }
        }
    }
}
