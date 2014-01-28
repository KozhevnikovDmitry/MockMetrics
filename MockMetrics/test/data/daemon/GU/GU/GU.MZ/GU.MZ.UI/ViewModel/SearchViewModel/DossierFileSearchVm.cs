﻿using System;
using System.Collections.Generic;

using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI.ViewModel.SearchViewModel;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.UI.ViewModel.SearchViewModel
{
    /// <summary>
    /// Класс ViewModel для окна поиска томов лицензионного дела
    /// </summary>
    public class DossierFileSearchVm : AbstractSearchVM<DossierFile>
    {
        /// <summary>
        /// Класс ViewModel для окна поиска томов лицензионного дела
        /// </summary>
        /// <param name="strategy">Стратегия поиска томов лицензионного дела</param>
        /// <param name="dataMapper">Маппер томов лицензионного дела</param>
        /// <param name="searchPresetContainer">Контейнер пресетов поиска</param>
        public DossierFileSearchVm(ISearchStrategy<DossierFile> strategy, IDomainDataMapper<DossierFile> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            SearchPresetVM = new SearchPresetVM(
                SearchPreset,
                new List<Type>
                    {
                        typeof(DossierFile),
                        typeof(Task),
                        typeof(LicenseDossier),
                        typeof(LicenseHolder),
                        typeof(HolderRequisites),
                        typeof(JurRequisites),
                        typeof(IndRequisites)
                    });
        }
    }
}