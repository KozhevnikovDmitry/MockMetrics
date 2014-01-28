using System;

using Common.UI.ViewModel.Interfaces;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.ValidationViewModel
{
    /// <summary>
    /// Абстрактный базовый класс Моделей Представления с функционалом отображения валидации данных.
    /// </summary>
    public abstract class ValidateableVM : NotificationObject, IValidateableVM
    {
        /// <summary>
        /// Абстрактный базовый класс Моделей Представления с функционалом валидации данных.
        /// </summary>
        /// <param name="isValidateable">Флаг управляющий доступностb функций валидации</param>
        protected ValidateableVM(bool isValidateable = false)
        {
            this._isValidateable = isValidateable;
            this.ReadyToValidateCommand = new DelegateCommand(this.ReadyToValidate);
            this.NotReadyToValidateCommand = new DelegateCommand(this.NotReadyToValidate);
        }

        /// <summary>
        /// Оповещает представление об обновлении данных по всем валидируемым полям.
        /// </summary>
        /// <remarks>
        /// Используется для обновления индикаторов валидности полей при смене используемых наборов валидации.
        /// </remarks>
        public virtual void RaiseValidatingPropertyChanged()
        {

        }

        /// <summary>
        /// Оповещает модель представления об изменении значения флага <c>IsValid</c> Модели-Представления.
        /// </summary>
        public virtual void RaiseIsValidChanged()
        {
            this.RaisePropertyChanged(() => this.IsValid);
            this.RaiseValidatingPropertyChanged();
        }

        /// <summary>
        /// Флаг управляющий доступностью функций валидации. Если значение <c>true</c> валидация должна быть доступна.
        /// </summary>
        protected bool _isValidateable;

        #region Binding Properties

        /// <summary>
        /// Возвращает флаг валидности объекта Модели Представления.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                if (!this.AllowValidate)
                {
                    return true;
                }

                return true;
            }
        }

        /// <summary>
        /// Флаг разрещающий или запрещающий проведение валидации.
        /// </summary>
        public bool AllowValidate { get; set; }

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
        protected virtual void ReadyToValidate()
        {
             AllowValidate = _isValidateable;
        }

        /// <summary>
        /// Запрещает валидацию.
        /// </summary>
        protected virtual void NotReadyToValidate()
        {
            this.AllowValidate = false;
        }

        #endregion

        #region IDataErrorInfo

        /// <summary>
        /// Не используется в WPF
        /// </summary>
        [Obsolete]
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Возвращает сообщение о нарушении правил валидации для поля привязки либо null, если значение поля валидно. 
        /// </summary>
        /// <param name="columnName">Имя поля привязки</param>
        /// <returns>Сообщение о нарушении правил валидации</returns>
        public virtual string this[string columnName]
        {
            get
            {
                return null;
            }
        }

        #endregion

    }
}
