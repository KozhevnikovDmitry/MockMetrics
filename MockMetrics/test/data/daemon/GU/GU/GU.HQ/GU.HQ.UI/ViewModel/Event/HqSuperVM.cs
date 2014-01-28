using System;

namespace GU.HQ.UI.ViewModel.Event
{
    public class HqSuperVM : HqValidateableVM, IRebuildRequest
    {
        public event ViewModelRebuildRequested RebuildRequested;

        protected virtual void RaiseRebuildRequest(EventArgs e)
        {
            if (RebuildRequested != null)
                RebuildRequested(this, e);
        }
    }
}