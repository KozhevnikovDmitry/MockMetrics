using System;

using Common.BL.Validation;

using GU.DataModel;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class SimpleDateNodeVm : SimpleNodeVm
    {
        public SimpleDateNodeVm(ContentNode entity, IDomainValidator<ContentNode> domainValidator,ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator,contentNodeVmBuilder, isValidateable)
        {
        }

        public DateTime? DateValue
        {
            get
            {
                return this.Entity.DateValue;
            }
            set
            {
                if (this.Entity.DateValue != value)
                {
                    this.Entity.DateValue = value;
                    this.RaisePropertyChanged(() => this.DateValue);
                }
            }
        }

        public override void RaiseValidatingPropertyChanged()
        {
            this.RaisePropertyChanged(() => DateValue);
        }
    }
}