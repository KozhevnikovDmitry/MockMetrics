using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Validation
{
    public class ExpertiseOrderValidator : AbstractDomainValidator<ExpertiseOrder>
    {
        private readonly IDomainValidator<ExpertiseOrderAgree> _agreeValidator;
        private readonly IDomainValidator<ExpertiseHolderAddress> _addressValidator;

        public ExpertiseOrderValidator(IDomainValidator<ExpertiseOrderAgree> agreeValidator, IDomainValidator<ExpertiseHolderAddress> addressValidator)
        {
            _agreeValidator = agreeValidator;
            _addressValidator = addressValidator;
            var order = ExpertiseOrder.CreateInstance();
            _validationActions[Util.GetPropertyName(() => order.TaskId)] = ValidateTaskId;
            _validationActions[Util.GetPropertyName(() => order.FullName)] = ValidateFullName;
            _validationActions[Util.GetPropertyName(() => order.Stamp)] = ValidateStamp;
            _validationActions[Util.GetPropertyName(() => order.RegNumber)] = ValidateRegNumber;
            _validationActions[Util.GetPropertyName(() => order.EmployeeName)] = ValidateEmployeeName;
            _validationActions[Util.GetPropertyName(() => order.EmployeePosition)] = ValidateEmployeePosition;
            _validationActions[Util.GetPropertyName(() => order.EmployeeContacts)] = ValidateEmployeeContacts;
            _validationActions[Util.GetPropertyName(() => order.Inn)] = ValidateInn;
            _validationActions[Util.GetPropertyName(() => order.TaskStamp)] = ValidateTaskStamp;
            _validationActions[Util.GetPropertyName(() => order.DueDate)] = ValidateDueDate;
            _validationActions[Util.GetPropertyName(() => order.StartDate)] = ValidateStartDate;
            _validationActions[Util.GetPropertyName(() => order.DueDays)] = ValidateDueDays;
            _validationActions[Util.GetPropertyName(() => order.Address)] = ValidateAddress;
            _validationActions[Util.GetPropertyName(() => order.LicensedActivity)] = ValidateLicensedActivity;
        }

        public override ValidationErrorInfo Validate(ExpertiseOrder domainObject)
        {
            var result = base.Validate(domainObject);

            if (domainObject.ExpertiseHolderAddressList != null)
            {
                foreach (var address in domainObject.ExpertiseHolderAddressList)
                {
                    result.AddResult(_addressValidator.Validate(address));
                }
            }

            if (domainObject.ExpertiseOrderAgreeList != null)
            {
                foreach (var agree in domainObject.ExpertiseOrderAgreeList)
                {
                    result.AddResult(_agreeValidator.Validate(agree));
                }
            }

            return result;
        }

        private string ValidateFullName(ExpertiseOrder order)
        {
            return string.IsNullOrEmpty(order.FullName) ? "Поле полное название организации должно быть заполнено" : null;
        }
       
        private string ValidateAddress(ExpertiseOrder order)
        {
            return string.IsNullOrEmpty(order.Address) ? "Поле полное адрес организации должно быть заполнено" : null;
        }
        
        private string ValidateLicensedActivity(ExpertiseOrder order)
        {
            return string.IsNullOrEmpty(order.LicensedActivity) ? "Поле осуществляемая деятельность должно быть заполнено" : null;
        }

        private string ValidateTaskId(ExpertiseOrder order)
        {
            return order.TaskId < 0 ? "Поле регистрационный номер заявления должно быть заполнено" : null;
        }

        private string ValidateEmployeeContacts(ExpertiseOrder order)
        {
            return string.IsNullOrEmpty(order.EmployeeContacts) ? "Поле контакты сотрудника должно быть заполнено" : null;
        }

        private string ValidateEmployeePosition(ExpertiseOrder order)
        {
            return string.IsNullOrEmpty(order.EmployeePosition) ? "Поле должность сотрудника должно быть заполнено" : null;
        }

        private string ValidateEmployeeName(ExpertiseOrder order)
        {
            return string.IsNullOrEmpty(order.EmployeeName) ? "Поле ФИО сотрудника должно быть заполнено" : null;
        }

        private string ValidateStamp(ExpertiseOrder order)
        {
            return order.Stamp == new DateTime() ? "Поле дата издания приказа должно быть заполнено" : null;
        }

        private string ValidateTaskStamp(ExpertiseOrder order)
        {
            return order.TaskStamp == new DateTime() ? "Поле дата подачи заявления должно быть заполнено" : null;
        }

        private string ValidateDueDays(ExpertiseOrder order)
        {
            return order.DueDays <= 0  ? "Поле срок выполнения должно быть заполнено" : null;
        }

        private string ValidateStartDate(ExpertiseOrder order)
        {
            return order.StartDate == new DateTime() ? "Поле дата начала проведения проверки должно быть заполнено" : null;
        }

        private string ValidateDueDate(ExpertiseOrder order)
        {
            return order.DueDate == new DateTime() ? "Поле дата окончания проведения проверки должно быть заполнено" : null;
        }

        private string ValidateRegNumber(ExpertiseOrder order)
        {
            return string.IsNullOrEmpty(order.RegNumber) ? "Поле регистрационный номер приказа должно быть заполнено" : null;
        }

        private string ValidateInn(ExpertiseOrder order)
        {
            return string.IsNullOrEmpty(order.Inn) ? "Поле ИНН организации должно быть заполнено" : null;
        }
    }
}
