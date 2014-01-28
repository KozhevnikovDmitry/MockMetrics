using System.Collections.Generic;

using Common.BL.Validation;

using GU.DataModel;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class SimpleDictNodeVm : SimpleNodeVm
    {
        public SimpleDictNodeVm(ContentNode entity, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, contentNodeVmBuilder, isValidateable)
        {
        }
        
        public string DictKey
        {
            get
            {
                return this.Entity.DictKey;
            }
            set
            {
                this.Entity.DictKey = value;
                this.RaisePropertyChanged(() => this.DictKey);
            }
        }

        public string StrValue
        {
            get
            {
                return this.Entity.StrValue;
            }
            set
            {
                this.Entity.StrValue = value;
                this.RaisePropertyChanged(() => this.StrValue);
            }
        }

        public List<DictDet> DictDets
        {
            get
            {
                return this.Entity.SpecNode.Dict.DictDets;
            }
        }

        public bool IsEditableDict
        {
            get { return this.Entity.SpecNode.IsEditableDict ?? false; }
        }

        public override void RaiseValidatingPropertyChanged()
        {
            this.RaisePropertyChanged(() => DictKey);
            this.RaisePropertyChanged(() => StrValue);
        }
    }
}