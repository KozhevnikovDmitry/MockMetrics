using System;
using System.Windows;
using Common.BL.Validation;
using Common.DA;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public abstract class SmartEditableVm<T> : NotificationObject, ISmartEditableVm<T>, IAvalonDockCaller
        where T : DomainObject<T>, IPersistentObject
    {
        private T _entity;

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
                    _editableFacade.Resubscribe<T>(this);
                }
            }
        }

        protected IEditableFacade _editableFacade;

        public bool IsEditable { get; set; }

        protected SmartEditableVm()
        {
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            SaveCommand = new DelegateCommand(Save);
            OnCloseCommand = new DelegateCommand(OnClose);
        }

        public virtual void Initialize(T entity, IEditableFacade editableFacade, bool isEditable = false)
        {
            _editableFacade = editableFacade;
            _editableFacade.Register(this, entity);
            IsEditable = isEditable;
            Entity = entity;
            Rebuild();
        }
        
        public Type GetEntityType()
        {
            return typeof(T);
        }

        public string GetEntityKeyValue()
        {
            return Entity.GetKeyValue();
        }

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

        protected virtual void Rebuild()
        {

        }

        #region Events

        public event Action IsDirtyChanged;
        public event Action<string> DisplayNameChanged;

        public void RaiseIsDirtyChanged()
        {
            if (IsDirtyChanged != null)
            {
                IsDirtyChanged.Invoke();
            }
        }

        protected void RaiseDisplayNameChanged(string newDisplayName)
        {
            if (DisplayNameChanged != null)
            {
                DisplayNameChanged.Invoke(newDisplayName);
            }
        }

        #endregion

        #region Binding Properties

        public bool IsDirty
        {
            get
            {
                return Entity.IsDirty;
            }
        }

        #endregion

        #region Binding Commands
        
        public DelegateCommand OnCloseCommand { get; protected set; }
        
        protected virtual void OnClose()
        {
            try
            {
                _editableFacade.Close(this);
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

        public DelegateCommand SaveCommand { get; protected set; }

        protected virtual void Save()
        {
            try
            {
                _editableFacade.Save<T>(this);
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

        protected ValidationErrorInfo Validate()
        {
            return _editableFacade.Validate<T>(this);
        }

        #endregion

        #region IAvalonDockCaller

        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion

    }
}
