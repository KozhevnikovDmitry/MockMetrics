using System;
using System.Collections.Generic;
using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI;
using Common.UI.ViewModel.SearchViewModel;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using GU.Archive.DataModel;
using GU.Archive.UI.View.EmployeeView;
using GU.Archive.UI.ViewModel.EmployeeViewModel;

namespace GU.Archive.UI.ViewModel.SearchViewModel
{
    public class EmployeeSearchVM : AbstractSearchVM<Employee>
    {
        public EmployeeSearchVM(ISearchStrategy<Employee> strategy, IDomainDataMapper<Employee> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            var searchEmployee = Employee.CreateInstance();
            SearchTemplateVMs.Add(UIFacade.CreateSearchTemplateVM(new EmployeeDataView(), 
                                                                  new EmployeeDataVM(searchEmployee, false), 
                                                                  searchEmployee, 
                                                                  "Рабочие данные"));
            SearchPresetVM = new SearchPresetVM(SearchPreset, new List<Type>() { typeof(Employee) });
        }
    }
}
