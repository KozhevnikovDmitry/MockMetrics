using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

using Common.BL.Search.SearchSpecification;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL.Search.SearchModification
{
    /// <summary>
    /// Класс представляющий модификацию для шаблонного поиска доменных объектов
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public class TemplatedSearchModification<T> : ISearchModification<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Выражение для фильтрации поиска по умолчанию
        /// </summary>
        private readonly Expression<Func<T, bool>> _defaultSearchFilter;

        /// <summary>
        /// Выражение для сортировки поиска по умолчанию
        /// </summary>
        private readonly Expression<Func<T, object>> _defaultOrder;

        /// <summary>
        /// Направление для сортировки поиска по умолчанию
        /// </summary>
        private readonly OrderDirection _defaultOrderDirection;

        /// <summary>
        /// Словарь делегатов соединения доменных таблиц.
        /// </summary>
        private readonly Dictionary<Type, DomainJoinDelegate<T>> _domainJoinDictionary;

        /// <summary>
        /// Объект-поисковик доменных объектов
        /// </summary>
        private readonly IDomainObjectSearcher _searcher;

        /// <summary>
        /// Класс представляющий модификацию для шаблонного поиска доменных объектов
        /// </summary>
        /// <param name="domainJoinDictionary">Словарь делегатов соединения доменных таблиц</param>
        /// <param name="searcher">Объект-поисковик доменных объектов</param>
        /// <param name="defaultSearchFilter">Выражение для фильтрации поиска по умолчанию</param>
        /// <param name="defaultOrder">Выражение для сортировки поиска по умолчанию</param>
        /// <param name="defaultOrderDirection">Направление для сортировки поиска по умолчанию</param>
        public TemplatedSearchModification(Dictionary<Type, 
            DomainJoinDelegate<T>> domainJoinDictionary, 
            IDomainObjectSearcher searcher, 
            Expression<Func<T, bool>> defaultSearchFilter,
            Expression<Func<T, object>> defaultOrder,
            OrderDirection defaultOrderDirection)
        {
            _domainJoinDictionary = domainJoinDictionary;
            _searcher = searcher;
            _defaultSearchFilter = defaultSearchFilter;
            _defaultOrder = defaultOrder;
            _defaultOrderDirection = defaultOrderDirection;
        }

        /// <summary>
        /// Возвращает делегат на метод поиска доменных объектов
        /// </summary>
        /// <param name="searchData">Объект с информацией для поиска</param>
        /// <returns>Делегат метода поиска</returns>
        public SearchDelegate<T> GetSearchActionDelegate(ISearchData searchData) 
        {
            switch (searchData.SearchActionType)
            {
                case SearchActionType.Default:
                    {
                        return DefaultSearch;
                    }
                case SearchActionType.Self:
                    {
                        return SelfSearch;
                    }
                case SearchActionType.OrdinalJoin:
                    {
                        return OrdinalJoinSearch;
                    }
                default:
                    {
                        return DefaultSearch;
                    }
            }
        }

        /// <summary>
        /// Возвращает результаты поиска по умолчанию, без использования образцов.
        /// </summary>
        /// <param name="searchData">Объект с информацией для поиска</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="cansellationToken">Токен отмены выполнения</param>
        /// <returns>Нематериализованный запрос результатов поиска</returns>
        private IQueryable<T> DefaultSearch(ISearchData searchData,
                                            IDomainDbManager dbManager,
                                            CancellationToken cansellationToken)
        {
            var results = dbManager.GetDomainTable<T>().Where(_defaultSearchFilter);
            if (_defaultOrder != null)
            {
                if(this._defaultOrderDirection == OrderDirection.Ascending)
                {
                    results = results.OrderBy(_defaultOrder);
                }
                else
                {
                    results = results.OrderByDescending(_defaultOrder);
                }
            }
            return results.AsQueryable();
        }

        /// <summary>
        /// Возвращает результаты поиска по образцу того же типа, что и объекты поиска
        /// </summary>
        /// <param name="searchData">Объект с информацией для поиска</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="cansellationToken">Токен отмены выполнения</param>
        /// <returns>Нематериализованный запрос результатов поиска</returns>
        private IQueryable<T> SelfSearch(ISearchData searchData,
                                         IDomainDbManager dbManager,
                                         CancellationToken cansellationToken)
        {
            return _searcher.Search(searchData.SearchObject as T, dbManager);
        }

        /// <summary>
        /// Возвращает результаты поиска по соединяемой таблице доменных объектов.
        /// </summary>
        /// <param name="searchData">Объект с информацией для поиска</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="cansellationToken">Токен отмены выполнения</param>
        /// <returns>Нематериализованный запрос результатов поиска</returns>
        /// <exception cref="BLLException">Ошибка соединения выражений поиска. Нет подходящего выржения Join.</exception>
        private IQueryable<T> OrdinalJoinSearch(ISearchData searchData,
                                                IDomainDbManager dbManager,
                                                CancellationToken cansellationToken)
        {
            if (!_domainJoinDictionary.ContainsKey(searchData.SearchObject.GetType().BaseType)) 
                throw new BLLException("Ошибка соединения выражений поиска. Нет подходящего выржения Join.");

            var innerQuery = _searcher.Search(searchData.SearchObject, dbManager);
            var resQuery = dbManager.GetDomainTable<T>();
            return _domainJoinDictionary[searchData.SearchObject.GetType().BaseType](resQuery, innerQuery);
        }
    }
}
