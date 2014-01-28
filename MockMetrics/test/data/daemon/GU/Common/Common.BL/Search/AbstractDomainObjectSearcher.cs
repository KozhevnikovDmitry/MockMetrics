using System;
using System.Collections.Generic;
using System.Linq;

using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL.Search
{
    /// <summary>
    /// Базовый класс для классов, предназначенных для поиска доменных объектов.
    /// </summary>
    public abstract class AbstractDomainObjectSearcher : IDomainObjectSearcher
    {
        /// <summary>
        /// Словарь методов поиска доменных объектов.
        /// </summary>
        /// <remarks>
        /// Ключом в словаре является доменный тип, а значениями делегаты на методы поиска.
        /// </remarks>
        protected readonly Dictionary<string, Func<IDomainObject, IDomainDbManager, IQueryable<IDomainObject>>> _searchActions = 
            new Dictionary<string, Func<IDomainObject, IDomainDbManager, IQueryable<IDomainObject>>>();

        /// <summary>
        /// Список замерженных searcher'ов
        /// </summary>
        private readonly List<IDomainObjectSearcher> _mergedSearchers = new List<IDomainObjectSearcher>();

        /// <summary>
        /// Вовзвращает запрос поиска доменных объектов по данным шаблонного объекта.
        /// </summary>
        /// <typeparam name="T">Доменный типа</typeparam>
        /// <param name="templateDomainObject">Объект - шаблон для поиска</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Запрос поиска доменных объектов</returns>
        /// <exception cref="BLLException">Метод поиска не обнаружен</exception>
        public IQueryable<T> Search<T>(T templateDomainObject, IDomainDbManager dbManager) where T : IDomainObject
        {
            string domainTypeName = templateDomainObject.GetType().Name;
            if (_searchActions.ContainsKey(domainTypeName))
            {
                return (IQueryable<T>)_searchActions[domainTypeName](templateDomainObject, dbManager);
            }
            else
            {
                foreach (var searcher in _mergedSearchers)
                {
                    try
                    {
                        return searcher.Search(templateDomainObject, dbManager);
                    }
                    catch
                    {

                    }
                }
                throw new BLLException("Метод поиска не обнаружен");
            }
        }

        /// <summary>
        /// Мержит сторонний searcher в данный.
        /// </summary>
        /// <param name="searcher">Searcher для мержа</param>
        public void Merge(IDomainObjectSearcher searcher)
        {
            _mergedSearchers.Add(searcher);
        }
    }
}
