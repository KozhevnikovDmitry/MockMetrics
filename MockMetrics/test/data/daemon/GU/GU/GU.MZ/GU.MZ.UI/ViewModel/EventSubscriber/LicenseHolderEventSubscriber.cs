using System.Windows;

using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;

using GU.MZ.DataModel.Holder;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    /// <summary>
    /// Класс отвественный подписку на событие <c>PropertyChanged</c> лицензиата.
    /// </summary>
    public class LicenseHolderEventSubscriber : BaseDomainObjectEventSubscriber<LicenseHolder>
    {
        /// <summary>
        /// Подписывает слушателя слабых событий на события <c>PropertyChanged</c> лицензиата.
        /// </summary>
        /// <param name="sourceObject">Объект том</param>
        /// <param name="listener">Слушатель слабых событий</param>
        public override void PropertyChangedSubscribe(LicenseHolder sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);
            if (sourceObject.RequisitesList != null) 
            {
                foreach (var holderRequisites in sourceObject.RequisitesList)
                {
                    PropertyChangedWeakEventManager.AddListener(holderRequisites, listener);
                }
            }
        }
    }
}
