using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Notifying;

namespace GU.MZ.BL.Validation
{
    public class ViolationNoticeValidator : AbstractDomainValidator<ViolationNotice>
    {
        public ViolationNoticeValidator()
        {
            var violationNotice = ViolationNotice.CreateInstance();
            _validationActions[Util.GetPropertyName(() => violationNotice.TaskRegNumber)] = ValidateTaskRegNumber;
            _validationActions[Util.GetPropertyName(() => violationNotice.TaskStamp)] = ValidateTaskStamp;
            _validationActions[Util.GetPropertyName(() => violationNotice.LicenseHolder)] = ValidateLicenseHolder;
            _validationActions[Util.GetPropertyName(() => violationNotice.EmployeeName)] = ValidateEmployeeName;
            _validationActions[Util.GetPropertyName(() => violationNotice.EmployeePosition)] = ValidateEmployeePosition;
            _validationActions[Util.GetPropertyName(() => violationNotice.LicensedActivity)] = ValidateLicensedActivity;
            _validationActions[Util.GetPropertyName(() => violationNotice.Address)] = ValidateAddress;
        }

        private string ValidateEmployeeName(ViolationNotice notice)
        {
            return string.IsNullOrEmpty(notice.EmployeeName) ? "Поле ФИО сотрудника должно быть заполнено" : null;
        }

        private string ValidateEmployeePosition(ViolationNotice notice)
        {
            return string.IsNullOrEmpty(notice.EmployeePosition) ? "Поле должность сотрудника должно быть заполнено" : null;
        }

        private string ValidateLicensedActivity(ViolationNotice notice)
        {
            return string.IsNullOrEmpty(notice.LicensedActivity) ? "Поле лицензируемая деятельность должно быть заполнено" : null;
        }

        private string ValidateLicenseHolder(ViolationNotice notice)
        {
            return string.IsNullOrEmpty(notice.LicenseHolder) ? "Поле лицензиат должно быть заполнено" : null;
        }

        private string ValidateTaskStamp(ViolationNotice notice)
        {
            return notice.TaskStamp == new DateTime() ? "Поле дата принятия заявления должно быть заполнено" : null;
        }

        private string ValidateTaskRegNumber(ViolationNotice notice)
        {
            return notice.TaskRegNumber <= 0 ? "Поле номер заявлениия должно быть заполнено" : null;
        }

        private string ValidateAddress(ViolationNotice notice)
        {
            return string.IsNullOrEmpty(notice.Address)? "Поле адрес должно быть заполнено" : null;
        }
        
    }
}
