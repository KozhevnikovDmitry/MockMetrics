using System;
using System.ComponentModel;
using System.Windows;

using Common.DA.Interface;

namespace Common.UI.WeakEvent.EventSubscriber
{
    /// <summary>
    /// Базовый класс для классов отвественных подписку на событие <c>PropertyChanged</c> доменных объектов через слабые события.
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public abstract class BaseDomainObjectEventSubscriber<T> : IDomainObjectEventSubscriber<T> where T : IDomainObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Интерфес классов отвественных подписку на событие <c>PropertyChanged</c> доменных объектов через слабые события.
        /// </summary>
        /// <param name="sourceObject">Доменный объект хозяин событий</param>
        /// <param name="listener">Слушатель слабых событий</param>
        /// <exception cref="ArgumentNullException">Слушатель является Null</exception>
        public virtual void PropertyChangedSubscribe(T sourceObject, IWeakEventListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("Слушатель является Null");
            }

            PropertyChangedWeakEventManager.AddListener(sourceObject, listener);
        }
    }
}
