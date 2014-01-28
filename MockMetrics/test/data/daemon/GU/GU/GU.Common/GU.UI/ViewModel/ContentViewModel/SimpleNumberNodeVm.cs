using Common.BL.Validation;

using GU.DataModel;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class SimpleNumberNodeVm : SimpleNodeVm
    {
        public SimpleNumberNodeVm(ContentNode entity, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, contentNodeVmBuilder, isValidateable)
        {
        }

        public decimal? NumberValue
        {
            get
            {
                return this.Entity.NumValue;
            }
            set
            {
                if (this.Entity.NumValue != value)
                {
                    this.Entity.NumValue = value;
                    this.RaisePropertyChanged(() => this.NumberValue);
                }
            }
        }

        public override void RaiseValidatingPropertyChanged()
        {
            this.RaisePropertyChanged(() => NumberValue);
        }
    }
}