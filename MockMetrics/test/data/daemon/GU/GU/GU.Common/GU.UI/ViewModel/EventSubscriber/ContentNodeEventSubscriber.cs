using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;

using GU.DataModel;

namespace GU.UI.ViewModel.EventSubscriber
{
    public class ContentNodeEventSubscriber : BaseDomainObjectEventSubscriber<ContentNode>
    {
        public override void PropertyChangedSubscribe(ContentNode sourceObject, System.Windows.IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.ChildContentNodes != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.ChildContentNodes, listener);
                foreach (var contentNode in sourceObject.ChildContentNodes)
                {
                    this.PropertyChangedSubscribe(contentNode, listener);
                }
            }
        }
    }
}