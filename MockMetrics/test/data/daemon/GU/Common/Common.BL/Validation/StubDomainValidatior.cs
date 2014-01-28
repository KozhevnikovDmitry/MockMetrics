using System.Collections.Generic;

using Common.DA.Interface;

namespace Common.BL.Validation
{
    /// <summary>
    /// Валидатор-заглушка для доменных объектов
    /// </summary>
    public class StubDomainValidatior<T> : IDomainValidator<T> where T : IDomainObject
    {
        /// <summary>
        /// Возвращает пустые результаты валидации - объект валиден.
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <returns>Пустые результаты</returns>
        public ValidationErrorInfo Validate(T domainObject)
        {
            return new ValidationErrorInfo();
        }

        /// <summary>
        /// Возвращает null - значение свойства корректно.
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns>null</returns>
        public string ValidateProperty(T domainObject, string propertyName)
        {
            return null;
        }

        /// <summary>
        /// Возвращает пустой список имён.
        /// </summary>
        public IEnumerable<string> ValidatedNames
        {
            get
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Флаг разрешающий проводить валидацию отдельных свойств доменного объекта
        /// </summary>
        public bool AllowSinglePropertyValidate { get; set; }
    }
}
