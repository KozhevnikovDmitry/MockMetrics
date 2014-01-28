using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Common.UI.ViewModel.ListViewModel;
using GU.Archive.DataModel;

namespace GU.Archive.UI.ViewModel.PostViewModel
{
    public class PostExecutorsVM : AbstractListVM<PostExecutor>
    {
        public PostExecutorsVM(Post post)
            : base(post.Executors)
        {
            CanAddItems = false;
            CanRemoveItems = false;
            Title = "Список исполнителей";
        }
    }
}
