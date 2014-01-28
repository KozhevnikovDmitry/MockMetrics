using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Common.DA.Interface;
using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс классов Моделей-представления для работы со списком доменных объектов.
    /// </summary>
    /// <typeparam name="T">Тип доменных объектов</typeparam>
    public interface IListVM<T> : INotifyPropertyChanged, IValidateableVM where T : IDomainObject
    { 
        #region Binding Properties

        /// <summary>
        /// Возвращает наименование списка.
        /// </summary>
        string Title { get;}

        /// <summary>
        /// Возвращает коллекцию моделей-представления отображаемых объектов
        /// </summary>
        ObservableCollection<IListItemVM<T>> ListItemVMs { get; }

        /// <summary>
        /// Возвращает или устанавливает значение выделенного объекта 
        /// </summary>
        IListItemVM<T> SelectedItem { get; set; }

        /// <summary>
        /// Вовзвращает шаблон отображения элеметна в списке.
        /// </summary>
        DataTemplate ListItemTemplate { get; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Возвращает команду добавления нового элемента списка.
        /// </summary>
        DelegateCommand AddItemCommand { get; }

        /// <summary>
        /// Возвращает команду удаления элемента из списка.
        /// </summary>
        DelegateCommand RemoveItemCommand { get; }

        #endregion

        #region Validateable

        /// <summary>
        /// Оповещает представление об обновлении данных по всем валидируемым полям объектов в списке.
        /// </summary>
        void RaiseItemsValidatingPropertyChanged();

        #endregion
    }
}
