using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.UI.ViewModel.Interfaces
{
    public interface IRaisePropertyChanged
    {
        void RaiseNotifyPropertyChanged(string propertyName);
    }
}
