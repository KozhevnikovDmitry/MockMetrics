using System;
using Common.BL.Validation;
using Common.DA;
using Common.Types;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Validation
{
    public class StandartOrderValidator : AbstractDomainValidator<StandartOrder>
    {
        private readonly IDomainValidator<StandartOrderDetail> _detailValidator;
        private readonly IDomainValidator<StandartOrderAgree> _agreeValidator;

        public StandartOrderValidator(IDomainValidator<StandartOrderDetail> detailValidator, IDomainValidator<StandartOrderAgree> agreeValidator)
        {
            _detailValidator = detailValidator;
            _agreeValidator = agreeValidator;
            var order = StandartOrder.CreateInstance();
            _validationActions[Util.GetPropertyName(() => order.Stamp)] = ValidateStamp;
            _validationActions[Util.GetPropertyName(() => order.RegNumber)] = ValidateRegNumber;
            _validationActions[Util.GetPropertyName(() => order.EmployeeName)] = ValidateEmployeeName;
            _validationActions[Util.GetPropertyName(() => order.EmployeePosition)] = ValidateEmployeePosition;
            _validationActions[Util.GetPropertyName(() => order.EmployeeContacts)] = ValidateEmployeeContacts;
            _validationActions[Util.GetPropertyName(() => order.LicensiarHeadName)] = ValidateLicensiarHeadName;
            _validationActions[Util.GetPropertyName(() => order.LicensiarHeadPosition)] = ValidateLicensiarHeadPosition;
        }

        private string ValidateLicensiarHeadPosition(StandartOrder order)
        {
            return string.IsNullOrEmpty(order.LicensiarHeadPosition) ? "Поле должность главы лицензирующего органа должно быть заполнено" : null;
        }

        private string ValidateLicensiarHeadName(StandartOrder order)
        {
            return string.IsNullOrEmpty(order.LicensiarHeadName) ? "Поле ФИО главы лицензирующего органа должно быть заполнено" : null;
        }

        private string ValidateEmployeeContacts(StandartOrder order)
        {
            return string.IsNullOrEmpty(order.EmployeeContacts) ? "Поле контакты сотрудника должно быть заполнено" : null;
        }

        private string ValidateEmployeePosition(StandartOrder order)
        {
            return string.IsNullOrEmpty(order.EmployeePosition) ? "Поле должность сотрудника должно быть заполнено" : null;
        }

        private string ValidateEmployeeName(StandartOrder order)
        {
            return string.IsNullOrEmpty(order.EmployeeName) ? "Поле ФИО сотрудника должно быть заполнено" : null;
        }

        private string ValidateStamp(StandartOrder order)
        {
            return order.Stamp == new DateTime() ? "Поле дата издания приказа должно быть заполнено" : null;
        }

        private string ValidateRegNumber(StandartOrder order)
        {

            return string.IsNullOrEmpty(order.RegNumber) ? "Поле регистрационный номер приказа должно быть заполнено" : null;
        }

        public override ValidationErrorInfo Validate(StandartOrder domainObject)
        {
            var result = base.Validate(domainObject);
            if (domainObject.PersistentState == PersistentState.Old)
            {
                if (domainObject.DetailList != null)
                {
                    foreach (var detail in domainObject.DetailList)
                    {
                        result.AddResult(_detailValidator.Validate(detail), "Детализация приказа");
                    }
                }

                if (domainObject.AgreeList != null)
                {
                    foreach (var agree in domainObject.AgreeList)
                    {
                        result.AddResult(_agreeValidator.Validate(agree), "Согласовавшие приказ");
                    }
                }
            }

            return result;
        }
    }
}
