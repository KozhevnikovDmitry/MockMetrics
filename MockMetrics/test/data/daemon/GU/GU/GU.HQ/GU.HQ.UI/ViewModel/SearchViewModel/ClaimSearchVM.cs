using System;
using System.Collections.Generic;
using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI;
using Common.UI.ViewModel.SearchViewModel;
using GU.HQ.BL.Search.SearchDomain;
using GU.HQ.DataModel;
using GU.HQ.UI.View.Search.SearchTemplateViewModel;
using GU.HQ.UI.ViewModel.SearchViewModel.SearchTemplateViewModel;

namespace GU.HQ.UI.ViewModel.SearchViewModel
{
    public class ClaimSearchVM : AbstractSearchVM<Claim>
    {
        // конструктор для общего списка
        public ClaimSearchVM(ISearchStrategy<Claim> strategy, IDomainDataMapper<Claim> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            var searchClaim = new SearchClaim();  
            SearchTemplateVMs.Add(UIFacade.CreateSearchTemplateVM(new ClaimSearchTemplateView(), new ClaimSearchTemplateVM(searchClaim), searchClaim, "Данные заявки"));
            SearchPresetVM = UIFacade.CreateSearchPresetVM(this.SearchPreset, new List<Type> { typeof(Claim) });
        }

        // Конструктор для поиска
        internal ClaimSearchVM(ISearchStrategy<Claim> strategy, IDomainDataMapper<Claim> dataMapper, SearchClaim searchClaim)
            : base(strategy, dataMapper, new SearchSpecificationContainer())
        {
            SearchTemplateVMs.Add(UIFacade.CreateSearchTemplateVM(new ClaimSearchTemplateView(), new ClaimSearchTemplateVM(searchClaim), searchClaim, "Данные заявки"));
            SearchPresetVM = UIFacade.CreateSearchPresetVM(this.SearchPreset, new List<Type> { typeof(Claim) });
            IsDefaultSearchEnabled = false;
            DefaultSearchCommand.RaiseCanExecuteChanged();
        }
    }
}
