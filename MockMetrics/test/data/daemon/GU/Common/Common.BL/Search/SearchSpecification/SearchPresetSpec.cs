using System;
using System.Collections.Generic;

using Common.DA.Interface;

namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Класс представляющий спецификацию пресета поиска сущностей типа T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PresetSpec<T> : SearchPresetSpec where T : IPersistentObject
    {

    }

    /// <summary>
    /// Класс представляющий спецификацию пресета поиска
    /// </summary>
    [Serializable]
    public abstract class SearchPresetSpec
    {
        /// <summary>
        /// Класс представляющий спецификацию пресета поиска
        /// </summary>
        protected SearchPresetSpec()
        {
            PresetSpecDets = new List<PresetSpecExpression>();
            OrderFieldList = new List<SearchOrdering>();
        }

        /// <summary>
        /// Список спеков условий
        /// </summary>
        public List<PresetSpecExpression> PresetSpecDets { get; set; }

        /// <summary>
        /// Возвращает или устанавливает список полей сортировки
        /// </summary>
        public List<SearchOrdering> OrderFieldList { get; set; } 
        
        /// <summary>
        /// Добавляет в пресет поле сортировки
        /// </summary>
        /// <param name="domainType">Доменный тип сущности хозяина</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="orderDirection">Напрвеление сортировки</param>
        public void AddOrderField(Type domainType, string propertyName, OrderDirection orderDirection)
        {
            OrderFieldList.Add(
                new SearchOrdering
                    {
                      DomainPropertyName = propertyName, 
                      OrderDirection = orderDirection,
                      DomainType = domainType
                    });
        }
    }

    /// <summary>
    /// Класс представляющий спецификацю условия 
    /// </summary>
    [Serializable]
    public class PresetSpecExpression
    {
        /// <summary>
        /// Класс представляющий спецификацю условия 
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="searchConditionType">Тип условия</param>
        /// <param name="domainType">Доменный тип, которому принадлежит свойство</param>
        public PresetSpecExpression(string propertyName, SearchConditionType searchConditionType, Type domainType)
        {
            PropertyName = propertyName;
            SearchConditionType = searchConditionType;
            DomainType = domainType;
        }

        /// <summary>
        /// Имя свойства
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Тип условия
        /// </summary>
        public SearchConditionType SearchConditionType { get; private set; }

        /// <summary>
        /// Доменный тип, которому принадлежит свойство
        /// </summary>
        public Type DomainType { get; private set; }

        #region Specific values

        public string StringValue { get; set; }

        public int NumberValue1 { get; set; }

        public int NumberValue2 { get; set; }

        public DateTime? DateTimeValue1 { get; set; }

        public DateTime? DateTimeValue2 { get; set; }

        public bool BoolValue { get; set; }

        public int DictionarySelectedValue { get; set; }

        #endregion
    }
}
