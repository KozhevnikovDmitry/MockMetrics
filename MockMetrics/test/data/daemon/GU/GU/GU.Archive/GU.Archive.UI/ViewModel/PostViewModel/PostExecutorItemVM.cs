using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.BL.Validation;
using Common.UI.ViewModel.ListViewModel;
using GU.Archive.DataModel;
using GU.BL;

namespace GU.Archive.UI.ViewModel.PostViewModel
{
    public class PostExecutorItemVM : AbstractListItemVM<PostExecutor>
    {
        public PostExecutorItemVM(PostExecutor entity, bool isValidateable)
            : base(entity, isValidateable)
        {
        }

        protected override void Initialize()
        {
            var dbUser = GuFacade.GetDbUser();
            ExecutorString = string.Format("{0}, {1} {2}",
                                           Entity.Employee.ToString(),
                                           Entity.Stamp.ToLongDateString(),
                                           Entity.Stamp.ToShortTimeString());
            UserName = dbUser.Name;
            Note = Entity.Note;
        }

        public string ExecutorString { get; set; }

        public string UserName { get; set; }

        public string Note { get; set; }
    }
}
