using System.Windows.Controls;
using Common.DA.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.WorkspaceViewModel
{
    /// <summary>
    /// Класс представляющий Модель Представления шаблона поиска. 
    /// </summary>
    public class SearchTemplateVM : BaseWorkspaceVM, ISearchTemplateVM
    {
        public SearchTemplateVM(UserControl view, IDomainObject searchObject)
        {
            View = view;
            SearchObject = searchObject;
        }

        public IDomainObject SearchObject { get; set; }
    }
}
