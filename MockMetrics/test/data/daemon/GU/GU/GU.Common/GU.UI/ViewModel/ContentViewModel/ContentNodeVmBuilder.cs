using System;

using GU.BL.DomainValidation;
using GU.DataModel;

using System.Linq;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class ContentNodeVmBuilder
    {
        private ContentNode _contentNode;

        private readonly ContentNodeValidator _contentNodeValidator;

        public ContentNodeVmBuilder()
        {
            this._contentNodeValidator = new ContentNodeValidator();
        }

        public ContentNodeVmBuilder For(ContentNode contentNode)
        {
            this._contentNode = contentNode;
            return this;
        }
        
        public ContentNodeVM Build(bool isEditable = true)
        {
            return this.Build(this._contentNode, isEditable);
        }

        private ContentNodeVM Build(ContentNode contentNode, bool isEditable)
        {
            switch (contentNode.SpecNode.SpecNodeType)
            {
                 case SpecNodeType.Simple:
                    {
                        return this.BuildSimpleNode(contentNode, isEditable);
                    }
                 case SpecNodeType.Complex:
                    {
                        return this.BuildComplexNode(contentNode, isEditable);
                    }
                 case SpecNodeType.ComplexChoice:
                    {
                        return this.BuildComplexChoiseNode(contentNode, isEditable);
                    }
                 case SpecNodeType.RefSpec:
                    {
                        return this.BuildRefSpecNode(contentNode, isEditable);
                    }
            }

            throw new NotSupportedException("SpecNodeType out of known range");
        }

        private ContentNodeVM BuildSimpleNode(ContentNode contentNode, bool isEditable)
        {
            switch (contentNode.SpecNode.AttrDataType)
            {
                case AttrDataType.String:
                    {
                        return new SimpleStringNodeVm(contentNode, _contentNodeValidator, this) { IsEditable = isEditable };
                    }
                case AttrDataType.Number:
                    {
                        return new SimpleNumberNodeVm(contentNode, _contentNodeValidator, this) { IsEditable = isEditable };
                    }
                case AttrDataType.Boolean:
                    {
                        return new SimpleBooleanNodeVm(contentNode, _contentNodeValidator, this) { IsEditable = isEditable };
                    }
                case AttrDataType.Date:
                    {
                        return new SimpleDateNodeVm(contentNode, _contentNodeValidator, this) { IsEditable = isEditable };
                    }
                case AttrDataType.List:
                    {
                        return new SimpleDictNodeVm(contentNode, _contentNodeValidator, this) { IsEditable = isEditable };
                    }
                case AttrDataType.File:
                    {
                        return new SimpleFileNodeVm(contentNode, _contentNodeValidator, this) { IsEditable = isEditable };
                    }
            }

            throw new NotSupportedException("AttrDataType out of range");
        }

        private ContentNodeVM BuildComplexNode(ContentNode contentNode, bool isEditable)
        {
            var complexContentNodeVm = new ComplexContentNodeVM(contentNode, _contentNodeValidator, this) { IsEditable = isEditable };

            foreach (var childSpecNode in contentNode.SpecNode.ChildSpecNodes.OrderBy(t => t.Order))
            {
                var contentNodes =
                    contentNode.ChildContentNodes.Where(t => t.SpecNode.Id == childSpecNode.Id).OrderBy(t => t.SpecNode.Order);

                if (contentNodes.Count() != 0)
                {
                    foreach (var childContentNode in contentNodes)
                    {
                        var newChildNodeVm = this.Build(childContentNode, isEditable);
                        newChildNodeVm.ParentContentNodeVm = complexContentNodeVm;
                        complexContentNodeVm.ContentNodeVmList.Add(newChildNodeVm);
                    }
                }
                else
                {
                    var newPotentionalNodeVm = new PotentionalContentNodeVM(childSpecNode, complexContentNodeVm, this) { IsEditable = isEditable };
                    complexContentNodeVm.ContentNodeVmList.Add(newPotentionalNodeVm);
                }

            }

            return complexContentNodeVm;
        }

        private ContentNodeVM BuildRefSpecNode(ContentNode contentNode, bool isEditable)
        {
            var refSpecContentNodeVm = new RefSpecContentNodeVm(contentNode, contentNode.SpecNode.RefSpec, _contentNodeValidator, this) { IsEditable = isEditable };

            foreach (var childSpecNode in contentNode.SpecNode.RefSpec.RootSpecNodes.OrderBy(t => t.Order))
            {
                var contentNodes =
                    contentNode.ChildContentNodes.Where(t => t.SpecNode.Id == childSpecNode.Id).OrderBy(t => t.SpecNode.Order);

                if (contentNodes.Count() != 0)
                {
                    foreach (var childContentNode in contentNodes)
                    {
                        var newChildNodeVm = this.Build(childContentNode, isEditable);
                        newChildNodeVm.ParentContentNodeVm = refSpecContentNodeVm;
                        refSpecContentNodeVm.ContentNodeVmList.Add(newChildNodeVm);
                    }
                }
                else
                {
                    var newPotentionalNodeVm = new PotentionalContentNodeVM(childSpecNode, refSpecContentNodeVm, this) { IsEditable = isEditable };
                    refSpecContentNodeVm.ContentNodeVmList.Add(newPotentionalNodeVm);
                }

            }

            return refSpecContentNodeVm;
        }

        private ContentNodeVM BuildComplexChoiseNode(ContentNode contentNode, bool isEditable)
        {
            var newChoiseContentNodeVm = new ComplexChoiseContentNodeVM(contentNode, _contentNodeValidator, this) {IsEditable = isEditable};
            
            newChoiseContentNodeVm.ChoiseContentNodeVm = this.Build(contentNode.ChildContentNodes.Single(), isEditable);

            return newChoiseContentNodeVm;
        }
    }
}
