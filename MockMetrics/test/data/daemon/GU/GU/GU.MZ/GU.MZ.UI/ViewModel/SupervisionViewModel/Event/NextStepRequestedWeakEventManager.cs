using Common.UI.WeakEvent;

using GU.MZ.UI.ViewModel.SupervisionViewModel.Interface;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel.Event
{
    /// <summary>
    /// Менеждер слабых событий <see cref="NextStepRequested"/>.
    /// </summary>
    /// <remarks>
    /// Использовать для источников реализующих <c>ISupervisionStepVM</c>.
    /// </remarks>
    public class NextStepRequestedWeakEventManager : WeakEventManagerBase<NextStepRequestedWeakEventManager, ISupervisionStepVm>
    {
        protected override void StartListening(ISupervisionStepVm source)
        {
            source.StepNextRequested += DeliverEvent;
        }

        protected override void StopListening(ISupervisionStepVm source)
        {
            source.StepNextRequested -= DeliverEvent;
        }
    }
}
