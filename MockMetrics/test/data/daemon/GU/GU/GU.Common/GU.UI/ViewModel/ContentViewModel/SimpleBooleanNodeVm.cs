using Common.BL.Validation;

using GU.DataModel;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class SimpleBooleanNodeVm : SimpleNodeVm
    {
        public SimpleBooleanNodeVm(ContentNode entity, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, contentNodeVmBuilder, isValidateable)
        {
        }

        public bool? BoolValue
        {
            get
            {
                return this.Entity.BoolValue;
            }
            set
            {
                if (this.Entity.BoolValue != value)
                {
                    this.Entity.BoolValue = value;
                    this.RaisePropertyChanged(() => this.BoolValue);
                }
            }
        }

        public override void RaiseValidatingPropertyChanged()
        {
            this.RaisePropertyChanged(() => BoolValue);
        }
    }
}