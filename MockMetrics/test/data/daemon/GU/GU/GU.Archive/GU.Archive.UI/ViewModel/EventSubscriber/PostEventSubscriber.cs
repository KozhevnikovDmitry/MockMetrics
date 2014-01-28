using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.Archive.DataModel;

namespace GU.Archive.UI.ViewModel.EventSubscriber
{
    public class PostEventSubscriber : BaseDomainObjectEventSubscriber<Post>
    {
        public override void PropertyChangedSubscribe(Post sourceObject, System.Windows.IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);
            CollectionChangedWeakEventManager.AddListener(sourceObject.Executors, listener);
        }
    }
}
