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
    /// Класс ViewModel преденазначенный для отображения выражения сортировки
    /// </summary>
    public class OrderingVM : NotificationObject
    {
        /// <summary>
        /// Отображаемое выражение сортировки
        /// </summary>
        public SearchOrdering SearchOrdering { get; set; }

        /// <summary>
        /// Класс ViewModel преденазначенный для отоюражения выражения сортировки
        /// </summary>
        /// <param name="ordering">Сортировка поиска</param>
        public OrderingVM(SearchOrdering ordering)
        {
            SearchOrdering = ordering;
            RemoveOrderingCommand = new DelegateCommand(this.RemoveOrdering);
            this.SetOrderPropertyString();
        }
        
        /// <summary>
        /// Собирает строку с информацией о свойстве сортировки
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
        /// Событие оповещающие о запросе на удаление сортировки
        /// </summary>
        public event Action<OrderingVM> OnRemoveOrderingRequested;

        #endregion

        #region Binding Properties

        /// <summary>
        /// Строка с информацией о поле сортировки
        /// </summary>
        public string OrderProprtyString { get; private set; }

        /// <summary>
        /// Флаг указывающий на инвертированность сортировки
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
        /// Команда удаления выражения сортировки
        /// </summary>
        public DelegateCommand RemoveOrderingCommand { get; private set; }

        /// <summary>
        /// Посылает запрос на удаление сортировки из пресета
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
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #endregion
    }
}