using Common.DA;
using Common.DA.Interface;
using Common.UI.ViewModel.Interfaces;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public interface ISmartEditableVm : IEditableVM
    {
        DelegateCommand OnCloseCommand { get; }
    }

    public interface ISmartEditableVm<T> : IEditableVM<T>, ISmartEditableVm where T : DomainObject<T>, IPersistentObject
    {
        void Initialize(T entity, IEditableFacade editableFacade, bool isEditable = false);
    }
}