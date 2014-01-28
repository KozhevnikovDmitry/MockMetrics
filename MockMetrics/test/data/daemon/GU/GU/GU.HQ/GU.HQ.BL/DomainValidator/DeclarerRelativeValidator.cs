using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    /// <summary>
    /// Класс валидатор данных о родственнике заявителя.
    /// </summary>
    public class DeclarerRelativeValidator : AbstractDomainValidator<DeclarerRelative>
    {
        private readonly IDomainValidator<Person> _personValidator;

        public DeclarerRelativeValidator(IDomainValidator<Person> personValidator)
        {
            _personValidator = personValidator;
            var t = DeclarerRelative.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.Sname)] = ValidatePersonSname;
            _validationActions[Util.GetPropertyName(() => t.Name)] = ValidatePersonName;
            _validationActions[Util.GetPropertyName(() => t.Patronymic)] = ValidatePersonPatronymic; 
         
        }

        /// <summary>
        /// Валидация Фамилии родственника
        /// </summary>
        /// <param name="declarerRelative"></param>
        /// <returns></returns>
        private string ValidatePersonSname(DeclarerRelative declarerRelative)
        {
            return string.IsNullOrEmpty(declarerRelative.Sname) ? "Поле 'Фамилия' не заполнено!" : null;
        }

        /// <summary>
        /// Валидация имени родственника
        /// </summary>
        /// <param name="declarerRelative"></param>
        /// <returns></returns>
        private string ValidatePersonName(DeclarerRelative declarerRelative)
        {
            return string.IsNullOrEmpty(declarerRelative.Name) ? "Поле 'Имя' не заполнено!" : null;
        }

        /// <summary>
        /// Валидация отчества родственника
        /// </summary>
        /// <param name="declarerRelative"></param>
        /// <returns></returns>
        private string ValidatePersonPatronymic(DeclarerRelative declarerRelative)
        {
            return string.IsNullOrEmpty(declarerRelative.Patronymic) ? "Поле 'Отчество' не заполнено!" : null;
        }
    }
}
