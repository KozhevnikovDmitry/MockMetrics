using System.Windows.Media;

using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel
{
    public class SplashVM : NotificationObject
    {
        #region Binding Properties

        public SplashVM()
        {

        }

        public string ApplicationName { get; set; }
        
        #endregion
    }
}
