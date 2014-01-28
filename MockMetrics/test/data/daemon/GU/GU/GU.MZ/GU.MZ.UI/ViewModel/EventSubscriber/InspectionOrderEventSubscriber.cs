using System.Windows;
using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    public class InspectionOrderEventSubscriber : BaseDomainObjectEventSubscriber<InspectionOrder>
    {
        public override void PropertyChangedSubscribe(InspectionOrder sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.InspectionOrderAgreeList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.InspectionOrderAgreeList, listener);
                foreach (var agree in sourceObject.InspectionOrderAgreeList)
                {
                    PropertyChangedWeakEventManager.AddListener(agree, listener);
                }
            } 
            
            if (sourceObject.InspectionOrderExpertList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.InspectionOrderExpertList, listener);
                foreach (var expert in sourceObject.InspectionOrderExpertList)
                {
                    PropertyChangedWeakEventManager.AddListener(expert, listener);
                }
            }
        }
    }
}
