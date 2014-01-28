using System.Linq;

using Common.BL.Search.SearchModification;
using Common.DA.Interface;

namespace Common.BL.Search
{
    /// <summary>
    /// Интерфейс классов, предназначенных для поиска доменных объектов.
    /// </summary>
    /// <remarks>
    /// Классы searcher'ы предназначенны для хранения linq-запросов поиска доменных объектов по шаблонному объекту того же типа.
    /// Searcher'ы используются в шаблонной модификации(<see cref="TemplatedSearchModification{T}"/>) стратегий поиска.
    /// </remarks>
    public interface IDomainObjectSearcher
    {
        /// <summary>
        /// Возрвщает ленивый результат запроса поиска доменных объектов заданного типа.
        /// </summary>
        /// <typeparam name="T">Тип доменных объектов</typeparam>
        /// <param name="templateDomainObject">Шаблон поиска</param>
        /// <param name="dbManager">Менеджер доступа к данным</param>
        /// <returns>Результат запроса поиска</returns>
        IQueryable<T> Search<T>(T templateDomainObject, IDomainDbManager dbManager) where T : IDomainObject;

        /// <summary>
        /// Добавляет в данный экземпляр searcher'а возможности поиска другого экземпляра.
        /// </summary>
        /// <param name="searcher">Объект осуществляющий поиск</param>
        void Merge(IDomainObjectSearcher searcher);
    }
}
