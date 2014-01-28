using System.Collections.Generic;

using Common.DA.Interface;

using System.Linq;

using Common.Types.Exceptions;

namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Класс представляющий предварительную настройку поиска объектов типа T
    /// </summary>
    public class SearchPreset<T> : SearchPreset where T : IPersistentObject
    {
        private readonly SearchSpecificationContainer _searchSpecificationContainer;

        public SearchPreset(SearchSpecificationContainer searchSpecificationContainer)
        {
            this._searchSpecificationContainer = searchSpecificationContainer;
        }

        /// <summary>
        /// Добавляет в пресет новое условие
        /// </summary>
        /// <param name="presetSpecdet">Спецификация условия</param>
        public override void AddExpression(PresetSpecExpression presetSpecdet)
        {
            var propertySpec =
                this._searchSpecificationContainer.ResolveSearchPropertySpecs(presetSpecdet.DomainType).SingleOrDefault(
                    t => t.DomainPropertyName == presetSpecdet.PropertyName);

            if (propertySpec == null)
                throw new BLLException(string.Format("Свойство {0} типа {1} не существует, либо не может участвовать в поиске", presetSpecdet.PropertyName, typeof(T).Name));

            var expressionSpec =
                this._searchSpecificationContainer.ExpressionList.SingleOrDefault(
                    t => t.SearchTypeSpec == propertySpec.SearchTypeSpec && t.SearchConditionType == presetSpecdet.SearchConditionType);

            if (expressionSpec == null)
                throw new BLLException(string.Format("Не задана спецификация условия для типа {0} по свойству типа {1}", propertySpec.SearchTypeSpec, presetSpecdet.SearchConditionType));

            var expression = new SearchExpression { SearchExpressionSpec = expressionSpec, SearchPropertySpec = propertySpec };

            expression.StringValue = presetSpecdet.StringValue;
            expression.NumberValue1 = presetSpecdet.NumberValue1;
            expression.NumberValue2 = presetSpecdet.NumberValue2;
            expression.DateTimeValue1 = presetSpecdet.DateTimeValue1;
            expression.DateTimeValue2 = presetSpecdet.DateTimeValue2;
            expression.DictionarySelectedValue = presetSpecdet.DictionarySelectedValue;

            SearchExpressionList.Add(expression);
        }
    }

    /// <summary>
    /// Класс представляющий предварительную настройку поиска доменных объектов.
    /// </summary>
    public abstract class SearchPreset
    {
        /// <summary>
        /// Класс представляющий предварительную настройку поиска доменных объектов.
        /// </summary>
        public SearchPreset()
        {
            SearchExpressionList = new List<SearchExpression>();
            OrderFieldList = new List<SearchOrdering>();
        }

        /// <summary>
        /// Возвращает или устанавливает Id пресета.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Возвращает или устанавливает имя пресета.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возвращает или устанавливает список условий поиска.
        /// </summary>
        public List<SearchExpression> SearchExpressionList { get; set; }

        /// <summary>
        /// Возвращает или устанавливает список полей сортировки
        /// </summary>
        public List<SearchOrdering> OrderFieldList { get; set; } 

        /// <summary>
        /// Добавляет в пресет новое условие
        /// </summary>
        /// <param name="propertySpec">Спецификация свойства условия</param>
        /// <param name="expressionSpec">Спецификация условия</param>
        public void AddExpression(SearchPropertySpec propertySpec, SearchExpressionSpec expressionSpec)
        {
            SearchExpressionList.Add(new SearchExpression { SearchExpressionSpec = expressionSpec, SearchPropertySpec = propertySpec });
        }

        /// <summary>
        /// Добавляет в пресет поле сортировки
        /// </summary>
        /// <param name="ordering">Выражение сортировки</param>
        public void AddOrderField(SearchOrdering ordering)
        {
            OrderFieldList.Add(ordering);
        }

        /// <summary>
        /// Добавляет в пресет новое условие
        /// </summary>
        /// <param name="presetSpecdet">Спецификация условия</param>
        public abstract void AddExpression(PresetSpecExpression presetSpecdet);
    }
}
