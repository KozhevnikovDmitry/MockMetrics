using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

using Common.BL.Search.SearchSpecification;
using Common.DA.Interface;
using Common.Types;
using Common.Types.Exceptions;

namespace Common.BL.Search.SearchModification
{
    /// <summary>
    /// Класс представляющий модификацию для настраиваемого поиска доменных объектов
    /// </summary>
    /// <typeparam name="T">Тип доменных объектов</typeparam>
    public class CustomSearchModification<T> : ISearchModification<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Тип доменных объектов
        /// </summary>
        /// <param name="domainJoinDictionary">Словарь делегатов соединения доменных таблиц</param>
        /// <param name="domainDoubleJoinDictionary"></param>
        public CustomSearchModification(Dictionary<Type, DomainJoinDelegate<T>> domainJoinDictionary)
        {
            _domainJoinDictionary = domainJoinDictionary;
        }

        /// <summary>
        /// Словарь делегатов соединения доменных таблиц.
        /// </summary>
        private readonly Dictionary<Type, DomainJoinDelegate<T>> _domainJoinDictionary;

        /// <summary>
        /// Возвращает делегат на метод поиска доменных объектов
        /// </summary>
        /// <param name="searchData">Объект с информацией для поиска</param>
        /// <returns>Делегат метода поиска</returns>
        public SearchDelegate<T> GetSearchActionDelegate(ISearchData searchData)
        {
            SearchDelegate<T> resultDelegate = (st, db, ct) =>
            {
                var res = db.GetDomainTable<T>();

                var groupedExpressions = searchData.SearchPreset.SearchExpressionList.GroupBy(
                    t => t.SearchPropertySpec.DomainType);

                foreach (var groupedExpr in groupedExpressions)
                {
                    Type exprDomainType = groupedExpr.Key;
                    if (exprDomainType == null) throw new BLLException("Ошибка получения выражения поиска");

                    if (exprDomainType != typeof(T))
                    {
                        if (!_domainJoinDictionary.ContainsKey(exprDomainType))
                            throw new BLLException("Ошибка соединения выражений поиска. Нет подходящего выражения Join.");

                        IQueryable innerQuery = db.GetDomainTable(exprDomainType);
                        innerQuery = groupedExpr.Aggregate(innerQuery, ApplyExpression);
                        res = _domainJoinDictionary[exprDomainType](res, innerQuery);
                    }
                    else
                    {
                        IQueryable result = res;
                        result = groupedExpr.Aggregate(result, ApplyExpression);
                        res = (IQueryable<T>)result;
                    }
                }

                res = this.ApplyOrdering(res, searchData.SearchPreset.OrderFieldList);

                return res.Distinct();
            };
            return resultDelegate;
        }

        /// <summary>
        /// Добавляет условие к запросу поиска.
        /// </summary>
        /// <param name="query">Запрос поиска</param>
        /// <param name="expression">Условие поиска</param>
        /// <returns>Запрос с условием</returns>
        private IQueryable ApplyExpression(IQueryable query, SearchExpression expression)
        {
            string pattern = string.Format(expression.SearchExpressionSpec.Pattern, expression.PropertyName);
            switch (expression.SearchExpressionSpec.SearchTypeSpec)
            {
                case SearchTypeSpec.String:
                    {
                        return !string.IsNullOrEmpty(expression.StringValue) ? query.Where(pattern, expression.StringValue) : query;
                    }
                case SearchTypeSpec.Bool:
                    {
                        return query.Where(pattern, expression.BoolValue);
                    }
                case SearchTypeSpec.Number:
                    {
                        if (expression.SearchExpressionSpec.ExpressionQuantity == ExpressionQuantity.Single)
                        {
                            return query.Where(pattern, expression.NumberValue1);
                        }
                        else
                        {
                            return query.Where(pattern, expression.NumberValue1, expression.NumberValue2);
                        }
                    }
                case SearchTypeSpec.Date:
                    {
                        if (expression.SearchExpressionSpec.ExpressionQuantity == ExpressionQuantity.Single)
                        {
                            return expression.DateTimeValue1.HasValue ? query.Where(pattern, expression.DateTimeValue1) : query;
                        }
                        else
                        {
                            DateTime? date1 = expression.DateTimeValue1.HasValue ? expression.DateTimeValue1.Value : DateTime.MinValue;
                            DateTime? date2 = expression.DateTimeValue2.HasValue ? expression.DateTimeValue2.Value : DateTime.MaxValue;
                            return query.Where(pattern, date1, date2);
                        }
                    }
                case SearchTypeSpec.Dictionary:
                    {
                        return query.Where(pattern, expression.DictionarySelectedValue);
                    }
                case SearchTypeSpec.Enum:
                    {
                        var enumValue = Enum.Parse(expression.SearchPropertySpec.DomainPropertyType,
                                                   expression.DictionarySelectedValue.ToString());
                        return query.Where(pattern, enumValue);
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        /// <summary>
        /// Добавлеяет выражения сортировки в запрос поиска
        /// </summary>
        /// <param name="query">Неотсортированный запрос поиска</param>
        /// <param name="orderFieldList">Список полей сортировки</param>
        /// <returns>Отсортированный запрос</returns>
        private IQueryable<T> ApplyOrdering(IQueryable<T> query, List<SearchOrdering> orderFieldList)
        {
            foreach (var orderField in orderFieldList)
            {
                // TODO : Проверить что это работает! Нужны join'ы
                string expr = typeof(T).Name == orderField.DomainType.Name
                                  ? string.Format("{0}", orderField.DomainPropertyName)
                                  : string.Format("{0}.{1}", orderField.DomainType.Name, orderField.DomainPropertyName);

                if (orderFieldList.First() == orderField)
                {
                    query = orderField.OrderDirection == OrderDirection.Ascending
                                ? query.OrderBy(expr)
                                : query.OrderByDescending(expr);
                }

                query = orderField.OrderDirection == OrderDirection.Ascending
                            ? (query as IOrderedQueryable<T>).ThenBy(expr)
                            : (query as IOrderedQueryable<T>).ThenByDescending(expr);
            }
            return query;
        }
    }
}
