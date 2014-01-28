using Common.DA.Interface;
using Common.UI.View.SearchView;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;

namespace Common.UI.ViewModel.SearchViewModel.CustomSearch
{
    public class CustomSearchTemplateVM<T> : BaseWorkspaceVM, ICustomSearchTemplateVM<T> where T : IDomainObject
    {
        public CustomSearchTemplateVM(SearchPresetView view)
        {
            View = view;
        }
    }
}
