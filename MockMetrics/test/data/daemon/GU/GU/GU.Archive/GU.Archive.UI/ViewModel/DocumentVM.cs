using System;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;

using GU.Archive.DataModel;

namespace GU.Archive.UI.ViewModel
{
    public class DocumentVM : ValidateableVM
    {
        public DocumentVM(Document document, bool isValidateable = true)
            :base(isValidateable)
        {
            Document = document;
            AllowValidate = false;
        }

        public Document Document { get; set; }

        #region Binding Properties

        public bool AllowCopyData { get; set; }

        public bool CanCopyData { get; set; }

        public string SerialNumber
        {
            get
            {
                return Document.SerialNumber;
            }
            set
            {
                if (Document.SerialNumber != value)
                {
                    Document.SerialNumber = value;
                    RaisePropertyChanged(() => SerialNumber);
                }
            }
        }

        public string Number
        {
            get
            {
                return Document.Number;
            }
            set
            {
                if (Document.Number != value)
                {
                    Document.Number = value;
                    RaisePropertyChanged(() => Number);
                }
            }
        }

        public string Distributor
        {
            get
            {
                return Document.Distributor;
            }
            set
            {
                if (Document.Distributor != value)
                {
                    Document.Distributor = value;
                    RaisePropertyChanged(() => Distributor);
                }
            }
        }

        public string DistributorDeptCode
        {
            get
            {
                return Document.DistributorDeptCode;
            }
            set
            {
                if (Document.DistributorDeptCode != value)
                {
                    Document.DistributorDeptCode = value;
                    RaisePropertyChanged(() => DistributorDeptCode);
                }
            }
        }

        public DateTime? Stamp
        {
            get
            {
                return Document.Stamp;
            }
            set
            {
                if (!Document.Stamp.HasValue || Document.Stamp.Value != value)
                {
                    Document.Stamp = value;
                    RaisePropertyChanged(() => Stamp);
                }
            }
        }             

        private bool _isEditable = true;

        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }
            set
            {
                if (_isEditable != value)
                {
                    _isEditable = value;
                    RaisePropertyChanged(() => IsEditable);
                }
            }
        }

        #endregion

        #region Binding Commands
        
        #endregion           
    }
}
