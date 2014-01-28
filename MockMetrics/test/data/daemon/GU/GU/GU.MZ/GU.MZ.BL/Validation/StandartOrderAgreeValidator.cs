using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Validation
{
    public class StandartOrderAgreeValidator : AbstractDomainValidator<StandartOrderAgree>
    {
        public StandartOrderAgreeValidator()
        {
            var orderAgree = StandartOrderAgree.CreateInstance();
            _validationActions[Util.GetPropertyName(() => orderAgree.EmployeeName)] = ValidateEmployeeName;
            _validationActions[Util.GetPropertyName(() => orderAgree.EmployeePosition)] = ValidateEmployeePosition;
        }

        private string ValidateEmployeePosition(StandartOrderAgree order)
        {
            return string.IsNullOrEmpty(order.EmployeePosition) ? "Поле должность сотрудника должно быть заполнено" : null;
        }

        private string ValidateEmployeeName(StandartOrderAgree order)
        {
            return string.IsNullOrEmpty(order.EmployeeName) ? "Поле ФИО сотрудника должно быть заполнено" : null;
        }
    }
}
