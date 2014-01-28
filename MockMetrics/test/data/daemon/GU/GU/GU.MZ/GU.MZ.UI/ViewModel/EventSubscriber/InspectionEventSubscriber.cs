using System.Windows;

using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.MZ.DataModel.Inspect;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    public class InspectionEventSubscriber : BaseDomainObjectEventSubscriber<Inspection>
    {
        public override void PropertyChangedSubscribe(Inspection sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.InspectionEmployeeList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.InspectionEmployeeList, listener);
                foreach (var inspectionEmployee in sourceObject.InspectionEmployeeList)
                {
                    PropertyChangedWeakEventManager.AddListener(inspectionEmployee, listener);
                }
            }

            if (sourceObject.InspectionExpertList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.InspectionExpertList, listener);
                foreach (var inspectionExpert in sourceObject.InspectionExpertList)
                {
                    PropertyChangedWeakEventManager.AddListener(inspectionExpert, listener);
                }
            }
        }
    }
}
