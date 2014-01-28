using System.Linq;

using SpecManager.BL.Interface;
using SpecManager.BL.Model;
using SpecManager.UI.ViewModel.Exceptions;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public class SpecNodeTreeNodeVm : BaseSpecTreeNodeVm
    {
        private readonly SpecNode _specNode;


        public override SpecNodeParent Item
        {
            get
            {
                return this._specNode;
            }
        }

        public override SpecNodeType? SpecNodeType
        {
            get
            {
                return _specNode.SpecNodeType;
            }
        }

        public SpecNode SpecNode
        {
            get
            {
                return _specNode;
            }
        }

        public SpecNodeTreeNodeVm(SpecNode specNode)
        {
            this._specNode = specNode;
            this.CreateChildTreeNodeList(specNode.ChildSpecNodes);
        }

        public override string Name
        {
            get
            {
                return this._specNode.Name;
            }
        }

        public override string TypeText
        {
            get 
            {
                var typeText = "";
                switch (SpecNodeType)
                {
                    case BL.Model.SpecNodeType.ComplexChoice:
                        typeText = "?";
                        break;
                    case BL.Model.SpecNodeType.RefSpec:
                        typeText = "->";
                        break;
                    case BL.Model.SpecNodeType.Simple:
                        typeText = _specNode.AttrDataType.ToString().Substring(0, 1);
                        break;
                }

                if (_specNode.MaxOccurs > 1)
                    typeText = "..." + typeText;
                
                return typeText;
            }
        }

        public override bool IsRoot
        {
            get { return false; }
        }

        public override bool IsOptional
        {
            get { return _specNode.MinOccurs == 0; }
        }

        public override bool IsComplex
        {
            get
            {
                return _specNode.SpecNodeType == BL.Model.SpecNodeType.Complex ||
                       _specNode.SpecNodeType == BL.Model.SpecNodeType.ComplexChoice;
            }
        }

        public override void AddChild(int index, SpecNodeType specNodeType)
        {
            CheckParent();
            SpecNode childSpecNode = SpecNode.AddChild(index, specNodeType);
            var childTreeNodeVm = new SpecNodeTreeNodeVm(childSpecNode) { ParentVm = this };
            this.ChildTreeNodeVmList.Insert(index, childTreeNodeVm);
            childTreeNodeVm.IsSelected = true;
        }

        public override void AddChild(SpecNode specNode)
        {
            CheckParent();
            SpecNode childSpecNode = SpecNode.AddChild(specNode);
            var childTreeNodeVm = new SpecNodeTreeNodeVm(childSpecNode) { ParentVm = this };
            this.ChildTreeNodeVmList.Add(childTreeNodeVm);
            childTreeNodeVm.IsSelected = true;
        }

        public void ShiftUp()
        {
            CheckParent();
            int index = this.ParentVm.ChildTreeNodeVmList.IndexOf(this);

            if (index == -1)
            {
                throw new VmException("Элемент не принадлжит списку дочерних");
            }

            if (index > 0)
            {
                this.ParentVm.ChildTreeNodeVmList.RemoveAt(index);
                this.ParentVm.ChildTreeNodeVmList.Insert(index - 1, this);
            }

            this.SpecNode.ShiftUp();
            this.IsSelected = true;
        }

        public void ShiftDown()
        {
            CheckParent();
            int index = this.ParentVm.ChildTreeNodeVmList.IndexOf(this);

            if (index == -1)
            {
                throw new VmException("Элемент не принадлжит списку дочерних");
            }

            if (index < this.ParentVm.ChildTreeNodeVmList.Count - 1)
            {
                this.ParentVm.ChildTreeNodeVmList.RemoveAt(index);
                this.ParentVm.ChildTreeNodeVmList.Insert(index + 1, this);
            }

            this.SpecNode.ShiftDown();
            this.IsSelected = true;
        }

        public void Remove()
        {
            CheckParent();
            if (!this.ParentVm.ChildTreeNodeVmList.Contains(this))
            {
                throw new VmException("Элемент не принадлжит списку дочерних");
            }

            var index = this.ParentVm.ChildTreeNodeVmList.IndexOf(this);

            this.ParentVm.ChildTreeNodeVmList.Remove(this);

            this.SelectPreviouesNode(index);

            this.SpecNode.Remove();
        }

        private void SelectPreviouesNode(int index)
        {
            if (this.ParentVm.ChildTreeNodeVmList.Any())
            {
                if (index == this.ParentVm.ChildTreeNodeVmList.Count)
                {
                    index--;
                }

                this.ParentVm.ChildTreeNodeVmList.ElementAt(index).IsSelected = true;
            }
        }

        public void AddAfter()
        {
            CheckParent();
            var index = ParentVm.ChildTreeNodeVmList.IndexOf(this) + 1;
            ParentVm.AddChild(index, this._specNode.SpecNodeType);
            ParentVm.ChildTreeNodeVmList[index].IsSelected = true;
        }

        public void Copy()
        {
            this.CheckParent();
            var copyNode = SpecNode.Copy();
            var copyNodeVm = new SpecNodeTreeNodeVm(copyNode) { ParentVm = this.ParentVm };
            var index = this.ParentVm.ChildTreeNodeVmList.IndexOf(this);
            this.ParentVm.ChildTreeNodeVmList.Insert(index + 1, copyNodeVm);
        }

        private void CheckParent()
        {
            if (this.ParentVm == null || this.ParentVm.ChildTreeNodeVmList == null
                || this.ParentVm.ChildTreeNodeVmList.Count == 0)
            {
                throw new VmException("Родитель не установлен или список дочерних элементов не заполнен");
            }
        }
    }
}
