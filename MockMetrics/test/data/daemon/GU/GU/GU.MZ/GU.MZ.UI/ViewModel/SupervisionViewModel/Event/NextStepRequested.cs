using System;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel.Event
{
    /// <summary>
    /// Делегат событий запроса на переход к следующему этапу ведения
    /// </summary>
    public delegate void NextStepRequested(object sender, EventArgs eventArgs);
}
