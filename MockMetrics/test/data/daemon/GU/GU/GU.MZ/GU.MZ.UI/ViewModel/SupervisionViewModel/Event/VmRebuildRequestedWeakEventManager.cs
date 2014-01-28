using Common.UI.WeakEvent;

using GU.MZ.UI.ViewModel.SupervisionViewModel.Interface;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel.Event
{
    /// <summary>
    /// Менеждер слабых событий <see cref="ViewModelRebuildRequested"/>.
    /// </summary>
    /// <remarks>
    /// Использовать для источников реализующих <c>ISupervisionStepVM</c>.
    /// </remarks>
    public class VmRebuildRequestedWeakEventManager : WeakEventManagerBase<VmRebuildRequestedWeakEventManager, IRebuildRequestPublisher>
    {
        protected override void StartListening(IRebuildRequestPublisher source)
        {
            source.RebuildRequested += DeliverEvent;
        }

        protected override void StopListening(IRebuildRequestPublisher source)
        {
            source.RebuildRequested -= DeliverEvent;
        }
    }
}
