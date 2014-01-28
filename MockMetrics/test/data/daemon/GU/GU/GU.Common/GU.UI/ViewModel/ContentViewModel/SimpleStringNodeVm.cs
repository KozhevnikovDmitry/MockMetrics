using Common.BL.Validation;

using GU.DataModel;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class SimpleStringNodeVm : SimpleNodeVm
    {
        public SimpleStringNodeVm(ContentNode entity, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, contentNodeVmBuilder, isValidateable)
        {
        }

        public string StrValue
        {
            get
            {
                return this.Entity.StrValue;
            }
            set
            {
                if (this.Entity.StrValue != value)
                {
                    this.Entity.StrValue = value;
                    this.RaisePropertyChanged(() => this.StrValue);
                }
            }
        }

        public string FormatRegexp
        {
            get
            {
                return this.Entity.SpecNode.FormatRegexp;
            }
        }

        public string FormatDescription
        {
            get
            {
                return this.Entity.SpecNode.FormatDescription;
            }
        }

        public override void RaiseValidatingPropertyChanged()
        {
            this.RaisePropertyChanged(() => StrValue);
        }
    }
}