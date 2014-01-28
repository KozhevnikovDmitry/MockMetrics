using GU.MZ.UI.ViewModel.SupervisionViewModel.Event;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel.Interface
{
    /// <summary>
    /// Интерфес классов способных делать запрос на ребилд vm'a редактирования тома
    /// </summary>
    public interface IRebuildRequestPublisher
    {
        /// <summary>
        /// Событие запроса на пересобирание vm'a редактирования тома
        /// </summary>
        event ViewModelRebuildRequested RebuildRequested;
    }
}