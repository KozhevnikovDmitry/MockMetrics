using System;
using System.Windows.Controls;

using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.Interfaces;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.SearchViewModel
{
    /// <summary>
    /// Абстрактный ViewModel для отображения данных доменного объекта в элементе списка поиска доменных объектов.
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public abstract class AbstractSearchResultVM<T> : NotificationObject, ISearchResultVM<T> where T :  IDomainObject
    {
        /// <summary>
        /// Абстрактный ViewModel для отображения данных доменного объекта в элементе списка поиска доменных объектов.
        /// </summary>
        /// <param name="entity">Доменный объект</param>
        protected AbstractSearchResultVM(T entity)
        {
            ChooseItemCommand = new DelegateCommand(ChooseItem);
            Result = entity;
            if (Result != null)
                Initialize();
        }

        /// <summary>
        /// Перегрузка для реализаций AbstractSearchResultVM в MZ
        /// </summary>
        protected AbstractSearchResultVM(T entity, object justForMz)
        {
            ChooseItemCommand = new DelegateCommand(ChooseItem);
            Result = entity;
        }

        /// <summary>
        /// Отображаемый объект
        /// </summary>
        public T Result {get; private set;}

        /// <summary>
        /// Инициализирует поля привязки VM'а
        /// </summary>
        protected virtual void Initialize()
        {
            ResultItemTemplateSelector = new ResultItemTemplateSelector(Result.GetType().Name);
        }

        #region Events

        /// <summary>
        /// Событие оповещающее о том, что выбран объект в списке поиска.
        /// </summary>
        public event ChooseItemRequestedDelegate ChooseResultRequested;

        /// <summary>
        /// Событие оповещающее о том, что выделен объект в списке поиска.
        /// </summary>
        public event ChooseItemRequestedDelegate SelectedResultChanged;

        /// <summary>
        /// Возбуждает событие оповещающее о том, что выделен объект в списке поиска.
        /// </summary>
        /// <param name="result">Доменный объект</param>
        protected void RaiseSelectedResultChanged(T result)
        {
            if (SelectedResultChanged != null)
            {
                SelectedResultChanged(this, new ChooseItemRequestedEventArgs(result));
            }
        }
        
        #endregion

        #region Binding Properties

        /// <summary>
        /// Селектор шаблонов отображения данных VM'а на View
        /// </summary>
        private DataTemplateSelector _resultItemTemplateSelector;

        /// <summary>
        /// Возвращает или устанавливает селектор шаблонов отображения данных VM'а на View
        /// </summary>
        public DataTemplateSelector ResultItemTemplateSelector
        {
            get
            {
                return _resultItemTemplateSelector;
            }
            set
            {
                if (_resultItemTemplateSelector == null || !_resultItemTemplateSelector.Equals(value))
                {
                    _resultItemTemplateSelector = value;
                    RaisePropertyChanged(() => ResultItemTemplateSelector);
                }
            }
        }

        /// <summary>
        /// Флаг выделенности элемента
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// Возвращает или устанавливает флаг выделенности элемента
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    ResultItemTemplateSelector = new ResultItemTemplateSelector(Result.GetType().Name);
                    if (_isSelected)
                    {
                        RaiseSelectedResultChanged(Result);
                    }
                    RaisePropertyChanged(() => IsSelected);
                }
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда выбора данного элемента в списке поиска доменных объектов
        /// </summary>
        public DelegateCommand ChooseItemCommand { get; protected set; }

        /// <summary>
        /// Возбуждает событие оповещающее о том, что выбран объект в списке поиска.
        /// </summary>
        protected void ChooseItem()
        {
            try
            {
                if (this.ChooseResultRequested != null)
                {
                    this.ChooseResultRequested(this, new ChooseItemRequestedEventArgs(Result));
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }

        #endregion

    }
}
