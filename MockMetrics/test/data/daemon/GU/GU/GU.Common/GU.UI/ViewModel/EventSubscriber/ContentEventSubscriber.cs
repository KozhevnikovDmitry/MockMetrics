using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;

using GU.DataModel;

namespace GU.UI.ViewModel.EventSubscriber
{
    public class ContentEventSubscriber : BaseDomainObjectEventSubscriber<Content>
    {
        private readonly IDomainObjectEventSubscriber<ContentNode> _contentNodeSubscriber;

        public ContentEventSubscriber(IDomainObjectEventSubscriber<ContentNode> contentNodeSubscriber)
        {
            _contentNodeSubscriber = contentNodeSubscriber;
        }

        public override void PropertyChangedSubscribe(Content sourceObject, System.Windows.IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.RootContentNodes != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.RootContentNodes, listener);
                foreach (var contentNode in sourceObject.RootContentNodes)
                {
                    _contentNodeSubscriber.PropertyChangedSubscribe(contentNode, listener);
                }
            }
        }
    }
}
