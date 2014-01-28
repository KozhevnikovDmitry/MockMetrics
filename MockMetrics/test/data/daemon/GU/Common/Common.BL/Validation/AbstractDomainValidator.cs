using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL.Validation
{
    /// <summary>
    /// Базовый класс для классов валидаторов доменных объектов
    /// </summary>
    /// <remarks>
    /// Для того, чтобы добавить валидацию свойств объекта в наследнике от <c>AbstractDomainValidator</c>,
    /// надо определить методы вылидации каждого свойства и добавить делегаты на эти методы в словарь <c>_validationActions</c>.
    /// Дополнительную логику по валидации объекта в целом можно добавить в переопределение метода <c>Validate</c>.
    /// По умолчанию <c>Validate</c> просто последовательно перебирает методы в <c>_validationActions</c>.
    /// </remarks>
    public abstract class AbstractDomainValidator<T> : IDomainValidator<T> where T : IPersistentObject
    {
        /// <summary>
        /// Словарь делегатов на методы валидации 
        /// </summary>
        protected readonly Dictionary<string, Func<T, string>> _validationActions = new Dictionary<string,Func<T,string>>();

        /// <summary>
        /// Флаг разрешающий проводить валидацию отдельных свойств доменного объекта
        /// </summary>
        public bool AllowSinglePropertyValidate { get; set; }
        
        /// <summary>
        /// Валидирует свойства доменного объекта. Возвращает результаты валидации.
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <returns>Объект хранящий результаты валидации</returns>
        public virtual ValidationErrorInfo Validate(T domainObject)
        {
            var result = new ValidationErrorInfo();
            
            foreach (var validationAction in _validationActions)
            {
                var error = validationAction.Value(domainObject);
                if(error != null)
                {
                    result.AddError(error);
                }
            }

            AllowSinglePropertyValidate = true;

            return result;
        }

        /// <summary>
        /// Валидирует одно свойство доменного объекта
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns>Сообщение </returns>
        public virtual string ValidateProperty(T domainObject, string propertyName)
        {
            if (_validationActions.ContainsKey(propertyName))
            {
                return AllowSinglePropertyValidate ? this._validationActions[propertyName](domainObject) : null; 
            }

            return null;
        }

        /// <summary>
        /// Возвращает список имён, валидируемых свойств.
        /// </summary>
        public IEnumerable<string> ValidatedNames
        {
            get
            {
                return _validationActions.Keys;
            }
        }
    }
}
