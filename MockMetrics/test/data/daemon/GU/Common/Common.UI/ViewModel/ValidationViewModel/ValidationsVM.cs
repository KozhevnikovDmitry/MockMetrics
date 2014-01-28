using System.Collections.Generic;

using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.ValidationViewModel
{
    public class ValidationsVM : NotificationObject
    {
        public ValidationsVM(IList<string> validations)
        {
            this.Validations = validations;
        }

        public IList<string> Validations { get; private set; }
    }
}
