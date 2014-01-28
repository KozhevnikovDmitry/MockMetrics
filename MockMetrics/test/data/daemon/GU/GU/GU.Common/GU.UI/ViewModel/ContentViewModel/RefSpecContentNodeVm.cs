using Common.BL.Validation;

using GU.DataModel;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class RefSpecContentNodeVm : ComplexContentNodeVM
    {
        private readonly Spec _refSpec;

        public RefSpecContentNodeVm(ContentNode entity, Spec refSpec, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, contentNodeVmBuilder, isValidateable)
        {
            this._refSpec = refSpec;
        }

        public override string Name
        {
            get
            {
                return Entity.SpecNode.Name;
            }
        }
    }
}