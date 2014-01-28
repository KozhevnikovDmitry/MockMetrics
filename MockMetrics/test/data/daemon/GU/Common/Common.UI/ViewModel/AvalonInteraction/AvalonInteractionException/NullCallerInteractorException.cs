using Common.Types.Exceptions;

namespace Common.UI.ViewModel.AvalonInteraction.AvalonInteractionException
{
    /// <summary>
    /// Класс исключение для ошибок "Объект для взавимодействия с AvalonDock не задан."
    /// </summary>
    public class NullCallerInteractorException : GUException
    {
        public NullCallerInteractorException()
            :base("Объект для взавимодействия с AvalonDock не задан.")
        {
            
        }
    }
}