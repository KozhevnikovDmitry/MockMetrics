using System;
using System.Collections.Generic;

using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI.ViewModel.SearchViewModel;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.UI.ViewModel.SearchViewModel
{
    /// <summary>
    /// Класс ViewModel для окна поиска лицензий
    /// </summary>
    public class LicenseSearchVm : AbstractSearchVM<License>
    {
        /// <summary>
        /// Класс ViewModel для окна поиска лицензий
        /// </summary>
        /// <param name="strategy">Стратегия поиска лицензий</param>
        /// <param name="dataMapper">Маппер лицензий</param>
        /// <param name="searchPresetContainer">Контейнер пресетов поиска</param>
        public LicenseSearchVm(ISearchStrategy<License> strategy, IDomainDataMapper<License> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            SearchPresetVM = new SearchPresetVM(SearchPreset, new List<Type> 
            { 
                typeof(License),
                typeof(LicenseRequisites)
            });
        }
    }
}
