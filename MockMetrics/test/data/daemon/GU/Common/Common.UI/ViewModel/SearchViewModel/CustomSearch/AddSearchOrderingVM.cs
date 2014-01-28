using System;
using System.Collections.Generic;
using System.Linq;

using Common.BL.Search.SearchSpecification;

using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.SearchViewModel.CustomSearch
{
    /// <summary>
    /// ����� ViewModel ���������������� ��� ����������� ��������� ����������
    /// </summary>
    public class AddSearchOrderingVM : NotificationObject
    {
        /// <summary>
        /// ������������ ��������� ����������
        /// </summary>
        public SearchOrdering SearchOrdering { get; private set; }

        /// <summary>
        /// ����� ViewModel ���������������� ��� ����������� ��������� ����������
        /// </summary>
        /// <param name="searchTypes">������ �������� �����, �������� ������� ����� ����������� � ���������� ����������</param>
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
            this.OrderDirectionList[OrderDirection.Ascending] = "������";
            this.OrderDirectionList[OrderDirection.Descending] = "��������";
            this.SelectedOrderDirection = OrderDirection.Ascending;
        }

        /// <summary>
        /// ��������� ������������ ������� �� ���� ��������� �������
        /// </summary>
        /// <param name="domainType">��� ��������� �������</param>
        private void FilterProperties(Type domainType)
        {
            if (domainType != null)
            {
                DomainPropertyList = UIContainer.Instance.ResolveSearchPropertySpecs(domainType);
                SelectedDomainProperty = DomainPropertyList.First();
            }
        }

        /// <summary>
        /// ���������� ��������� ��������� ���������� ��� ��������� ����.
        /// </summary>
        /// <param name="domainType">������� ���</param>
        /// <returns>��������� ����������</returns>
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
        /// ������� ����� �� ������� ����� ������������� ����������.
        /// </summary>
        private Dictionary<Type, string> _domainEntityList;

        /// <summary>
        /// ���������� ��� ������������� ������� ����� �� ������� ����� ������������� ����������.
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
        /// ������ ������������ �������, ������� ����� �������������� � ���������� ����������.
        /// </summary>
        private List<SearchPropertySpec> _domainPropertyList;

        /// <summary>
        /// ���������� ��� ������������� c����� ������������ �������, ������� ����� �������������� � ���������� ����������.
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
        /// ��������� ��� �������� ��� ������� ������.
        /// </summary>
        private Type _selectedDomainEntity;

        /// <summary>
        /// ���������� ��� ������������� ��������� ��� �������� ��� ������� ������.
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
        /// ��������� �������� ��� ������� ������.
        /// </summary>
        private SearchPropertySpec _selectedDomainProperty;
        
        /// <summary>
        /// ���������� ��� ������������� ��������� �������� ��� ������� ������.
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
        /// ������� ����������� ����������
        /// </summary>
        public Dictionary<OrderDirection, string> OrderDirectionList { get; set; }
        
        /// <summary>
        /// ���������� ����a������ ����������
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