using Common.DA.Interface;
using Common.UI.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI
{
    public interface IListVmUiFactory
    {
        IListItemVM<T> GetListItemVm<T>(T item) where T : IDomainObject;
    }

    public interface IListDialogUiFactory : IListVmUiFactory, IDialogUiFactory
    {
        IDomainValidateableVM<T> GetDomainValidateableVm<T>(T entity) where T : IDomainObject;
    }
}