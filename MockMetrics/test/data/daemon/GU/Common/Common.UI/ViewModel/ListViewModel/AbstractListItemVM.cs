using Common.BL.Validation;
using Common.DA.Interface;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.ValidationViewModel;

using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.ListViewModel
{
    /// <summary>
    /// Базовый класс классов ViewModel для отображения данных доменного объекта в элементе списка.
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public abstract class AbstractListItemVM<T> : DomainValidateableVM<T>, IListItemVM<T> where T : IDomainObject
    {
        protected AbstractListItemVM(T entity, bool isValidateable)
            : base(entity, new StubDomainValidatior<T>(), isValidateable)
        {
            OpenItemCommand = new DelegateCommand(OpenItem, CanOpenItem);
            RemoveItemCommand = new DelegateCommand(RemoveItem, CanRemoveItem);
            SelectItemCommand = new DelegateCommand(SelectItem, CanSelectItem);
            CopyItemCommand = new DelegateCommand(CopyItem, CanCopyItem);
            Initialize();
        }

        protected AbstractListItemVM(T entity, IDomainValidator<T> domainValidator, bool isValidateable)
            : base(entity, domainValidator, isValidateable)
        {
            OpenItemCommand = new DelegateCommand(OpenItem, CanOpenItem);
            RemoveItemCommand = new DelegateCommand(RemoveItem, CanRemoveItem);
            SelectItemCommand = new DelegateCommand(SelectItem, CanSelectItem);
            CopyItemCommand = new DelegateCommand(CopyItem, CanCopyItem);
            Initialize();
        }

        /// <summary>
        /// Инициализирует поля привязки.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Обрабатывает событие изменения отображаемых данных объекта. 
        /// </summary>
        public virtual void OnDisplayPropertyChanged()
        {
            Initialize();
        }

        /// <summary>
        /// Возвращает или устанавливает отображаемый доменный объект.
        /// </summary>
        public T Item 
        {
            get
            {
                return this.Entity;
            }
            set
            {
                if (this.Entity == null || !this.Entity.Equals(value))
                {
                    this.Entity = value;
                    Initialize();
                }
            }
        }

        #region Event

        /// <summary>
        /// Событие запроса на "открытие" элемента списка.
        /// </summary>
        public event ChooseItemRequestedDelegate OpenItemRequested;

        /// <summary>
        /// Событие запроса на удаление элемента списка.
        /// </summary>
        public event ChooseItemRequestedDelegate RemoveItemRequested;

        /// <summary>
        /// Событие выделения элемента списка.
        /// </summary>
        public event ChooseItemRequestedDelegate SelectItemRequested;

        /// <summary>
        /// Событие запроса на копирование элемента списка.
        /// </summary>
        public event ChooseItemRequestedDelegate CopyItemRequested;

        #endregion

        #region Binding Commands

        /// <summary>
        /// Вовзращает команду "открытия" элемента списка.
        /// </summary>
        public DelegateCommand OpenItemCommand { get; protected set; }

        /// <summary>
        /// Вовзращает команду удаления элемента списка.
        /// </summary>
        public DelegateCommand RemoveItemCommand { get; protected set; }

        /// <summary>
        /// Вовзращает команду выделения элемента списка.
        /// </summary>
        public DelegateCommand SelectItemCommand { get; protected set; }

        /// <summary>
        /// Вовзращает команду копирования элемента списка.
        /// </summary>
        public DelegateCommand CopyItemCommand { get; protected set; }

        /// <summary>
        /// Открывает элемент списка.
        /// </summary>
        protected virtual void OpenItem()
        {
            if (OpenItemRequested != null)
            {
                OpenItemRequested(this, new ChooseItemRequestedEventArgs(Item));
            }
        }

        /// <summary>
        /// Удаляет элемент списка.
        /// </summary>
        protected virtual void RemoveItem()
        {
            if (RemoveItemRequested != null)
            {
                RemoveItemRequested(this, new ChooseItemRequestedEventArgs(Item));
            }
        }

        /// <summary>
        /// Выделяет элемент списка.
        /// </summary>
        protected virtual void SelectItem()
        {
            if (SelectItemRequested != null)
            {
                SelectItemRequested(this, new ChooseItemRequestedEventArgs(Item));
            }
        }

        /// <summary>
        /// Добавляет копию элемента списка.
        /// </summary>
        protected virtual void CopyItem()
        {
            if (CopyItemRequested != null)
            {
                CopyItemRequested(this, new ChooseItemRequestedEventArgs(Item));
            }
        }

        /// <summary>
        /// Возвращает флаг возможности открытия элемента списка.
        /// </summary>
        /// <returns>Флаг возможности открытия элемента списка</returns>
        protected virtual bool CanOpenItem()
        {
            return OpenItemRequested != null;
        }

        /// <summary>
        /// Возвращает флаг возможности удаления элемента списка.
        /// </summary>
        /// <returns>Флаг возможности удаления элемента списка</returns>
        protected virtual bool CanRemoveItem()
        {
            return RemoveItemRequested != null;
        }

        /// <summary>
        /// Возвращает флаг возможности выделения элемента списка.
        /// </summary>
        /// <returns>Флаг возможности выделения элемента списка</returns>
        protected virtual bool CanSelectItem()
        {
            return SelectItemRequested != null;
        }

        /// <summary>
        /// Возвращает флаг возможности добавления копии элемента списка.
        /// </summary>
        /// <returns>Флаг возможности добавления копии элемента списка</returns>
        protected virtual bool CanCopyItem()
        {
            return CopyItemRequested != null;
        }

        #endregion
    }
}
