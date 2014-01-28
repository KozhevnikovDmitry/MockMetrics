using System;
using System.Collections.Generic;
using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.SearchViewModel;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using GU.Trud.DataModel;
using GU.Trud.UI.View.Search.SearchTemplateView;
using GU.Trud.UI.ViewModel.Search.SearchTemplateViewModel;

namespace GU.Trud.UI.ViewModel.Search
{
    public class TaskExportSearchVM : AbstractSearchVM<TaskExport>
    {
        public TaskExportSearchVM(ISearchStrategy<TaskExport> strategy, IDomainDataMapper<TaskExport> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            var searchTaskexport = TaskExport.CreateInstance();
            SearchTemplateVMs.Add(
                UIFacade.CreateSearchTemplateVM(
                    new TaskExportSearchTemplateView(),
                    new TaskExportSearchTemplateVM(searchTaskexport),
                    searchTaskexport,
                    "Данные выгрузки"));
            SearchPresetVM = new SearchPresetVM(SearchPreset, new List<Type> { typeof(TaskExport) });
        }

        protected override void OnChooseResultRequested(object sender, ChooseItemRequestedEventArgs e)
        {
            RaiseChooseResultRequested(e.Result);
        }
    }
}
