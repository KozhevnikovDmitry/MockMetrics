using System;
using System.Collections.Generic;
using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI;
using Common.UI.ViewModel.SearchViewModel;

using GU.BL.Search.SearchDomain;
using GU.DataModel;
using GU.UI.View.SearchView.SearchTemplateView;
using GU.UI.ViewModel.SearchViewModel.SearchTemplateViewModel;

namespace GU.UI.ViewModel.SearchViewModel
{
    public class TaskSearchVM : AbstractSearchVM<Task>
    {
        public TaskSearchVM(ISearchStrategy<Task> strategy, IDomainDataMapper<Task> dataMapper, SearchSpecificationContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            var searchTask = new SearchTask();
            SearchTemplateVMs.Add(UIFacade.CreateSearchTemplateVM(new SearchTaskTemplateView(), new TaskSearchTemplateVM(searchTask), searchTask, "Данные заявления"));
            SearchPresetVM = UIFacade.CreateSearchPresetVM(this.SearchPreset, new List<Type> { typeof(Task) });
        }

        internal TaskSearchVM(ISearchStrategy<Task> strategy, IDomainDataMapper<Task> dataMapper, SearchTask searchTask)
            : base(strategy, dataMapper, new SearchSpecificationContainer())
        {
            SearchTemplateVMs.Add(UIFacade.CreateSearchTemplateVM(new SearchTaskTemplateView(), new TaskSearchTemplateVM(searchTask), searchTask, "Данные заявления"));
            SearchPresetVM = UIFacade.CreateSearchPresetVM(this.SearchPreset, new List<Type> { typeof(Task) });
            IsDefaultSearchEnabled = false;
            DefaultSearchCommand.RaiseCanExecuteChanged();
        }
    }
}
