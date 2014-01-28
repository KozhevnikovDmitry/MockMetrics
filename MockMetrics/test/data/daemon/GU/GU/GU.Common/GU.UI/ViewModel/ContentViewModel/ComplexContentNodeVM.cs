using System.Collections.ObjectModel;

using Common.BL.Validation;

using GU.DataModel;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class ComplexContentNodeVM : ContentNodeVM
    {
        private ObservableCollection<ContentNodeVM> _contentNodeVmList;

        public ComplexContentNodeVM(ContentNode entity, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, contentNodeVmBuilder, isValidateable)
        {
            this.ContentNodeVmList = new ObservableCollection<ContentNodeVM>();
        }
        
        public ObservableCollection<ContentNodeVM> ContentNodeVmList
        {
            get
            {
                return this._contentNodeVmList;
            }
            private set
            {
                if (Equals(value, this._contentNodeVmList))
                {
                    return;
                }
                this._contentNodeVmList = value;
                this.RaisePropertyChanged(() => this.ContentNodeVmList);
            }
        }
        
        public override void RaiseValidatingPropertyChanged()
        {
            foreach (var contentNodeVM in ContentNodeVmList)
            {
                contentNodeVM.RaiseValidatingPropertyChanged();
            }
        }
    }
}