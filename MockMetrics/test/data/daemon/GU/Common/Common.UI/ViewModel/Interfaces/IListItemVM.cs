using System.ComponentModel;
using Common.DA.Interface;
using Common.UI.ViewModel.Event;
using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс классов моделей-представления для отображения доменного объекта в списке.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта</typeparam>
    public interface IListItemVM<T> : IListItemVM, IValidateableVM where T : IDomainObject
    {
        /// <summary>
        /// Возвращает или устанавливает отображаемый объект.
        /// </summary>
        T Item { get; set; }
    }

    /// <summary>
    /// Интерфейс классов моделей-представления для отображения элемента списка.
    /// </summary>
    public interface IListItemVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Обрабатывает событие изменения отображаемых данных объекта. 
        /// </summary>
        void OnDisplayPropertyChanged();

        #region Event

        /// <summary>
        /// Событие запроса на "открытие" элемента списка.
        /// </summary>
        event ChooseItemRequestedDelegate OpenItemRequested;

        /// <summary>
        /// Событие запроса на удаление элемента списка.
        /// </summary>
        event ChooseItemRequestedDelegate RemoveItemRequested;

        /// <summary>
        /// Событие выделения элемента списка.
        /// </summary>
        event ChooseItemRequestedDelegate SelectItemRequested;

        /// <summary>
        /// Событие запроса на копирование элемента списка.
        /// </summary>
        event ChooseItemRequestedDelegate CopyItemRequested;

        #endregion

        #region Binding Commands

        /// <summary>
        /// Вовзращает команду "открытия" элемента списка.
        /// </summary>
        DelegateCommand OpenItemCommand { get; }

        /// <summary>
        /// Вовзращает команду удаления элемента списка.
        /// </summary>
        DelegateCommand RemoveItemCommand { get; }

        /// <summary>
        /// Вовзращает команду выделения элемента списка.
        /// </summary>
        DelegateCommand SelectItemCommand { get; }

        /// <summary>
        /// Вовзращает команду копирования элемента списка.
        /// </summary>
        DelegateCommand CopyItemCommand { get; }

        #endregion
    }
}
