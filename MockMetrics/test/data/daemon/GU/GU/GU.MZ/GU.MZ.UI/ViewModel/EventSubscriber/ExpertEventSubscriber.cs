using System;
using System.Windows;

using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;

using GU.MZ.DataModel.Person;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    /// <summary>
    /// Класс отвественный подписку на событие <c>PropertyChanged</c> эксперта
    /// </summary>
    public class ExpertEventSubscriber : BaseDomainObjectEventSubscriber<Expert>
    {
        #region Overrides of BaseDomainObjectEventSubscriber<Expert>

        /// <summary>
        /// Интерфес классов отвественных подписку на событие <c>PropertyChanged</c> доменных объектов через слабые события.
        /// </summary>
        /// <param name="sourceObject">Доменный объект хозяин событий</param>
        /// <param name="listener">Слушатель слабых событий</param>
        /// <exception cref="ArgumentNullException">Слушатель является Null</exception>
        public override void PropertyChangedSubscribe(Expert sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);
            PropertyChangedWeakEventManager.AddListener(sourceObject.ExpertState, listener);
            sourceObject.ExpertStateTypeChanged += t => PropertyChangedWeakEventManager.AddListener(sourceObject.ExpertState, listener);
        }

        #endregion
    }
}
