using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.DataModel;

namespace GU.UI.ViewModel.EventSubscriber
{
    public class UserEventSubscriber : BaseDomainObjectEventSubscriber<DbUser>
    {
        public override void PropertyChangedSubscribe(DbUser sourceObject, System.Windows.IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);
            CollectionChangedWeakEventManager.AddListener(sourceObject.UserRoleList, listener);
        }
    }
}
