using System;
using System.Collections.Generic;

using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI.ViewModel.SearchViewModel;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using GU.MZ.DataModel.Holder;

namespace GU.MZ.UI.ViewModel.SearchViewModel
{
    /// <summary>
    /// Класс ViewModel для окна поиска лицензиатов
    /// </summary>
    public class LicenseHolderSearchVm : AbstractSearchVM<LicenseHolder>
    {
        /// <summary>
        /// Класс ViewModel для окна поиска томов лицензионного дела
        /// </summary>
        /// <param name="strategy">Стратегия поиска томов лицензионного дела</param>
        /// <param name="dataMapper">Маппер томов лицензионного дела</param>
        /// <param name="searchPresetContainer">Контейнер пресетов поиска</param>
        public LicenseHolderSearchVm(ISearchStrategy<LicenseHolder> strategy, IDomainDataMapper<LicenseHolder> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            SearchPresetVM = new SearchPresetVM(SearchPreset, new List<Type>
            {
                typeof(LicenseHolder), 
                typeof(HolderRequisites)
            });
        }
    }
}
