using System.Collections.ObjectModel;
using System.ComponentModel;

using Common.DA.Interface;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Event;

using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс для классов Моделей-Представления поиска доменных объектов по базе данных. 
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public interface ISearchVM<T> : ISearchVM where T : IDomainObject
    {
        /// <summary>
        /// Возвращает доменныый объект - результат поиска.
        /// </summary>
        T Result { get; }

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        IAvalonDockInteractor AvalonInteractor { get; }


        #region Binding Properties

        /// <summary>
        /// Возвращает список моделей-представления для доменных объектов страницы результатов поиска.
        /// </summary>
        ObservableCollection<ISearchResultVM<T>> ResultItemsVMList { get; }

        #endregion
    }

    /// <summary>
    /// Интерфейс для классов Моделей-Представления поиска сущностей по базе данных. 
    /// </summary>
    public interface ISearchVM : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// Событие запроса на выбор одной сущности как результата поиска.
        /// </summary>
        event ChooseItemRequestedDelegate ChooseResultRequested;

        #endregion

        #region Binding Properties

        /// <summary>
        /// Возвращает флаг отмены поиска.
        /// </summary>
        bool IsCancelationRequested { get; }

        /// <summary>
        /// Возвращает или устанавливает флаг указывающий на незавершённость процесса поиска.
        /// </summary>
        bool IsLoadingProgressing { get; set; }

        /// <summary>
        /// Возвращает или устанавливает флаг указывающий на открытость панели поиска.
        /// </summary>
        bool IsSearchOpenned { get; set; }

        /// <summary>
        /// Возвращает или устанавливает номер текущей страницы результатов поиска.
        /// </summary>
        int SearchPage { get; set; }

        /// <summary>
        /// Возвращает или устанавливает информацию о результатах поиска.
        /// </summary>
        string SearchResultInfo { get; set; }

        /// <summary>
        /// Коллекция моделей-представления для вкладок панели поиска.
        /// </summary>
        ObservableCollection<ISearchTemplateVM> SearchTemplateVMs { get; set; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Возвращает команду отмены поиска.
        /// </summary>
        DelegateCommand CancelOperationCommand { get; }

        /// <summary>
        /// Возвращает команду инициирования поиска.
        /// </summary>
        DelegateCommand<ISearchTemplateVM> SearchCommand { get; }

        /// <summary>
        /// Возвращает команду инициирования поиска следующей страницы данных.
        /// </summary>
        DelegateCommand<ISearchTemplateVM> SearchNextPageCommand { get; }

        /// <summary>
        /// Возвращает команду инициирования поиска предыдущей страницы данных.
        /// </summary>
        DelegateCommand<ISearchTemplateVM> SearchPreviousPageCommand { get; }

        #endregion
    }
}
