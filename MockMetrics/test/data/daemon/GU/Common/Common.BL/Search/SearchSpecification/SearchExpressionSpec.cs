using System;
using Common.Types;

namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Класс предназначенный для описания условия поиска по свойству доменного объекта.
    /// </summary>
    public class SearchExpressionSpec
    {
        /// <summary>
        /// Класс предназначенный для описания условия поиска по свойству доменного объекта.
        /// </summary>
        /// <param name="name">Имя условия</param>
        /// <param name="pattern">Строковый паттерн выражения поиска</param>
        /// <param name="searchTypeSpec">Упрощённый тип поля</param>
        /// <param name="expressionQuantity">Вместимость условия поиска</param>
        public SearchExpressionSpec(string name, 
                                    string pattern, 
                                    SearchTypeSpec searchTypeSpec, 
                                    SearchConditionType searchConditionType,
                                    ExpressionQuantity expressionQuantity)
        {
            Name = name;
            Pattern = pattern;
            SearchTypeSpec = searchTypeSpec;
            SearchConditionType = searchConditionType;
            ExpressionQuantity = expressionQuantity;
        }

        /// <summary>
        /// Упрощённый тип свойства поиска.
        /// </summary>
        public SearchTypeSpec SearchTypeSpec { get; private set; }

        /// <summary>
        /// Тип условия поиска
        /// </summary>
        public SearchConditionType SearchConditionType { get; private set; }

        /// <summary>
        /// Вместимость условия.
        /// </summary>
        public ExpressionQuantity ExpressionQuantity { get; private set; }

        /// <summary>
        /// Возвращает имя условия.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Возвращает строковый паттерн выражения поиска.
        /// </summary>
        public string Pattern { get; private set; }
    }
}
