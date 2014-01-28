using Common.DA.Interface;
using Common.UI.Interface;
using Common.UI.ViewModel;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel
{
    public interface IEditableHostInfo
    {
        IEditableVM<T> GetActiveEditableVm<T>() where T : IDomainObject;
    }

    public class MzAvalonDockVm : BaseAvalonDockVM
    {
        private readonly IEditableFacade _editableFacade;

        public MzAvalonDockVm(IDockableUiFactory uiFactory, IEditableFacade editableFacade)
            : base(uiFactory)
        {
            _editableFacade = editableFacade;
        }

        protected override bool PrepareWorkspaceClosing(IDockableVM workspaceVM)
        {
            bool isClosing = false;
            if (workspaceVM is EditableDockableVM)
            {
                var edvm = workspaceVM as EditableDockableVM;
                isClosing = edvm.EditableDataContext.OnClosing(edvm.DisplayName);
                if (isClosing)
                {
                    if (edvm.EditableDataContext is ISmartEditableVm)
                    {
                        _editableFacade.Close(edvm.EditableDataContext as ISmartEditableVm);
                    }
                }
            }
            else
            {
                isClosing = true;
            }

            return isClosing;
        }
    }
}
