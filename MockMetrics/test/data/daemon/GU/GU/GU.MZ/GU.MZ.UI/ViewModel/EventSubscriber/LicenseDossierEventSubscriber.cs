using System.Windows;

using Common.UI.WeakEvent.EventSubscriber;

using GU.MZ.DataModel.Dossier;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    /// <summary>
    /// Класс отвественный подписку на событие <c>PropertyChanged</c> лицензионного дела.
    /// </summary>
    public class LicenseDossierEventSubscriber : BaseDomainObjectEventSubscriber<LicenseDossier>
    {
        /// <summary>
        /// Подписывает слушателя слабых событий на события <c>PropertyChanged</c> лицензионного дела.
        /// </summary>
        /// <param name="sourceObject">Объект том</param>
        /// <param name="listener">Слушатель слабых событий</param>
        public override void PropertyChangedSubscribe(LicenseDossier sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);
        }
    }
}
