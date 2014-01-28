using System.ComponentModel;
using System.Windows.Controls;
using Common.DA.Interface;

using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс классов моделей-представления
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISearchResultVM<T> : INotifyPropertyChanged, ISelectableItemVM where T : IDomainObject
    {       
        #region Binding Properties

        /// <summary>
        /// Возвращает отображаемый доменный объект - результат поиска.
        /// </summary>
        T Result { get; }

        /// <summary>
        /// Вовзращает или устанавливает селектор шаблонов отображения для результата поиска.
        /// </summary>
        DataTemplateSelector ResultItemTemplateSelector { get; set; }
        
        #endregion

        #region Binding Commands

        /// <summary>
        /// Возвращает команду "выбора" отображаемого объекта как результирующего для всего поиска.
        /// </summary>
        DelegateCommand ChooseItemCommand { get; }
        
        #endregion
    }
}
