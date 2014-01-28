using System.Windows;

using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;

using GU.MZ.DataModel.Dossier;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    /// <summary>
    /// Класс отвественный подписку на событие <c>PropertyChanged</c> тома лицензионного дела.
    /// </summary>
    public class DossierFileEventSubscriber : BaseDomainObjectEventSubscriber<DossierFile>
    {
        /// <summary>
        /// Подписчик слабых событий для этапов ведения тома
        /// </summary>
        private readonly IDomainObjectEventSubscriber<DossierFileScenarioStep> _fileStepSubscriber;

        /// <summary>
        ///  Класс отвественный подписку на событие <c>PropertyChanged</c> тома лицензионного дела.
        /// </summary>
        /// <param name="fileStepSubscriber">Подписчик слабых событий для этапов ведения тома</param>
        public DossierFileEventSubscriber(IDomainObjectEventSubscriber<DossierFileScenarioStep> fileStepSubscriber)
        {
            _fileStepSubscriber = fileStepSubscriber;
        }

        /// <summary>
        /// Подписывает слушателя слабых событий на события <c>PropertyChanged</c> тома лицензионного дела.
        /// </summary>
        /// <param name="sourceObject">Объект том</param>
        /// <param name="listener">Слушатель слабых событий</param>
        public override void PropertyChangedSubscribe(DossierFile sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.DossierFileStepList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.DossierFileStepList, listener);
                foreach (var step in sourceObject.DossierFileStepList)
                {
                    _fileStepSubscriber.PropertyChangedSubscribe(step, listener);
                }
            }

        }
    }
}
