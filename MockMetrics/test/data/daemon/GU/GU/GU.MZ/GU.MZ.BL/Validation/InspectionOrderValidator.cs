using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Validation
{
    public class InspectionOrderValidator : AbstractDomainValidator<InspectionOrder>
    {
        private readonly IDomainValidator<InspectionOrderAgree> _agreeValidator;
        private readonly IDomainValidator<InspectionOrderExpert> _expertValidator;
        private readonly IDomainValidator<InspectionHolderAddress> _addressValidator;

        public InspectionOrderValidator(IDomainValidator<InspectionOrderAgree> agreeValidator,
                                        IDomainValidator<InspectionOrderExpert> expertValidator, 
                                        IDomainValidator<InspectionHolderAddress> addressValidator)
        {
            _agreeValidator = agreeValidator;
            _expertValidator = expertValidator;
            _addressValidator = addressValidator;
            var order = InspectionOrder.CreateInstance();
            _validationActions[Util.GetPropertyName(() => order.TaskId)] = ValidateTaskId;
            _validationActions[Util.GetPropertyName(() => order.FullName)] = ValidateFullName;
            _validationActions[Util.GetPropertyName(() => order.Stamp)] = ValidateStamp;
            _validationActions[Util.GetPropertyName(() => order.RegNumber)] = ValidateRegNumber;
            _validationActions[Util.GetPropertyName(() => order.EmployeeName)] = ValidateEmployeeName;
            _validationActions[Util.GetPropertyName(() => order.EmployeePosition)] = ValidateEmployeePosition;
            _validationActions[Util.GetPropertyName(() => order.EmployeeContacts)] = ValidateEmployeeContacts;
            _validationActions[Util.GetPropertyName(() => order.Inn)] = ValidateInn;
            _validationActions[Util.GetPropertyName(() => order.TaskStamp)] = ValidateInn;
            _validationActions[Util.GetPropertyName(() => order.DueDate)] = ValidateDueDate;
            _validationActions[Util.GetPropertyName(() => order.StartDate)] = ValidateStartDate;
            _validationActions[Util.GetPropertyName(() => order.DueDays)] = ValidateDueDays;
            _validationActions[Util.GetPropertyName(() => order.Address)] = ValidateAddress;
            _validationActions[Util.GetPropertyName(() => order.LicensedActivity)] = ValidateLicensedActivity;
            _validationActions[Util.GetPropertyName(() => order.ActivityAdditionalInfo)] = ValidateActivityAdditionalInfo;
        }

        public override ValidationErrorInfo Validate(InspectionOrder domainObject)
        {
            var result = base.Validate(domainObject);

            if (domainObject.InspectionHolderAddressList != null)
            {
                foreach (var address in domainObject.InspectionHolderAddressList)
                {
                    result.AddResult(_addressValidator.Validate(address));
                }
            }

            if (domainObject.InspectionOrderAgreeList != null)
            {
                foreach (var agree in domainObject.InspectionOrderAgreeList)
                {
                    result.AddResult(_agreeValidator.Validate(agree));
                }
            }

            if (domainObject.InspectionOrderExpertList != null)
            {
                foreach (var expert in domainObject.InspectionOrderExpertList)
                {
                    result.AddResult(_expertValidator.Validate(expert));
                }
            }

            return result;
        }

        private string ValidateFullName(InspectionOrder order)
        {
            return string.IsNullOrEmpty(order.FullName) ? "Поле полное название организации должно быть заполнено" : null;
        }
        private string ValidateAddress(InspectionOrder order)
        {
            return string.IsNullOrEmpty(order.Address) ? "Поле полное адрес организации должно быть заполнено" : null;
        }
        private string ValidateLicensedActivity(InspectionOrder order)
        {
            return string.IsNullOrEmpty(order.LicensedActivity) ? "Поле осуществляемая деятельность должно быть заполнено" : null;
        }
        private string ValidateActivityAdditionalInfo(InspectionOrder order)
        {
            return string.IsNullOrEmpty(order.ActivityAdditionalInfo) ? "Поле осуществляемая деятельность(дополн.) должно быть заполнено" : null;
        }

        private string ValidateTaskId(InspectionOrder order)
        {
            return order.TaskId < 0 ? "Поле регистрационный номер заявления должно быть заполнено" : null;
        }

        private string ValidateEmployeeContacts(InspectionOrder order)
        {
            return string.IsNullOrEmpty(order.EmployeeContacts) ? "Поле контакты сотрудника должно быть заполнено" : null;
        }

        private string ValidateEmployeePosition(InspectionOrder order)
        {
            return string.IsNullOrEmpty(order.EmployeePosition) ? "Поле должность сотрудника должно быть заполнено" : null;
        }

        private string ValidateEmployeeName(InspectionOrder order)
        {
            return string.IsNullOrEmpty(order.EmployeeName) ? "Поле ФИО сотрудника должно быть заполнено" : null;
        }

        private string ValidateStamp(InspectionOrder order)
        {
            return order.Stamp == new DateTime() ? "Поле дата издания приказа должно быть заполнено" : null;
        }

        private string ValidateTaskStamp(InspectionOrder order)
        {
            return order.TaskStamp == new DateTime() ? "Поле дата подачи заявления должно быть заполнено" : null;
        }

        private string ValidateDueDays(InspectionOrder order)
        {
            return order.DueDays <= 0  ? "Поле срок выполнения должно быть заполнено" : null;
        }

        private string ValidateStartDate(InspectionOrder order)
        {
            return order.StartDate == new DateTime() ? "Поле дата начала проведения проверки должно быть заполнено" : null;
        }

        private string ValidateDueDate(InspectionOrder order)
        {
            return order.DueDate == new DateTime() ? "Поле дата окончания проведения проверки должно быть заполнено" : null;
        }

        private string ValidateRegNumber(InspectionOrder order)
        {
            return string.IsNullOrEmpty(order.RegNumber) ? "Поле регистрационный номер приказа должно быть заполнено" : null;
        }

        private string ValidateInn(InspectionOrder order)
        {
            return string.IsNullOrEmpty(order.Inn) ? "Поле ИНН организации должно быть заполнено" : null;
        }
    }
}
