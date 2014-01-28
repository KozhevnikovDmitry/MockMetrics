using System;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.Event
{
    /// <summary>
    /// Делегат событий запроса на пересобираение ViewModel'ов учетного дела
    /// </summary>
    public delegate void ViewModelRebuildRequested(object sender, ClaimEventsArgs claimEventArgs);

    /// <summary>
    /// Параметры события
    /// </summary>
    public class ClaimEventsArgs : EventArgs
    {
        public ClaimEventsArgs(Claim claim)
        {
            Claim = claim;
        }

        public Claim Claim { get; private set; }
    }
}