using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI;
using Common.UI.ViewModel.SearchViewModel;
using Common.UI.ViewModel.SearchViewModel.CustomSearch;
using GU.Archive.DataModel;
using GU.Archive.UI.View.PostView;
using GU.Archive.UI.ViewModel.PostViewModel;

namespace GU.Archive.UI.ViewModel.SearchViewModel
{
    public class PostSearchVM : AbstractSearchVM<Post>
    {
        public PostSearchVM(ISearchStrategy<Post> strategy, IDomainDataMapper<Post> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            var searchObject = Post.CreateInstance();
            
            SearchTemplateVMs.Add( UIFacade.CreateSearchTemplateVM(new PostCommonView(), 
                                                                   new PostDataVM(searchObject, false), 
                                                                   searchObject, 
                                                                   "Общая информация"));
            SearchPresetVM = new SearchPresetVM(SearchPreset, new List<Type>() { typeof(Post) });
        }
    }
}
