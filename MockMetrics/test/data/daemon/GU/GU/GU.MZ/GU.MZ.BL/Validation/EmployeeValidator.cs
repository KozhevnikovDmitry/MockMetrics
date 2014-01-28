using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.Validation
{
    public class EmployeeValidator : AbstractDomainValidator<Employee>
    {
        public EmployeeValidator()
        {
            var employee = Employee.CreateInstance();
            _validationActions[Util.GetPropertyName(() => employee.Name)] = ValidateName;
            _validationActions[Util.GetPropertyName(() => employee.Phone)] = ValidatePhone;
            _validationActions[Util.GetPropertyName(() => employee.Position)] = ValidatePosition;
            _validationActions[Util.GetPropertyName(() => employee.Email)] = ValidateEmail;
        }

        private string ValidateName(Employee employee)
        {
            return string.IsNullOrEmpty(employee.Name) ? "Поле ФИО должно быть заполнено" : null;
        }

        private string ValidatePhone(Employee employee)
        {
            return string.IsNullOrEmpty(employee.Phone) ? "Поле Телефон должно быть заполнено" : null;
        }

        private string ValidatePosition(Employee employee)
        {
            return string.IsNullOrEmpty(employee.Position) ? "Поле Должность должно быть заполнено" : null;
        }

        private string ValidateEmail(Employee employee)
        {
            return string.IsNullOrEmpty(employee.Email) ? "Поле Email должно быть заполнено" : null;
        }
    }
}