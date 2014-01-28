using System;
using System.Collections.Generic;
using System.Linq;
using Common.DA.Interface;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public abstract class SmartValidateableVm<T> : NotificationObject, ISmartValidateableVm<T> where T : IDomainObject
    {
        public T Entity { get; protected set; }

        private IValidateFacade _validateFacade;
        
        protected SmartValidateableVm()
        {
            _isValidateable = true;
            IsEditable = true;
            AllowHighlight = false;
            AllowValidate = false;
            ReadyToValidateCommand = new DelegateCommand(ReadyToValidate);
            NotReadyToValidateCommand = new DelegateCommand(NotReadyToValidate);
        }


        #region Children

        /// <summary>
        /// Список дочерних vm'ов для которых можно дублировать вызовы родительского
        /// Подразумевается, что дочерние vm не пересоздаются, а только переинициализируются
        /// </summary>
        private readonly List<ISmartValidateableVm> _childs = new List<ISmartValidateableVm>();

        protected void AddChild(ISmartValidateableVm childVm)
        {
            _childs.Add(childVm);
        }

        private void ChildrenDo(Action<ISmartValidateableVm> action)
        {
            foreach (var childVm in _childs.Where(t => t.IsInitialized))
            {
                action(childVm);
            }
        }

        #endregion


        #region Initialize

        public void SetFacade(IValidateFacade validateFacade)
        {
            if (validateFacade == null) 
                throw new ArgumentNullException("validateFacade");

            _validateFacade = validateFacade;
        }

        public bool IsInitialized {
            get { return _validateFacade != null && Entity != null; }
        }

        /// <summary>
        /// Меняет отображаемую сущность, запрещает подсветку валидации, поднимает все валидируемые поля
        /// </summary>
        public virtual void Initialize(T entity)
        {
            Entity = entity;
            AllowHighlight = false;
            RaiseAllPropertyChanged();
        }

        #endregion


        #region Flags

        private bool _isValidateable;

        public bool IsEditable { get; set; }

        public bool AllowHighlight { get; set; }

        public bool AllowValidate { get; set; }

        public bool IsValid
        {
            get
            {
                if (!AllowValidate)
                {
                    return true;
                }

                return _validateFacade.Validate<T>(this).IsValid;
            }
        }

        #endregion


        #region Raise Property Changed

        /// <summary>
        /// Просто опубличенный метод из INotifyPropertyChanged
        /// </summary>
        public void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);
        }

        public virtual void RaiseAllPropertyChanged()
        {
            _validateFacade.RaiseValidatingPropertyChanged<T>(this);
            ChildrenDo(vm => vm.RaiseAllPropertyChanged());
        }

        /// <summary>
        /// Поднимает все валидируемые свойства для Vm'а
        /// </summary>
        public virtual void RaiseValidatingPropertyChanged()
        {
            _validateFacade.RaiseValidatingPropertyChanged<T>(this);
            ChildrenDo(vm => vm.RaiseValidatingPropertyChanged());
        }

        /// <summary>
        /// Разрешает подсветку полей, поднимает все валидируемые свойства и флаг IsValid
        /// </summary>
        public void RaiseIsValidChanged()
        {
            if (Entity == null)
            {
                return;
            }

            AllowHighlight = true;
            RaisePropertyChanged(() => IsValid);
            RaiseValidatingPropertyChanged();
            ChildrenDo(vm => vm.RaiseIsValidChanged());
        }

        #endregion


        #region Binding Commands

        /// <summary>
        /// Объект команда разрешеающий валидацию полей Модели-Представления.
        /// </summary>
        public DelegateCommand ReadyToValidateCommand { get; set; }

        /// <summary>
        /// Объект команда запрещащий валидацию полей Модели-Представления.
        /// </summary>
        public DelegateCommand NotReadyToValidateCommand { get; set; }

        /// <summary>
        /// Разрешает валидацию.
        /// </summary>
        public virtual void ReadyToValidate()
        {
            AllowValidate = _isValidateable;
            ChildrenDo(vm => vm.ReadyToValidate());
        }

        /// <summary>
        /// Запрещает валидацию.
        /// </summary>
        public virtual void NotReadyToValidate()
        {
            AllowValidate = false;
            ChildrenDo(vm => vm.NotReadyToValidate());
        }

        #endregion


        #region IDataErrorInfo
        
        [Obsolete]
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Валидирует и подсвечивает (невалидное) поле в том случае если разрешена валидация вообще и подсветка полей
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                if (!AllowValidate || !AllowHighlight)
                {
                    return null;
                }

                return _validateFacade.Validate<T>(this, columnName);
            }
        }
        
        #endregion
    }
}
