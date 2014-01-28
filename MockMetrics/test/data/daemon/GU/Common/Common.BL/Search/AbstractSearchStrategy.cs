using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search.SearchModification;
using Common.BL.Search.SearchSpecification;
using Common.DA.Interface;

namespace Common.BL.Search
{
    /// <summary>
    /// Класс абстрактная стратегия настраиваемого поиска доменных объектов.
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public abstract class AbstractSearchStrategy<T> : DomainDependent, ISearchStrategy<T> where T : class, IPersistentObject
    {
        private readonly IDomainDataMapper<T> _dataMapper;

        /// <summary>
        /// Класс абстрактная стратегия настраиваемого поиска доменных объектов.
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        protected AbstractSearchStrategy(IDomainContext domainContext, IDomainDataMapper<T> dataMapper)
            : base(domainContext)
        {
            _dataMapper = dataMapper;
            CancelSource = new CancellationTokenSource();
            _searchShceduler = TaskScheduler.FromCurrentSynchronizationContext();
            DomainJoinDictionary = new Dictionary<Type, DomainJoinDelegate<T>>();
        }
        
        /// <summary>
        /// Выражение для фильтрации шаблонного поиска по умолчанию.
        /// </summary>
        protected Expression<Func<T, bool>> _defaultSearchFilter = t => true;

        /// <summary>
        /// Выражение для сортировки шаблонного поиска по умолчанию.
        /// </summary>
        protected Expression<Func<T, object>> _defaultOrder;

        /// <summary>
        /// Направление сортировки шаблонного поиска по умолчанию.
        /// </summary>
        protected OrderDirection _defaultOrderDirection;

        /// <summary>
        /// Возвращает источник токенов отмены выполнения
        /// </summary>
        public CancellationTokenSource CancelSource { get; private set; }

        /// <summary>
        /// Планировщик выполнения задач <c>Task</c>.
        /// </summary>
        private readonly TaskScheduler _searchShceduler;

        #region Search Specification 
        
        /// <summary>
        /// Объект с информацией для поиска
        /// </summary>
        private ISearchData _searchData;

        /// <summary>
        /// Количество результатов поиска
        /// </summary>
        private int _resultCount;

        /// <summary>
        /// Словарь делегатов выполняющих соединение различных доменных таблиц с таблицей типа T.
        /// </summary>
        protected readonly Dictionary<Type, DomainJoinDelegate<T>> DomainJoinDictionary;

        /// <summary>
        /// Режим поиска.
        /// </summary>
        private SearchMode _searchMode;

        /// <summary>
        /// Вовзращает или устанавливает режим поиска
        /// </summary>
        public SearchMode SearchMode
        {
            get
            {
                return _searchMode;
            }
            set
            {
                _searchMode = value;
                if (_searchMode == SearchMode.Templated)
                {
                    _searchModifcation = new TemplatedSearchModification<T>(DomainJoinDictionary, _searcher, _defaultSearchFilter, _defaultOrder, _defaultOrderDirection);
                }
                else
                {
                    _searchModifcation = new CustomSearchModification<T>(DomainJoinDictionary);
                }
            }
        }

        /// <summary>
        /// Модификация поиска доменных объектов.
        /// </summary>
        private ISearchModification<T> _searchModifcation;

        /// <summary>
        /// Объект занимающийся поиском по шаблону.
        /// </summary>
        private IDomainObjectSearcher _searcher;

        #endregion

        #region Search Processing

        /// <summary>
        /// Осуществляет поиск доменных объектов по заданной спецификации
        /// </summary>
        /// <param name="searchData">Объект с информацией для поиска</param>
        public void Search(ISearchData searchData)
        {
            var cancelToken = CancelSource.Token;

            // Получаем делегат на метод поиска
            SearchDelegate<T> searchAction = (sd, db, ct) =>
            {
                var results = GetSearchAction(searchData)(sd, db, ct);
                CheckCancelationRequsted(ct);
                return results;
            };

            // Создаём задачу для поиска передаём в задачу вызов делегата поиска.
            var searchTask =
                new Task<SearchResultReadyEventArgs<T>>(() =>
                {
                    try
                    {
                        using (var dbManager = this.GetDbManager())
                        {
                            _searchData = searchData;
                            var query = searchAction(searchData, dbManager, cancelToken);
                            _resultCount = query.Count();
                            int actualPosition;
                            var results = PageResults(query, searchData.PageStartPosition, searchData.PageSize, out actualPosition, cancelToken);
                            this.FillResultsAssociations(results, dbManager, cancelToken);
                            return new SearchResultReadyEventArgs<T>(results, _resultCount, actualPosition);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        return new SearchResultReadyEventArgs<T>(new List<T>(), 0, 0);
                    }
                    catch (Exception ex)
                    {
                        return new SearchResultReadyEventArgs<T>(ex);
                    }
                }, cancelToken);

            // Задаём продолжение для задачи поиска, которое будет выполняться в главном потоке.
            // Для этого передаём _searchShceduler
            searchTask.ContinueWith(t =>
            {
                if (SearchResultReady != null)
                {
                    SearchResultReady(t.Result);
                }
            }, _searchShceduler);

            // Запускаем задачу на выполнения
            searchTask.Start();
        }

        /// <summary>
        /// Осуществляет поиск страницы с заданной позиции.
        /// </summary>
        /// <param name="position">Начальная позиция страницы поиска</param>
        public void SearchPage(int position)
        {
            if (_searchData != null)
            {
                _searchData.PrepareReuse(position);
                Search(_searchData);
            }  
        }

        /// <summary>
        /// Вовзращает true, если возможно получить другую страницу поиска.
        /// </summary>
        /// <returns>Флаг возможности полчения другой страницы поиска</returns>
        public bool CanGetAnotherPage()
        {
            if (_searchData != null &&
                _resultCount != 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Возращает делегат на метод поиска.
        /// </summary>
        /// <param name="searchData">Объект с информацией для поиска</param>
        /// <returns>Делегат на метод поиска</returns>
        protected virtual SearchDelegate<T> GetSearchAction(ISearchData searchData)
        {
            return _searchModifcation.GetSearchActionDelegate(searchData);
        }

        /// <summary>
        /// Возвращает список объектов <c>IDomainObject</c>, представляющий возвращаемую страницу результатов поиска.
        /// </summary>
        /// <param name="query">Последовательность результатов поиска</param>
        /// <param name="position">Позиция начала возвращаемой страницы результатов</param>
        /// <param name="amount">Количество результатов на возвращаемой странице</param>
        /// <param name="actualPosition">Реальная позиция начала возврщаемой страниы поиска</param>
        /// <param name="cansellationToken">Токен отмены выполнения</param>
        /// <returns>Возвращаемая страница результатов поиска</returns>
        private List<T> PageResults(IQueryable<T> query,
                                    int position,
                                    int amount,
                                    out int actualPosition,
                                    CancellationToken cansellationToken)
        {
            List<T> results;
            if (query == null)
            {
                actualPosition = 0;
                return null;
            }

            int count = query.Count();
            if (count >= (position + amount))
            {
                actualPosition = position;
                results = query.Skip(position).Take(amount).ToList();
            }
            else
            {
                if (count > amount)
                {
                    actualPosition = position;
                    results = query.Skip(position).ToList();
                }
                else
                {
                    actualPosition = 0;
                    results = query.ToList();
                }
            }
            CheckCancelationRequsted(cansellationToken);
            return results;
        }

        /// <summary>
        /// Заполняет отображаемые ассоциации доменных объектов результатов поиска.
        /// </summary>
        /// <param name="resultPage">Страница результатов поиска</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="cancelToken">Токен отмены выполнения</param>
        private void FillResultsAssociations(List<T> resultPage, IDomainDbManager dbManager, CancellationToken cancelToken)
        {
            foreach (var res in resultPage)
            {
                _dataMapper.FillAssociations(res, dbManager);
                CheckCancelationRequsted(cancelToken);
            }
        }        

        public IDomainObjectSearcher Searcher
        {
            set
            {
                _searcher = value;
            }
        }

        #endregion

        #region Search Response

        /// <summary>
        /// Событие, оповещающее завершении поиска.
        /// </summary>
        public event SearchResultReadyDelegate<T> SearchResultReady;

        /// <summary>
        /// Проверяет наличие запроса на отмену операции. Выбрасывает исключение  <c>OperationCanceledException</c>.
        /// </summary>
        /// <param name="cansellationToken">Токен отмены операции</param>
        /// <exception cref="OperationCanceledException">Операция была отменена</exception>
        private void CheckCancelationRequsted(CancellationToken cansellationToken)
        {
            if (cansellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(cansellationToken);
            }
        }
        
        #endregion
    }
}
