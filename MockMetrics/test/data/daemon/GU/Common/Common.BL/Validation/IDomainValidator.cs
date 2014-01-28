using System.Collections.Generic;
using System.ComponentModel;

using Common.DA.Interface;

namespace Common.BL.Validation
{
    public interface IDomainValidator
    {
        
    }

    /// <summary>
    /// Интерфейс для классов валидаторов доменных объектов.
    /// </summary>
    /// <remarks>
    /// Объекты валидаторы предполагается использовать для проверки корректности состояния объекта.
    /// Валидатор может использоваться классами бизнес логики, в этом случае следует вызывать метов <c>Validate</c> для полной валидации объекта.
    /// Валидатор может использоваться в классах ViewModel для валидации значений отдельных свойств.
    /// В этом случае нужно вызвать метод <c>ValidateProperty</c>. 
    /// Предполагается, что ViewModel наследует <see cref="IDataErrorInfo"/>, 
    /// поэтому метод <c>ValidateProperty</c> возвращает null для корректного значения.
    /// </remarks>
    public interface IDomainValidator<T> : IDomainValidator where T : IDomainObject
    {
        /// <summary>
        /// Валидирует свойства доменного объекта. Возвращает результаты валидации.
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <returns>Объект хранящий результаты валидации</returns>
        ValidationErrorInfo Validate(T domainObject);

        /// <summary>
        /// Валидирует одно свойство доменного объекта
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns>Сообщение </returns>
        string ValidateProperty(T domainObject, string propertyName);

        /// <summary>
        /// Возвращает список имён, валидируемых свойств.
        /// </summary>
        IEnumerable<string> ValidatedNames { get; }

        /// <summary>
        /// Флаг разрешающий проводить валидацию отдельных свойств доменного объекта
        /// </summary>
        bool AllowSinglePropertyValidate { get; set; }
    }
}
