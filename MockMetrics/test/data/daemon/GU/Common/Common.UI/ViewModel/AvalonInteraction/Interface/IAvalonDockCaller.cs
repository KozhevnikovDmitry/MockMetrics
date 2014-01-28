namespace Common.UI.ViewModel.AvalonInteraction.Interface
{
    /// <summary>
    /// Интерфейс для классов, которым необходимо делать запросы на создание новых вкладок AvalonDock. 
    /// </summary>
    /// <remarks>
    /// Композиция ViewModel'ов, реализующих данный интерфейс образуют цепочку обязанностей.
    /// События на открытие вкладки прокидываются по цепочке до нужного AvalonDockVM, который обрабатывает событие и открывает нужную вкладку.
    /// События, обработчики и регистрация дочерних сaller'ов делегированы объекту IAvalonDockInteractor
    /// </remarks>
    public interface IAvalonDockCaller
    {
        /// <summary>
        /// Объект предназанченный для взаимодействия с AvalonDockVM
        /// </summary>
        IAvalonDockInteractor AvalonInteractor { get; }
    }
}
