using System;
using System.Diagnostics;
using System.Windows;

namespace Common.UI.WeakEvent
{
    /// <summary>
    /// Базовый класс, предназначеный для управления событиями по паттерну слабых событий.
    /// </summary>
    /// <typeparam name="TManager">Тип менеджера событий</typeparam>
    /// <typeparam name="TEventSource">Тип источника событий</typeparam>
    /// <remarks>
    /// Наследник данного класса управляет подписками на события источников определённого типа событий.
    /// Поддержкой слабых событий занимается wpf-шный <c>WeakEventManager</c>.
    /// В двух словах: позволяет избежать утечек памяти на источники собыьтий к тех случаях, когда подписчик живёт дольше источников.
    /// Шаблон слабых событий http://habrahabr.ru/post/89529/
    /// Класс попёрт с http://devdefined-tools.googlecode.com/svn/trunk/presentations/2009/DSL/BooDslExampleApp/src/ICSharpCode.AvalonEdit/Utils/WeakEventManagerBase.cs
    /// </remarks>
    public abstract class WeakEventManagerBase<TManager, TEventSource> : WeakEventManager
        where TManager : WeakEventManagerBase<TManager, TEventSource>, new()
        where TEventSource : class
    {
        /// <summary>
        /// Базовый класс, предназначеный для управления событиями по паттерну слабых событий.
        /// </summary>
        protected WeakEventManagerBase()
        {
            Debug.Assert(GetType() == typeof(TManager));
        }

        /// <summary>
        /// Добавляет подписчика на событие источника.
        /// </summary>
        /// <param name="source">Объект источник событий</param>
        /// <param name="listener">Объект слушатель событий</param>
        public static void AddListener(TEventSource source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>
        /// Удялет подписчика на событие источника
        /// </summary>
        /// <param name="source">Объект источник событий</param>
        /// <param name="listener">Объект слушатель событий</param>
        public static void RemoveListener(TEventSource source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        /// <summary>
        /// Добавляет подписку на событие источника.
        /// </summary>
        /// <param name="source">Объект источник событий</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected sealed override void StartListening(object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            StartListening((TEventSource)source);
        }

        /// <summary>
        /// Снимает подписку на событие источника.
        /// </summary>
        /// <param name="source">Объект источник событий</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected sealed override void StopListening(object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            StopListening((TEventSource)source);
        }

        /// <summary>
        /// Добавляет подписку на событие источника.
        /// </summary>
        /// <param name="source">Объект источник событий</param>
        protected abstract void StartListening(TEventSource source);


        /// <summary>
        /// Снимает подписку на событие источника.
        /// </summary>
        /// <param name="source">Объект источник событий</param>
        protected abstract void StopListening(TEventSource source);

        /// <summary>
        /// Возвращает текущий менеждер слабых событий.
        /// </summary>
        protected static TManager CurrentManager
        {
            get
            {
                Type managerType = typeof(TManager);
                TManager manager = (TManager)GetCurrentManager(managerType);
                if (manager == null)
                {
                    manager = new TManager();
                    SetCurrentManager(managerType, manager);
                }
                return manager;
            }
        }
    }
}
