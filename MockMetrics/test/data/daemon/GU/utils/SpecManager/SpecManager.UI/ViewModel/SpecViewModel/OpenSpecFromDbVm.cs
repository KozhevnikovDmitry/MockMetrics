using Microsoft.Practices.Prism.ViewModel;

using SpecManager.UI.ViewModel.SpecSourceViewModel;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public class OpenSpecFromDbVm : NotificationObject
    {
        private readonly DbConfigVm _configVm;

        public OpenSpecFromDbVm(DbConfigVm configVm)
        {
            _configVm = configVm;
            OpenById = true;
            OpenByUri = false;
        }

        #region Binding Properties

        private bool _openById;

        public bool OpenById
        {
            get
            {
                return this._openById;
            }
            set
            {
                if (!value.Equals(this._openById))
                {
                    this._openById = value;
                    this.RaisePropertyChanged(() => this.OpenById);
                }
            }
        }

        private bool _openByUri;

        public bool OpenByUri
        {
            get
            {
                return this._openByUri;
            }
            set
            {
                if (!value.Equals(this._openByUri))
                {
                    this._openByUri = value;
                    this.RaisePropertyChanged(() => this.OpenByUri);
                }
            }
        }

        private string _uri;

        public string Uri
        {
            get { return _uri; }
            set
            {
                if (value != _uri)
                {
                    _uri = value.Trim();
                    RaisePropertyChanged(() => Uri);
                }
            }
        }

        public int SpecId { get; set; }

        public DbConfigVm DbConfigVm
        {
            get
            {
                return _configVm;
            }
        }

        #endregion

    }
}
