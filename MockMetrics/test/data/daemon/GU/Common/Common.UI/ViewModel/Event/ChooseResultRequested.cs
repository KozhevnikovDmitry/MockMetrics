using System;
using Common.DA.Interface;

namespace Common.UI.ViewModel.Event
{
    public delegate void ChooseItemRequestedDelegate(object sender, ChooseItemRequestedEventArgs e);
    
    public class ChooseItemRequestedEventArgs : EventArgs 
    {
        public ChooseItemRequestedEventArgs(IDomainObject result)
        {
            Result = result;
        }

        public IDomainObject Result { get; private set; }
    }
}
