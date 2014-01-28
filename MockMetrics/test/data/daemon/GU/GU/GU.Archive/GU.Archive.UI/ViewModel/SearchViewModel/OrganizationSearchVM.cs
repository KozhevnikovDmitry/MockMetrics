using System;
using System.Collections.Generic;
using System.ComponentModel;
using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI;
using Common.UI.ViewModel.SearchViewModel;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using GU.Archive.DataModel;
using GU.Archive.UI.View.OrganizationView;
using GU.Archive.UI.ViewModel.OrganizationViewModel;

namespace GU.Archive.UI.ViewModel.SearchViewModel
{
    public class OrganizationSearchVM : AbstractSearchVM<Organization>
    {
        public OrganizationSearchVM(ISearchStrategy<Organization> strategy, IDomainDataMapper<Organization> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            var searchObject = Organization.CreateInstance();
            SearchTemplateVMs.Add(UIFacade.CreateSearchTemplateVM(new OrganizationDataView(), 
                                                                  new OrganizationDataVM(searchObject), 
                                                                  searchObject, 
                                                                  "Данные организации"));
            SearchPresetVM = new SearchPresetVM(SearchPreset, new List<Type>() { typeof(Organization) });
        }
    }
}
