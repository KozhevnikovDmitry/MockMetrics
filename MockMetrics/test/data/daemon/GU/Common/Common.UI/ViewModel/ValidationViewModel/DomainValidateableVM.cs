using Common.BL.Validation;
using Common.DA.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.ValidationViewModel
{
    /// <summary>
    /// Базовый класс для ViewModel'ов с функционалом отображения валидации полей доменных объектов
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public abstract class DomainValidateableVM<T> : ValidateableVM, IDomainValidateableVM<T>
        where T : IDomainObject
    {
        /// <summary>
        /// Валидируемый доменный объект.
        /// </summary>
        public T Entity { get; protected set; }

        /// <summary>
        /// Флаг возможности редактирования.
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// Валидатор доменного объекта
        /// </summary>
        protected readonly IDomainValidator<T> _domainValidator;

        /// <summary>
        /// Базовый класс для ViewModel'ов с функционалом отображения валидации полей доменных объектов
        /// </summary>
        /// <param name="entity">Валидируемый доменный объект</param>
        /// <param name="domainValidator">Валидатор доменного объекта</param>
        /// <param name="isValidateable">Флаг возможности валидации</param>
        protected DomainValidateableVM(T entity, IDomainValidator<T> domainValidator, bool isValidateable = true)
            :base(isValidateable)
        {
            IsEditable = true;
            this.Entity = entity;
            _domainValidator = domainValidator;
            _domainValidator.AllowSinglePropertyValidate = false;
        }

        /// <summary>
        /// Возвращает сообщение о нарушении правил валидации для поля привязки либо null, если значение поля валидно. 
        /// </summary>
        /// <param name="columnName">Имя поля привязки</param>
        /// <returns>Сообщение о нарушении правил валидации</returns>
        /// <remarks>
        /// columnName должно быть именем свойства доменного объекта. 
        /// То есть поле привязки, отображающее свойтсво, и само свойство должны называться одинаково.
        /// </remarks>
        public override string this[string columnName]
        {
            get
            {
                if (!this.AllowValidate)
                {
                    return null;
                }

                return _domainValidator.ValidateProperty(this.Entity, columnName);
            }
        }

        /// <summary>
        /// Генерирует события для обновления валидируемых полей привязки согласно валидатору.
        /// </summary>
        public override void RaiseValidatingPropertyChanged()
        {
            foreach (var validatedName in _domainValidator.ValidatedNames)
            {
                this.RaisePropertyChanged(validatedName);
            }
        }

        /// <summary>
        /// Возвращает флаг валидности объекта Entity.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (!this.AllowValidate)
                {
                    return true;
                }

                return _domainValidator.Validate(this.Entity).IsValid;
            }
        }
    }
}