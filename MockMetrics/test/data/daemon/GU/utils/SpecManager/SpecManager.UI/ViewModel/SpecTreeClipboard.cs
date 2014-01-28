using SpecManager.BL.Model;
using SpecManager.UI.ViewModel.SpecViewModel;

namespace SpecManager.UI.ViewModel
{
    public class SpecTreeClipboard
    {
        private SpecNodeTreeNodeVm _holdedItem;

        private bool _isHoldedItemCut;

        public void Copy(ISpecTreeNodeVm nodeVm)
        {
            if (nodeVm.SpecNodeType != null)
            {
                this._holdedItem = nodeVm as SpecNodeTreeNodeVm;
                this._isHoldedItemCut = false;
            }
        }

        public void Cut(ISpecTreeNodeVm nodeVm)
        {
            if (nodeVm.SpecNodeType != null)
            {
                this._holdedItem = nodeVm as SpecNodeTreeNodeVm;
                this._isHoldedItemCut = true;
            }
        }

        public void Paste(ISpecTreeNodeVm targetNodeVm)
        {
            if (_holdedItem != null
                && (targetNodeVm.SpecNodeType == null 
                    || targetNodeVm.SpecNodeType.Value == SpecNodeType.Complex
                    || targetNodeVm.SpecNodeType.Value == SpecNodeType.ComplexChoice))
            {
                if (_isHoldedItemCut)
                {
                    _holdedItem.Remove();
                    targetNodeVm.AddChild(_holdedItem.SpecNode.Clone());
                    return;
                }

                targetNodeVm.AddChild(_holdedItem.SpecNode.Clone());
            }
        }
    }
}
