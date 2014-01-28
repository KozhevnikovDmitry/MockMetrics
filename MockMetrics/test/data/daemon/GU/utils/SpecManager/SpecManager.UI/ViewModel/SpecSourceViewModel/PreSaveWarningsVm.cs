using System.Collections.Generic;

using Microsoft.Practices.Prism.ViewModel;

using SpecManager.BL.SpecSource;

namespace SpecManager.UI.ViewModel.SpecSourceViewModel
{
    public class PreSaveWarningsVm : NotificationObject
    {
        public PreSaveWarningsVm(PreSaveWarning preSaveWarning)
        {
            this.Messages = preSaveWarning.Messages;
            Title = preSaveWarning.Title;
        }

        public IList<string> Messages { get; private set; }

        public string Title { get; private set; }
    }
}
