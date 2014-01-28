using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.DataModel;

namespace GU.UI.ViewModel.EventSubscriber
{
    public class TaskEventSubscriber : BaseDomainObjectEventSubscriber<Task>
    {

        private readonly IDomainObjectEventSubscriber<Content> _contentSubscriber;

        public TaskEventSubscriber(IDomainObjectEventSubscriber<Content> contentSubscriber)
        {
            _contentSubscriber = contentSubscriber;
        }

        public override void PropertyChangedSubscribe(Task sourceObject, System.Windows.IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);
            if (sourceObject.StatusList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.StatusList, listener);
            }

            if (sourceObject.Content != null)
            {
                _contentSubscriber.PropertyChangedSubscribe(sourceObject.Content, listener);
            }
        }
    }


}
