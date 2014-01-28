using System;
using System.Collections.Generic;
using System.Linq;

using Common.BL.Search.SearchSpecification;

using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.SearchViewModel.CustomSearch
{
    /// <summary>
    /// Класс ViewModel преденазначенный для отоюражения выражения сортировки
    /// </summary>
    public class AddSearchOrderingVM : NotificationObject
    {
        /// <summary>
        /// Отображаемое выражение сортировки
        /// </summary>
        public SearchOrdering SearchOrdering { get; private set; }

        /// <summary>
        /// Класс ViewModel преденазначенный для отоюражения выражения сортировки
        /// </summary>
        /// <param name="searchTypes">Список доменных типов, свойства которых могут участвовать в выражениях сортировки</param>
        public AddSearchOrderingVM(List<Type> searchTypes)
        {
            this.SearchOrdering = CreateNewOrdering(searchTypes.First());
            DomainPropertyList = new List<SearchPropertySpec>();
            DomainEntityList = new Dictionary<Type, string>();
            foreach (var searchType in searchTypes)
            {
                DomainEntityList.Add(searchType, UIContainer.Instance.ResolveSearchPropertySpecs(searchType).First().DomainName);
            }
            SelectedDomainEntity = DomainEntityList.First().Key;
            this.OrderDirectionList = new Dictionary<OrderDirection, string>();
            this.OrderDirectionList[OrderDirection.Ascending] = "Прямая";
            this.OrderDirectionList[OrderDirection.Descending] = "Обратная";
            this.SelectedOrderDirection = OrderDirection.Ascending;
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
        /// Возвращает дефолтное выражение сортировки для доменного типа.
        /// </summary>
        /// <param name="domainType">Доменны тип</param>
        /// <returns>Выражение сортировки</returns>
        private SearchOrdering CreateNewOrdering(Type domainType)
        {
            var propertySpec = UIContainer.Instance.ResolveSearchPropertySpecs(domainType).First();
            return new SearchOrdering
            {
                OrderDirection = OrderDirection.Ascending,
                DomainPropertyName = propertySpec.DomainPropertyName,
                DomainType = propertySpec.DomainType
            };
        }

        #region Binding Properties

        /// <summary>
        /// Словарь типов по которым может производиться сортировка.
        /// </summary>
        private Dictionary<Type, string> _domainEntityList;

        /// <summary>
        /// Возвращает или устанавливает словарь типов по которым может производиться сортировка.
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
        /// Список спецификаций свойств, которые могут использоваться в выражениях сортировки.
        /// </summary>
        private List<SearchPropertySpec> _domainPropertyList;

        /// <summary>
        /// Возвращает или устанавливает cписок спецификаций свойств, которые могут использоваться в выражениях сортировки.
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
                    SearchOrdering.DomainType = _selectedDomainProperty.DomainType;
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
                    SearchOrdering.DomainPropertyName = _selectedDomainProperty.DomainPropertyName;
                    RaisePropertyChanged(() => SelectedDomainProperty);
                }
            }
        }

        /// <summary>
        /// Словарь направлений сортировки
        /// </summary>
        public Dictionary<OrderDirection, string> OrderDirectionList { get; set; }
        
        /// <summary>
        /// Возвращает напрaвление сортировки
        /// </summary>
        public OrderDirection SelectedOrderDirection
        {
            get
            {
                return this.SearchOrdering.OrderDirection;
            }
            set
            {
                if (this.SearchOrdering.OrderDirection != value)
                {
                    this.SearchOrdering.OrderDirection = value;
                    RaisePropertyChanged(() => this.SearchOrdering.OrderDirection);
                }
            }
        }

        #endregion
    }
}