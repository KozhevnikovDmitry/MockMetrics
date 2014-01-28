using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class PersonValidator : AbstractDomainValidator<Person>
    {
        public PersonValidator()
        {
            var person = Person.CreateInstance();
            _validationActions[Util.GetPropertyName(() => person.Sname)] = ValidatePersonSname;
            _validationActions[Util.GetPropertyName(() => person.Name)] = ValidatePersonName;
            _validationActions[Util.GetPropertyName(() => person.Patronymic)] = ValidatePersonPatronymic;

        }

        private string ValidatePersonSname(Person person)
        {
            return string.IsNullOrEmpty(person.Sname) ? "Поле 'Фамилия' не заполнено!" : null;
        }

        private string ValidatePersonName(Person person)
        {
            return string.IsNullOrEmpty(person.Name) ? "Поле 'Имя' не заполнено!" : null;
        }

        private string ValidatePersonPatronymic(Person person)
        {
            return string.IsNullOrEmpty(person.Patronymic) ? "Поле 'Отчество' не заполнено!" : null;
        }
    }
}
