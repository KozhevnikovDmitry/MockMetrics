using System.Windows;

using Common.UI.WeakEvent.EventSubscriber;

using GU.MZ.DataModel.Person;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    /// <summary>
    /// Класс отвественный подписку на событие <c>PropertyChanged</c> сотрудника.
    /// </summary>
    public class EmployeeEventSubscriber : BaseDomainObjectEventSubscriber<Employee>
    {
        /// <summary>
        /// Подписывает слушателя слабых событий на события <c>PropertyChanged</c> сотрудника.
        /// </summary>
        /// <param name="sourceObject">Объект сотрудник</param>
        /// <param name="listener">Слушатель слабых событий</param>
        public override void PropertyChangedSubscribe(Employee sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);
        }
    }
}
