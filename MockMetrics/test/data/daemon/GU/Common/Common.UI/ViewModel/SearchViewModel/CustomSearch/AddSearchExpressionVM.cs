using System;
using System.Collections.Generic;
using System.Linq;

using Common.BL.Search.SearchSpecification;

using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.SearchViewModel.CustomSearch
{
    /// <summary>
    /// Класс ViewModel для диалога добавления условия в настраиваемый поиск доменных объектов
    /// </summary>
    public class AddSearchExpressionVM : NotificationObject
    {
        /// <summary>
        /// Класс ViewModel для диалога добавления условия в настраиваемый поиск доменных объектов
        /// </summary>
        /// <param name="searchTypes">Типы, по которым может производиться поиск</param>
        public AddSearchExpressionVM(List<Type> searchTypes)
        {
            DomainPropertyList = new List<SearchPropertySpec>();
            DomainEntityList = new Dictionary<Type, string>();
            foreach (var searchType in searchTypes)
            {
                var presetSpes = UIContainer.Instance.ResolveSearchPropertySpecs(searchType);

                if (!presetSpes.Any())
                {
                    throw null;
                }

                DomainEntityList.Add(searchType, presetSpes.First().DomainName);
            }
            SelectedDomainEntity = DomainEntityList.First().Key;
        }

        /// <summary>
        /// Фильтрует спецификации свойств по типу доменного объекта
        /// </summary>
        /// <param name="domainType">Тип доменного объекта</param>
        private void FilterProperties(Type domainType)
        {
            if (domainType != null)
            {
                DomainPropertyList = UIContainer.Instance.ResolveSearchPropertySpecs(domainType);
                SelectedDomainProperty = DomainPropertyList.First();
            }
        }

        /// <summary>
        /// Фильтрует спецификации условий по свойству доменного объекта
        /// </summary>
        /// <param name="searchPropertySpec"></param>
        private void FilterExpression(SearchPropertySpec searchPropertySpec)
        {
            if (searchPropertySpec != null)
            {
                ExpressionList = UIContainer.Instance
                                            .ResolveSearchExpressionSpec()
                                            .Where(t => t.SearchTypeSpec == searchPropertySpec.SearchTypeSpec)
                                            .ToList();
                SelectedExpression = ExpressionList.First();
            }
        }

        #region Binding Properties

        /// <summary>
        /// Словарь типов по которым может производиться поиск.
        /// </summary>
        private Dictionary<Type, string> _domainEntityList;

        /// <summary>
        /// Возвращает или устанавливает словарь типов по которым может производиться поиск.
        /// </summary>
        public Dictionary<Type, string> DomainEntityList
        {
            get
            {
                return _domainEntityList;
            }
            set
            {
                if (_domainEntityList != value)
                {
                    _domainEntityList = value;
                    RaisePropertyChanged(() => DomainEntityList);
                }
            }
        }

        /// <summary>
        /// Список спецификаций свойств, которые могут использоваться в условиях поиска.
        /// </summary>
        private List<SearchPropertySpec> _domainPropertyList;

        /// <summary>
        /// Возвращает или устанавливает cписок спецификаций свойств, которые могут использоваться в условиях поиска.
        /// </summary>
        public List<SearchPropertySpec> DomainPropertyList
        {
            get
            {
                return _domainPropertyList;
            }
            set
            {
                if (_domainPropertyList != value)
                {
                    _domainPropertyList = value;
                    RaisePropertyChanged(() => DomainPropertyList);
                }
            }
        }

        /// <summary>
        /// Список спецификаций условий, которые можно добавить в условия поиска.
        /// </summary>
        private List<SearchExpressionSpec> _expressionList;

        /// <summary>
        /// Возвращает или устанавливает cписок спецификаций условий, которые можно добавить в условия поиска.
        /// </summary>
        public List<SearchExpressionSpec> ExpressionList
        {
            get
            {
                return _expressionList;
            }
            set
            {
                if (_expressionList != value)
                {
                    _expressionList = value;
                    RaisePropertyChanged(() => ExpressionList);
                }
            }
        }

        /// <summary>
        /// Выбранный тип сущности для условия поиска.
        /// </summary>
        private Type _selectedDomainEntity;

        /// <summary>
        /// Возвращает или устанавливает выбранный тип сущности для условия поиска.
        /// </summary>
        public Type SelectedDomainEntity
        {
            get
            {
                return _selectedDomainEntity;
            }
            set
            {
                if (_selectedDomainEntity != value)
                {
                    _selectedDomainEntity = value;
                    FilterProperties(_selectedDomainEntity);
                    RaisePropertyChanged(() => SelectedDomainEntity);
                }
            }
        }

        /// <summary>
        /// Выбранное свойство для условия поиска.
        /// </summary>
        private SearchPropertySpec _selectedDomainProperty;

        /// <summary>
        /// Возвращает или устанавливает выбранное свойство для условия поиска.
        /// </summary>
        public SearchPropertySpec SelectedDomainProperty
        {
            get
            {
                return _selectedDomainProperty;
            }
            set
            {
                if (_selectedDomainProperty != value)
                {
                    _selectedDomainProperty = value;
                    FilterExpression(_selectedDomainProperty);
                    RaisePropertyChanged(() => SelectedDomainProperty);
                }
            }
        }

        /// <summary>
        /// Выбранное условие для поиска.
        /// </summary>
        private SearchExpressionSpec _selectedExpression;

        /// <summary>
        /// Возвращает или устанавливает выбранное условие для поиска.
        /// </summary>
        public SearchExpressionSpec SelectedExpression
        {
            get
            {
                return _selectedExpression;
            }
            set
            {
                if (_selectedExpression != value)
                {
                    _selectedExpression = value;
                    RaisePropertyChanged(() => SelectedExpression);
                }
            }
        }

        #endregion
    }
}
