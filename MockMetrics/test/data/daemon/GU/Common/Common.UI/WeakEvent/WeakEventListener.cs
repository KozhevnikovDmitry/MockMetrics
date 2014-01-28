using System;
using System.Windows;

namespace Common.UI.WeakEvent
{   
    /// <summary>
    /// Класс обобщённый подписчик на слабые события <c>Delegate(object sender, TEventArgs args)</c>.
    /// </summary>
    /// <typeparam name="TEventArgs">Тип аргументов события</typeparam>
    /// <remarks>
    /// Класс попёрт с http://badecho.com/2012/04/a-generic-iweakeventlistener/
    /// </remarks>
    public class WeakEventListener<TEventArgs> : IWeakEventListener
        where TEventArgs : EventArgs
    {
        /// <summary>
        /// Делегат обработчика событий.
        /// </summary>
        private readonly EventHandler<TEventArgs> _handler;
                
        /// <summary>
        /// Класс обобщённый подписчик на слабые события <c>Delegate(object sender, TEventArgs args)</c>.
        /// </summary>
        /// <param name="handler">Делегат обработчика событий</param>
        public WeakEventListener(EventHandler<TEventArgs> handler)
        {
            if (handler == null)
                throw new ArgumentNullException("Handler is null");
            _handler = handler;
        }

        /// <summary>
        /// Выполняет обработку события. Возвращет флаг обработанности события: обработано - true, необработано -false.
        /// </summary>
        /// <param name="managerType">Тип менеждера слабых событий</param>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <returns>Флаг обработанности события</returns>
        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            TEventArgs eventArgs = e as TEventArgs;

            if (null == eventArgs)
                return false;

            _handler(sender, eventArgs);

            return true;
        }
    }
}
