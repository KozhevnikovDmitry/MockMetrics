using System.Threading;

using Common.BL.DomainContext;
using Common.BL.Search.SearchModification;
using Common.BL.Search.SearchSpecification;
using Common.DA.Interface;

namespace Common.BL.Search
{
    /// <summary>
    /// Интерфейс для стратегий поиска объектов <c>T</c>.
    /// </summary>
    public interface ISearchStrategy<T> : IDomainDependent where T : IDomainObject
    {
        /// <summary>
        /// Объект-источник токенов отмены выполнения.
        /// </summary>
        CancellationTokenSource CancelSource { get; }

        /// <summary>
        /// Возвращает флаг возможности получения другой страницы результатов.
        /// </summary>
        /// <returns>Флаг доступности страниц</returns>
        bool CanGetAnotherPage();

        /// <summary>
        /// Выполняет поиск доменных объектов по согласно входящей информации. Генерирует событие <c>SearchResultReady</c>.
        /// </summary>
        /// <param name="searchData">Объект с информацией для поиска</param>
        void Search(ISearchData searchData);

        /// <summary>
        /// Получает новую страницу результатов поиска по последнему запросу поиска. Генерирует событие <c>SearchResultReady</c>.
        /// </summary>
        /// <param name="position">Позиция начала возвращаемой страницы результатов</param>
        void SearchPage(int position);

        /// <summary>
        /// Событие, извещающее об окончании процедуры поиска.
        /// </summary>
        event SearchResultReadyDelegate<T> SearchResultReady;

        /// <summary>
        /// Возвращает или устанаваливает режим поиска.
        /// </summary>
        SearchMode SearchMode { get; set; }

        IDomainObjectSearcher Searcher { set; }
    }
}
