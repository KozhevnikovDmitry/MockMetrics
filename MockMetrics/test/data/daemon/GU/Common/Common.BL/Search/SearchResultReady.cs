using System;
using System.Collections.Generic;
using Common.DA.Interface;

namespace Common.BL.Search
{
    /// <summary>
    /// Делегат методов-обработчиков события готовности результатов поиска.
    /// </summary>
    /// <param name="e">Аргумент события</param>
    public delegate void SearchResultReadyDelegate<T>(SearchResultReadyEventArgs<T> e) where T : IDomainObject;

    /// <summary>
    /// Класс, прдеставляющий аргументы события готовности результатов поиска.
    /// </summary>
    public class SearchResultReadyEventArgs<T> : EventArgs where T : IDomainObject
    {
        /// <summary>
        /// Класс, прдеставляющий аргументы события готовности результатов поиска.
        /// </summary>
        /// <param name="resultPage">Возвращаемая страница результатов</param>
        /// <param name="resultCount">Общее число результатов</param>
        /// <param name="position">Позиция начала страницы в общем списке результатов</param>
        public SearchResultReadyEventArgs(List<T> resultPage, 
                                          int resultCount, 
                                          int position)
        {
            ResultPage = resultPage;
            ResultCount = resultCount;
            Position = position;
            IsFailed = false;
        }

        /// <summary>
        /// Класс, прдеставляющий аргументы события готовности результатов поиска.
        /// </summary>
        /// <param name="exception">Объект исключение, сообщающий об ошибке во время поиска</param>
        public SearchResultReadyEventArgs(Exception exception)
        {
            Exception = exception;
            IsFailed = true;
        }

        /// <summary>
        /// Возвращает страницу результатов поиска.
        /// </summary>
        public List<T> ResultPage { get; private set; }

        /// <summary>
        /// Возвращает общее число результатов.
        /// </summary>
        public int ResultCount { get; private set; }

        /// <summary>
        /// Возвращает позицию начала страницы в общем списке результатов.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Возвращает объект исключение, сообщающий об ошибке во время поиска.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Возвращает <c>true</c>, если поиск завершился с ошибкой.
        /// </summary>
        public bool IsFailed { get; private set; }
    }
}
