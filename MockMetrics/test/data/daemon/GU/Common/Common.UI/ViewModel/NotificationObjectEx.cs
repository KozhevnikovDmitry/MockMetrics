using Common.UI.ViewModel.Interfaces;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel
{
    public class NotificationObjectEx : NotificationObject, IRaisePropertyChanged
    {
        public void RaiseNotifyPropertyChanged(string propertyName)
        {
            RaisePropertyChanged(propertyName);
        }
    }
}
