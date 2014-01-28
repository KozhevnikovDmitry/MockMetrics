using System.Linq;
using System.Threading;

using Common.DA.Interface;

namespace Common.BL.Search.SearchModification
{
    /// <summary>
    /// Делегат методов поиска доменных объектов.
    /// </summary>
    /// <typeparam name="T">Тип доменных объектов</typeparam>
    /// <param name="searchData">Объект с информацией для поиска</param>
    /// <param name="dbManager">Менеджер базы данных</param>
    /// <param name="cansellationToken">Токен отмены выполнения</param>
    /// <returns>Нематериализованный запрос поиска доменных объектов</returns>
    public delegate IQueryable<T> SearchDelegate<T>(ISearchData searchData, IDomainDbManager dbManager, CancellationToken cansellationToken) where T : IDomainObject;

    /// <summary>
    /// Делегат соединения таблицы доменных объектов T с другой таблицей доменных объектов.
    /// </summary>
    /// <typeparam name="T">Тип доменных объектов</typeparam>
    /// <param name="joinQuery">Таблица доменных объектов T</param>
    /// <param name="innerQuery">Таблица доменных объектов для соединения</param>
    /// <returns>Соединение таблиц доменных объектов</returns>
    public delegate IQueryable<T> DomainJoinDelegate<T>(IQueryable<T> joinQuery, IQueryable innerQuery) where T : IDomainObject;

    /// <summary>
    /// Интерфейс классов модификаций для стратегий поиска доменных объектов.
    /// </summary>
    /// <typeparam name="T">Тип доменных объектов</typeparam>
    public interface ISearchModification<T> where T : IDomainObject
    {
        /// <summary>
        /// Возвращает делегат на метод поиска доменных объектов
        /// </summary>
        /// <param name="searchData">Объект с информацией для поиска</param>
        /// <returns>Делегат метода поиска</returns>
        SearchDelegate<T> GetSearchActionDelegate(ISearchData searchData);
    }
}
