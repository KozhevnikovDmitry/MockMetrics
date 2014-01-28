using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;

namespace GU.HQ.UI.ViewModel
{
    public abstract class HqValidateableVM : ValidateableVM
    {
        protected HqValidateableVM(bool isValidateable = true)
            :base(isValidateable)
        {
        }
    }
}
