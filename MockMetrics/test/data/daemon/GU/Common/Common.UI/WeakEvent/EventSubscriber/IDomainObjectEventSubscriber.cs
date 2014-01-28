using System.ComponentModel;
using System.Windows;

using Common.DA.Interface;

namespace Common.UI.WeakEvent.EventSubscriber
{
    public interface IDomainObjectEventSubscriber
    {
        
    }

    /// <summary>
    /// Интерфейс классов отвественных подписку на событие <c>PropertyChanged</c> доменных объектов через слабые события.
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    /// <remarks>
    /// Классы subscriber'ы необходимы для слабой подписки на <c>PropertyChanged</c> сложных доменных объектов
    /// с множественными ассоциациями один ко многим. 
    /// Subscriber обеспечивает слушателя событием даже в случае, когда меняется свойство одного объекта внутри ассоциированного списка.
    /// Subscriber должен обходить доменный объект до необходимого уровня и подписываться на <c>PropertyChanged</c> ассоциированных объектов.
    /// </remarks>
    public interface IDomainObjectEventSubscriber<in T> : IDomainObjectEventSubscriber where T : IDomainObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Интерфес классов отвественных подписку на событие <c>PropertyChanged</c> доменных объектов через слабые события.
        /// </summary>
        /// <param name="sourceObject">Доменный объект хозяин событий</param>
        /// <param name="listener">Слушатель слабых событий</param>
        void PropertyChangedSubscribe(T sourceObject, IWeakEventListener listener);
    }
}
