using Common.UI.View.SearchView;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.WorkspaceViewModel
{
    /// <summary>
    /// Класс ViewModel для окна диалога с рабочей областью поиска доменных объектов. 
    /// </summary>
    internal class SearchDialogVM : DialogVM
    {
        /// <summary>
        /// Класс ViewModel для окна диалога с рабочей областью поиска доменных объектов. 
        /// </summary>
        /// <param name="searchVm">ViewModel рабочей области поиска доменных объектов</param>
        public SearchDialogVM(ISearchVM searchVm)
        {
            SearchVm = searchVm;
            View = new SearchView {DataContext = searchVm};
        }

        /// <summary>
        /// ViewModel рабочей области поиска доменных объектов.
        /// </summary>
        public ISearchVM SearchVm { get; private set; }
    }
}
