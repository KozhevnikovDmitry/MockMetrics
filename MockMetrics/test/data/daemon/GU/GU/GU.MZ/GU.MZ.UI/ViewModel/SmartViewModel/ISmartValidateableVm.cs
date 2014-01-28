using Common.DA.Interface;
using Common.UI.ViewModel.Interfaces;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public interface ISmartValidateableVm : IValidateableVM
    {
        void RaisePropertyChanged(string propertyName);
        void RaiseAllPropertyChanged();
        void SetFacade(IValidateFacade validateFacade);
        bool IsInitialized { get; }
        void ReadyToValidate();
        void NotReadyToValidate();
    }

    public interface ISmartValidateableVm<T> : IDomainValidateableVM<T>, ISmartValidateableVm where T : IDomainObject
    {
        void Initialize(T entity);
    }
}