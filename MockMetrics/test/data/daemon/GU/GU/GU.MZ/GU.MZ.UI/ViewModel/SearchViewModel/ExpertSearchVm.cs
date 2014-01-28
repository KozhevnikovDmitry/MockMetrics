using System;
using System.Collections.Generic;

using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI.ViewModel.SearchViewModel;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using GU.MZ.DataModel.Person;

namespace GU.MZ.UI.ViewModel.SearchViewModel
{
    /// <summary>
    /// Класс ViewModel для окна поиска экспертов
    /// </summary>
    public class ExpertSearchVm : AbstractSearchVM<Expert>
    {
        /// <summary>
        /// Класс ViewModel для окна поиска экспертов
        /// </summary>
        /// <param name="strategy">Стратегия поиска экспертов</param>
        /// <param name="dataMapper">Маппер экспертов</param>
        /// <param name="searchPresetContainer">Контейнер пресетов поиска</param>
        public ExpertSearchVm(ISearchStrategy<Expert> strategy, IDomainDataMapper<Expert> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            SearchPresetVM = new SearchPresetVM(SearchPreset, new List<Type> { typeof(Expert), typeof(IndividualExpertState), typeof(JuridicalExpertState) });
        }
    }
}
