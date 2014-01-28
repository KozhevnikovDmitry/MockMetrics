using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Common.BL.Search.SearchSpecification;
using Common.Types.Exceptions;
using Common.UI.View.SearchView;
using Common.UI.ViewModel.Interfaces;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.SearchViewModel.CustomSearch
{
    /// <summary>
    /// Класс ViewModel для проведения настройки кастомного поиска.
    /// </summary>
    public class SearchPresetVM : NotificationObject, ISearchPresetVM
    {
        /// <summary>
        /// Пресет настраиваемого поиска
        /// </summary>
        private readonly SearchPreset _searchPreset;

        /// <summary>
        /// Список типов по которым может производиться поиск
        /// </summary>
        private readonly List<Type> _searchTypes;
        
        /// <summary>
        /// Класс ViewModel для проведения настройки кастомного поиска.
        /// </summary>
        /// <param name="searchPreset">Пресет настраиваемого поиска</param>
        /// <param name="searchTypes">Список типов по которым может производиться поиск</param>
        public SearchPresetVM(SearchPreset searchPreset, List<Type> searchTypes)
        {
            _searchPreset = searchPreset;
            _searchTypes = searchTypes;
            AddExpressionCommand = new DelegateCommand(AddExpression);
            AddOrderingCommand = new DelegateCommand(this.AddOrdering);
            ExpressionVMs = new ObservableCollection<ExpressionVM>();
            OrderingVMs = new ObservableCollection<OrderingVM>();

            foreach (var searchExpression in _searchPreset.SearchExpressionList)
            {
                var item = new ExpressionVM(searchExpression);
                item.OnRemoveExpressionRequsted += OnRemoveExpressionRequsted;
                ExpressionVMs.Add(item);
            }

            foreach (var orderField in _searchPreset.OrderFieldList)
            {
                var item = new OrderingVM(orderField);
                item.OnRemoveOrderingRequested += this.OnRemoveOrderingRequsted;
                OrderingVMs.Add(item);
            }
        }

        #region Binding Properties

        /// <summary>
        /// Коллекция VM'ов с условиями поиска
        /// </summary>
        public ObservableCollection<ExpressionVM> ExpressionVMs { get; set; }

        /// <summary>
        /// Коллекция VM'ов с выражениями сортировки
        /// </summary>
        public ObservableCollection<OrderingVM> OrderingVMs { get; set; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда добавления условия поиска.
        /// </summary>
        public DelegateCommand AddExpressionCommand { get; protected set; }

        /// <summary>
        /// Добавляет условаие поиска
        /// </summary>
        private void AddExpression()
        {
            try
            {
                var vm = new AddSearchExpressionVM(_searchTypes);
                var result = UIFacade.ShowDialogView(new AddSearchExpressionView(), vm, "Добавить условие");
                if(result)
                {
                    _searchPreset.AddExpression(vm.SelectedDomainProperty, vm.SelectedExpression);
                    var item = new ExpressionVM(_searchPreset.SearchExpressionList.Last());
                    item.OnRemoveExpressionRequsted += OnRemoveExpressionRequsted;
                    ExpressionVMs.Add(item);
                }
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        /// <summary>
        /// Обрабатывает событие запроса на удаление условия
        /// </summary>
        /// <param name="searchExpression">Удаляемое условие</param>
        private void OnRemoveExpressionRequsted(SearchExpression searchExpression)
        {
            try
            {
                var item = ExpressionVMs.Single(t => t.SearchExpression == searchExpression);
                item.OnRemoveExpressionRequsted -= OnRemoveExpressionRequsted;
                ExpressionVMs.Remove(item);
                _searchPreset.SearchExpressionList.Remove(searchExpression);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }
        
        /// <summary>
        /// Команда добавления выражения сортировки.
        /// </summary>
        public DelegateCommand AddOrderingCommand { get; protected set; }

        /// <summary>
        /// Добавляет выражение сортировки.
        /// </summary>
        private void AddOrdering()
        {
            try
            {
                var vm = new AddSearchOrderingVM(_searchTypes);
                var result = UIFacade.ShowDialogView(new AddSearchOrderingView(), vm, "Добавить сортировку");
                if (result)
                {
                    var ordering = vm.SearchOrdering;
                    _searchPreset.AddOrderField(ordering);
                    var orderingVm = new OrderingVM(ordering);
                    orderingVm.OnRemoveOrderingRequested += this.OnRemoveOrderingRequsted;
                    OrderingVMs.Add(orderingVm);
                    this.RaisePropertyChanged(() => OrderingVMs);
                }
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        /// <summary>
        /// Обрабатывает событие запроса на удаление сортировки
        /// </summary>
        private void OnRemoveOrderingRequsted(OrderingVM item)
        {
            try
            {
                item.OnRemoveOrderingRequested -= OnRemoveOrderingRequsted;
                OrderingVMs.Remove(item);
                _searchPreset.OrderFieldList.Remove(item.SearchOrdering);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }

        #endregion
    }
}
