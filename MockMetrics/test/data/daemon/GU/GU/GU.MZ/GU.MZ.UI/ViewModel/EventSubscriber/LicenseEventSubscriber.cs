using System.Windows;

using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;

using GU.MZ.DataModel.Licensing;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    /// <summary>
    /// Класс отвественный подписку на событие <c>PropertyChanged</c> лицензии.
    /// </summary>
    public class LicenseEventSubscriber : BaseDomainObjectEventSubscriber<License>
    {
        /// <summary>
        /// Подписывает слушателя слабых событий на события <c>PropertyChanged</c> лицензии.
        /// </summary>
        /// <param name="sourceObject">Объект лицензия</param>
        /// <param name="listener">Слушатель слабых событий</param>
        public override void PropertyChangedSubscribe(License sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);
            if (sourceObject.LicenseObjectList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.LicenseObjectList, listener);
                sourceObject.LicenseObjectList.ForEach(t => PropertyChangedWeakEventManager.AddListener(t, listener));
            }

            if (sourceObject.LicenseStatusList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.LicenseStatusList, listener);
                sourceObject.LicenseStatusList.ForEach(t => PropertyChangedWeakEventManager.AddListener(t, listener));
            }

            if (sourceObject.LicenseRequisitesList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.LicenseRequisitesList, listener);
                sourceObject.LicenseRequisitesList.ForEach(t => PropertyChangedWeakEventManager.AddListener(t, listener));
            }
        }
    }
}
