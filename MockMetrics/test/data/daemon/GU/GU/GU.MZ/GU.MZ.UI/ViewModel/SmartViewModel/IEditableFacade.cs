using Common.BL.Validation;
using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public interface IEditableFacade
    {
        void Register<T>(ISmartEditableVm vm, T entity) where T : DomainObject<T>, IPersistentObject;
        void Save<T>(ISmartEditableVm vm) where T : DomainObject<T>, IPersistentObject;
        void Resubscribe<T>(ISmartEditableVm vm) where T : DomainObject<T>, IPersistentObject;
        void Close(ISmartEditableVm vm);
        ValidationErrorInfo Validate<T>(ISmartEditableVm vm) where T : DomainObject<T>, IPersistentObject;
    }

    public interface IValidateFacade
    {
        string Validate<T>(ISmartValidateableVm vm, string columnName) where T : IDomainObject;
        ValidationErrorInfo Validate<T>(ISmartValidateableVm vm) where T : IDomainObject;
        void RaiseValidatingPropertyChanged<T>(ISmartValidateableVm vm) where T : IDomainObject;
    }

    public interface IEntityFacade : IEditableFacade, IValidateFacade
    {
        
    }
}