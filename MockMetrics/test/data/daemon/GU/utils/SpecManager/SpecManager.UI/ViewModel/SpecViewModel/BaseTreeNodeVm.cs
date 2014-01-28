using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Microsoft.Practices.Prism.ViewModel;

using SpecManager.BL.Model;
using System.Linq;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public abstract class BaseSpecTreeNodeVm : NotificationObject, ISpecTreeNodeVm
    {
        public abstract string Name { get; }
        
        public abstract string TypeText { get; }

        private bool _isSelected;

        public bool IsSelected  
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    this._isSelected = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    RaiseNodeCollectionChangesChanged();
                }
            }
        }

        public bool IsExpanded
        {
            get
            {
                return true;
            }
        }

        public abstract bool IsRoot { get; }
        
        public abstract bool IsOptional { get; }
        
        public abstract bool IsComplex { get; }
        
        public abstract SpecNodeParent Item { get; }

        public abstract SpecNodeType? SpecNodeType { get; }

        private ObservableCollection<ISpecTreeNodeVm> _childTreeNodeVmList;

        public ObservableCollection<ISpecTreeNodeVm> ChildTreeNodeVmList
        {
            get
            {
                return this._childTreeNodeVmList;
            }
            set
            {
                if (this._childTreeNodeVmList != value)
                {
                    this._childTreeNodeVmList = value;
                    this.RaisePropertyChanged(() => this.ChildTreeNodeVmList);
                }
            }
        }

        public ISpecTreeNodeVm ParentVm { get; set; }

        public void AddChild(SpecNodeType specNodeType)
        {
            AddChild(ChildTreeNodeVmList.Count, specNodeType);
        }

        public abstract void AddChild(int index, SpecNodeType specNodeType);

        public abstract void AddChild(SpecNode specNode);

        protected void CreateChildTreeNodeList(IEnumerable<SpecNode> specNodes)
        {
            this.ChildTreeNodeVmList = new ObservableCollection<ISpecTreeNodeVm>();

            foreach (var specNode in specNodes.OrderBy(t => t.Order))
            {
                this.ChildTreeNodeVmList.Add(new SpecNodeTreeNodeVm(specNode) { ParentVm = this });
            }

            ChildTreeNodeVmList.CollectionChanged += NodeCollectionChanged;
        }

        #region Event Handling

        void NodeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseNodeCollectionChangesChanged();
        }

        #endregion

        
        public void RaiseSpecNodeDataChanged()
        {
            this.RaisePropertyChanged(() => this.Name);
            this.RaisePropertyChanged(() => this.TypeText);
            this.RaisePropertyChanged(() => this.IsRoot);
            this.RaisePropertyChanged(() => this.IsOptional);
            this.RaisePropertyChanged(() => this.IsComplex);
        }

        public void RaiseNodeCollectionChangesChanged()
        {
            this.RaisePropertyChanged(() => ChildTreeNodeVmList); 
            if (ParentVm != null)
            {
                ParentVm.RaiseNodeCollectionChangesChanged();
            }
        }

        public void UnsubscribeCollectionChanged()
        {
            this.ChildTreeNodeVmList.CollectionChanged -= this.NodeCollectionChanged;

            foreach (var childVm in this.ChildTreeNodeVmList)
            {
                childVm.UnsubscribeCollectionChanged();
            }
        }
    }
}
