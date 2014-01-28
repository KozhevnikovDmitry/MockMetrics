using System;
using System.Collections.Generic;
using System.Linq;

using Common.BL.Search.SearchSpecification;
using Common.Types.Exceptions;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.SearchViewModel.CustomSearch
{
    /// <summary>
    /// ����� ViewModel ���������������� ��� ����������� ��������� ����������
    /// </summary>
    public class OrderingVM : NotificationObject
    {
        /// <summary>
        /// ������������ ��������� ����������
        /// </summary>
        public SearchOrdering SearchOrdering { get; set; }

        /// <summary>
        /// ����� ViewModel ���������������� ��� ����������� ��������� ����������
        /// </summary>
        /// <param name="ordering">���������� ������</param>
        public OrderingVM(SearchOrdering ordering)
        {
            SearchOrdering = ordering;
            RemoveOrderingCommand = new DelegateCommand(this.RemoveOrdering);
            this.SetOrderPropertyString();
        }
        
        /// <summary>
        /// �������� ������ � ����������� � �������� ����������
        /// </summary>
        private void SetOrderPropertyString()
        {
            var searchPropertySpec = UIContainer.Instance
                                                .ResolveSearchPropertySpecs(SearchOrdering.DomainType)
                                                .Single(t => t.DomainPropertyName == SearchOrdering.DomainPropertyName);

            OrderProprtyString = string.Format(@"{0} / {1}", searchPropertySpec.DomainName, searchPropertySpec.DisplayName);
        }

        #region Events

        /// <summary>
        /// ������� ����������� � ������� �� �������� ����������
        /// </summary>
        public event Action<OrderingVM> OnRemoveOrderingRequested;

        #endregion

        #region Binding Properties

        /// <summary>
        /// ������ � ����������� � ���� ����������
        /// </summary>
        public string OrderProprtyString { get; private set; }

        /// <summary>
        /// ���� ����������� �� ����������������� ����������
        /// </summary>
        public bool IsDescending
        {
            get
            {
                return SearchOrdering.OrderDirection == OrderDirection.Descending;
            }
            set
            {
                SearchOrdering.OrderDirection = value ? OrderDirection.Descending : OrderDirection.Ascending;
                RaisePropertyChanged(() => IsDescending);
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// ������� �������� ��������� ����������
        /// </summary>
        public DelegateCommand RemoveOrderingCommand { get; private set; }

        /// <summary>
        /// �������� ������ �� �������� ���������� �� �������
        /// </summary>
        private void RemoveOrdering()
        {
            try
            {
                if (OnRemoveOrderingRequested != null)
                {
                    OnRemoveOrderingRequested(this);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("�������������� ������", ex));
            }
        }

        #endregion
    }
}