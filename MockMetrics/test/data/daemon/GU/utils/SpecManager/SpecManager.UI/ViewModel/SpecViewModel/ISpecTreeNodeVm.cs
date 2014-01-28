using System.Collections.ObjectModel;

using SpecManager.BL.Model;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public interface ISpecTreeNodeVm 
    {
        string Name { get; }

        string TypeText { get; }

        bool IsSelected { get; set; }

        bool IsExpanded { get; }

        bool IsRoot { get; }

        bool IsOptional { get; }

        bool IsComplex { get; }

        SpecNodeParent Item { get; }

        SpecNodeType? SpecNodeType { get; }

        ISpecTreeNodeVm ParentVm { get; set; }

        ObservableCollection<ISpecTreeNodeVm> ChildTreeNodeVmList { get; set; }
        
        void AddChild(SpecNode specNode);

        void AddChild(SpecNodeType specNodeType);
        
        void AddChild(int index, SpecNodeType specNodeType);

        void RaiseNodeCollectionChangesChanged();

        void UnsubscribeCollectionChanged();

        void RaiseSpecNodeDataChanged();
    }
}