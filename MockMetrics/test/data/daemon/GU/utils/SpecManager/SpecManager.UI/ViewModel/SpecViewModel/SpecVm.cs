using SpecManager.BL.Model;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public class SpecVm : SpecVmBase
    {
        private readonly Spec _spec;

        public SpecVm(Spec specItem)
        {
            _spec = specItem;
        }

        #region Binding Properties

        public string Name
        {
            get
            {
                return _spec.Name;
            }
            set
            {
                if (_spec.Name != value)
                {
                    _spec.Name = value;
                    this.RaisePropertyChanged(() => this.Name);
                    this.OnNameChanged(_spec.Name);
                }
            }
        }

        public string Uri
        {
            get
            {
                return _spec.Uri;
            }
            set
            {
                if (_spec.Uri != value)
                {
                    _spec.Uri = value;
                    this.RaisePropertyChanged(() => this.Uri);
                }
            }
        }

        #endregion

        #region Binding Commands

        #endregion
    }
}
