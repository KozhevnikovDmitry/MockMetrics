using System.Windows;

using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.MZ.DataModel.Inspect;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    public class DocumentExpertiseEventSubscriber : BaseDomainObjectEventSubscriber<DocumentExpertise>
    {
        public override void PropertyChangedSubscribe(DocumentExpertise sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.ExperiseResultList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.ExperiseResultList, listener);
                foreach (var expertiseResult in sourceObject.ExperiseResultList)
                {
                    PropertyChangedWeakEventManager.AddListener(expertiseResult, listener);
                }
            }

        }
    }
}
