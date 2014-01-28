using Common.BL.Validation;

using GU.DataModel;

namespace GU.UI.ViewModel.ContentViewModel
{
    public abstract class SimpleNodeVm : ContentNodeVM
    {
        protected SimpleNodeVm(ContentNode entity, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, contentNodeVmBuilder, isValidateable)
        {
        }

        public override string Name
        {
            get
            {
                return this.Entity.SpecNode.Name + (this.Entity.SpecNode.MinOccurs > 0 ? "*" : string.Empty);
            }
        }

        public AttrDataType AttrDataType
        {
            get
            {
                return this.Entity.SpecNode.AttrDataType.Value;
            }
        }

        protected override bool CanDeleteSpecific()
        {
            if (this.Entity.SpecNode.MinOccurs == 0 && this.Entity.SpecNode.MaxOccurs == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}