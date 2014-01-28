using SpecManager.BL.Model;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public class SpecTreeNodeVm : BaseSpecTreeNodeVm
    {
        private readonly Spec _spec;


        public override SpecNodeParent Item
        {
            get
            {
                return this._spec;
            }
        }

        public override SpecNodeType? SpecNodeType
        {
            get
            {
                return null;
            }
        }

        public Spec Spec
        {
            get
            {
                return _spec;
            }
        }

        public SpecTreeNodeVm(Spec spec)
        {
            this._spec = spec;
            this.CreateChildTreeNodeList(this._spec.ChildSpecNodes);
        }

        public override void AddChild(int index, SpecNodeType specNodeType)
        {
            SpecNode childSpecNode = this.Spec.AddChild(index, specNodeType);
            var childTreeNodeVm = new SpecNodeTreeNodeVm(childSpecNode) { ParentVm = this };
            this.ChildTreeNodeVmList.Insert(index, childTreeNodeVm);
            childTreeNodeVm.IsSelected = true;
        }

        public override void AddChild(SpecNode specNode)
        {
            SpecNode childSpecNode = this.Spec.AddChild(specNode);
            var childTreeNodeVm = new SpecNodeTreeNodeVm(childSpecNode) { ParentVm = this };
            this.ChildTreeNodeVmList.Add(childTreeNodeVm);
            childTreeNodeVm.IsSelected = true;
        }

        #region Binding Properties

        public override string Name
        {
            get
            {
                return this._spec.Name;
            }
        }

        public override string TypeText
        {
            get { return null; }
        }

        public override bool IsRoot
        {
            get { return true; }
        }

        public override bool IsOptional
        {
            get { return false; }
        }

        public override bool IsComplex
        {
            get { return true; }
        }

        #endregion

    }
}
