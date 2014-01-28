using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

using Common.BL.DataMapping;
using Common.DA;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel
{
    /// <summary>
    /// Базовый класс классов ViewModel рабочих областей, предназначенных для редактирования доменного объекта.  
    /// </summary>
    /// <typeparam name="T">Тип редактируемого объекта</typeparam>
    /// <remarks>
    /// Класс предназначен для рабочей области(вкладки, окна) редактирования данных одного сложного доменного объекта.
    /// Внесение изменений в редактируемый объект может быть заблокировано флажком _isEditable.
    /// Класс предоставляет команду сохранения изменений в редактируемом объекте.
    /// Класс предоставляет флажок IsDirty указывающий на наличие несохранённых изменений в редактируемом объекте.
    /// Для выставления IsDirty необходимо подписываться на события PropertyChanged редактируемого объекта.
    /// Так как подразумевается редактирование сложного объекта с редактируемыми ассоциацмиями один-ко-многим,
    /// подписка на событие PropertyChanged происходит через слабые события, а обход объект осуществляется специальным подписчиком _eventSubscriber.
    /// </remarks>
    public abstract class EditableVM<T> : NotificationObject, IEditableVM<T>, IAvalonDockCaller where T : DomainObject<T>, IPersistentObject
    {
        /// <summary>
        /// Маппер доменного объекта
        /// </summary>
        protected readonly IDomainDataMapper<T> _dataMapper;

        /// <summary>
        /// Подписчик слабых событий на PropertyChanged редактируемого объекта.
        /// </summary>
        private readonly IDomainObjectEventSubscriber<T> _eventSubscriber;

        /// <summary>
        /// Слушатель слабых событий на PropertyChanged редактируемого объекта.
        /// </summary>
        private readonly WeakEventListener<EventArgs> _weakListener;

        /// <summary>
        /// Редактируемый доменный объект.
        /// </summary>
        private T _entity;

        /// <summary>
        /// Возвращает или устанавливает редактируемый доменный объект.
        /// </summary>
        public T Entity 
        {
            get
            {
                return _entity;
            }
            set
            {
                if (_entity == null || !_entity.Equals(value))
                {
                    _entity = value;
                    this._eventSubscriber.PropertyChangedSubscribe(_entity, this._weakListener);
                }
            }
        }

        /// <summary>
        /// Флаг, указывающий на возможность внесения изменений в редактируемый объект.
        /// </summary>
        protected bool _isEditable;

        /// <summary>
        /// Базовый класс классов ViewModel рабочих областей, предназначенных для редактирования доменного объекта.  
        /// </summary>
        /// <param name="entity">Редактируемый доменный объект</param>
        /// <param name="eventSubscriber">Подписчик слабых событий на PropertyChanged редактируемого объекта</param>
        /// <param name="dataMapper"> Маппер доменного объекта</param>
        /// <param name="isEditable">Флаг, указывающий на возможность внесения изменений в редактируемый объект</param>
        protected EditableVM(T entity, 
                             IDomainObjectEventSubscriber<T> eventSubscriber,
                             IDomainDataMapper<T> dataMapper,
                             bool isEditable = false)
        {   
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            _weakListener = new WeakEventListener<EventArgs>(OnEditableSourcePropertyChanged);
            _eventSubscriber = eventSubscriber;
            _dataMapper = dataMapper;
            _isEditable = isEditable; 
            Entity = entity;
            SaveCommand = new DelegateCommand(Save);
            Rebuild();
        }

        /// <summary>
        /// Перегрузка для реализаций EditableVm в MZ
        /// </summary>
        protected EditableVM(T entity,
                             IDomainObjectEventSubscriber<T> eventSubscriber,
                             IDomainDataMapper<T> dataMapper,
                             object justForMz,
                             bool isEditable = false)
        {
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            _weakListener = new WeakEventListener<EventArgs>(OnEditableSourcePropertyChanged);
            _eventSubscriber = eventSubscriber;
            _dataMapper = dataMapper;
            _isEditable = isEditable;
            Entity = entity;
            SaveCommand = new DelegateCommand(Save);
        }

        /// <summary>
        /// Возвращает тип редактируемого объекта.
        /// </summary>
        /// <returns>Тип редактируемого объекта</returns>
        public Type GetEntityType()
        {
            return typeof(T);
        }

        /// <summary>
        /// Обрабатывает событие внесения изменений в редактируемый объект.
        /// </summary>
        /// <param name="sender">Хозяин события</param>
        /// <param name="args">Аргументы события</param>
        protected void OnEditableSourcePropertyChanged(object sender, EventArgs args)
        {
            RaiseIsDirtyChanged();
            if (args is NotifyCollectionChangedEventArgs)
            {
                var collArgs = args as NotifyCollectionChangedEventArgs;
                if (collArgs.NewItems != null)
                {
                    foreach (var item in collArgs.NewItems)
                    {
                        if (item is INotifyPropertyChanged)
                        {
                            PropertyChangedWeakEventManager.AddListener(item as INotifyPropertyChanged, _weakListener);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает значение первичного ключа сущности.
        /// </summary>
        /// <returns>Значение первичного ключа сущности</returns>
        public string GetEntityKeyValue()
        {
            return Entity.GetKeyValue();
        }

        /// <summary>
        /// Обрабатывает запрос на закрытие рабочей области редактирования. Возвращает флаг, указывающий на возможность закрытия области.
        /// </summary>
        /// <param name="displayName">Отображаемое имя сущности</param>
        /// <returns>Флаг, указывающий на возможность закрытия области</returns>
        public bool OnClosing(string displayName)
        {
            if (IsDirty)
            {
                var mbr = NoticeUser.ShowQuestionYesNoCancel(string.Format("Сохранить изменения в {0}?", displayName));
                switch (mbr)
                {
                    case MessageBoxResult.Yes:
                        {
                            Save();
                            break;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                    case MessageBoxResult.Cancel:
                        {
                            return false;
                        }
                    default:
                        {
                            throw new NotSupportedException();
                        }
                }
            }
            return true;
        }

        /// <summary>
        /// Пересобирает поля привязки.
        /// </summary>
        protected virtual void Rebuild()
        {
 
        }

        #region Events

        /// <summary>
        /// Событие, оповещающее об изменении флага IsDirty.
        /// </summary>
        public event Action IsDirtyChanged;

        /// <summary>
        /// Событие, оповещающее об изменении отображаемого имени.
        /// </summary>
        public event Action<string> DisplayNameChanged;

        /// <summary>
        /// Возбуждает событие IsDirtyChanged.
        /// </summary>
        public void RaiseIsDirtyChanged()
        {
            if (IsDirtyChanged != null)
            {
                IsDirtyChanged.Invoke();
            }
        }

        /// <summary>
        /// Возбуждает событие DisplayNameChanged.
        /// </summary>
        /// <param name="newDisplayName">Новое имя</param>
        protected void RaiseDisplayNameChanged(string newDisplayName)
        {
            if (DisplayNameChanged != null)
            {
                DisplayNameChanged.Invoke(newDisplayName);
            }
        }
        
        #endregion

        #region Binding Properties

        /// <summary>
        /// Возвращает флаг, указывающий на наличие несохранённых изменений в редактируемом объекте.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return Entity.IsDirty;
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда сохранения изменений в редактируемом объекте.
        /// </summary>
        public DelegateCommand SaveCommand { get; protected set; }

        /// <summary>
        /// Сохраняет изменения в редактируемом объекте.
        /// </summary>
        protected virtual void Save()
        {
            try
            {
                Entity = _dataMapper.Save(Entity);
                Rebuild();
                RaiseDisplayNameChanged(Entity.ToString());
                RaiseIsDirtyChanged();
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new VMException("Непредвиденная ошибка", ex));
            }
        }

        #endregion        

        #region IAvalonDockCaller
        
        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion
    }

}
