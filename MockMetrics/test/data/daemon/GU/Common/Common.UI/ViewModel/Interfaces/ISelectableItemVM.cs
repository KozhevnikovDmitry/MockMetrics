using Common.UI.ViewModel.Event;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс классов моделей-представления для "выделяемых" или "выбираемых" отображаемых элементов.
    /// </summary>
    public interface ISelectableItemVM
    {
        /// <summary>
        /// Возвращает флаг выделенности элемента. 
        /// </summary>
        bool IsSelected { get; }

        /// <summary>
        /// Событие запроса на выбор элемента.
        /// </summary>
        event ChooseItemRequestedDelegate ChooseResultRequested;

        /// <summary>
        /// Событие выделения элемента.
        /// </summary>
        event ChooseItemRequestedDelegate SelectedResultChanged;
    }
}
