using Microsoft.Practices.Prism.ViewModel;

namespace GU.UI.ViewModel
{
    public class AddNoteVM : NotificationObject
    {              
        #region Binding Properties

        private string _comment;        

        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    RaisePropertyChanged(() => Comment);
                }
            }
        }

        #endregion
    }
}
