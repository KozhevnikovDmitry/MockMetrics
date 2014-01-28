using System;
using System.Collections.Generic;

using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI.ViewModel.SearchViewModel;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using GU.DataModel;
using GU.MZ.DataModel.Person;

namespace GU.MZ.UI.ViewModel.SearchViewModel
{
    /// <summary>
    /// Класс ViewModel для окна поиска сотрудников
    /// </summary>
    public class EmployeeSearchVm : AbstractSearchVM<Employee>
    {
        /// <summary>
        /// Класс ViewModel для окна поиска сотрудников
        /// </summary>
        /// <param name="strategy">Стратегия поиска сотрудников</param>
        /// <param name="dataMapper">Маппер сотрдуников</param>
        /// <param name="searchPresetContainer">Контейнер пресетов поиска</param>
        public EmployeeSearchVm(ISearchStrategy<Employee> strategy, IDomainDataMapper<Employee> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            SearchPresetVM = new SearchPresetVM(SearchPreset, new List<Type> { typeof(Employee), typeof(DbUser) });
        }
    }
}
