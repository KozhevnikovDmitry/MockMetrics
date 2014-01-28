using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.Validation
{
    public class InventoryValidator : AbstractDomainValidator<DocumentInventory>
    {
        private readonly IDomainValidator<ProvidedDocument> _docValidator;

        public InventoryValidator(IDomainValidator<ProvidedDocument> docValidator)
        {
            _docValidator = docValidator;
            var inventory = DocumentInventory.CreateInstance();
            _validationActions[Util.GetPropertyName(() => inventory.RegNumber)] = ValidateRegNumber;
            _validationActions[Util.GetPropertyName(() => inventory.Stamp)] = ValidateStamp;
            _validationActions[Util.GetPropertyName(() => inventory.LicenseHolder)] = ValidateLicenseHolder;
            _validationActions[Util.GetPropertyName(() => inventory.EmployeeName)] = ValidateEmployeeName;
            _validationActions[Util.GetPropertyName(() => inventory.EmployeePosition)] = ValidateEmployeePosition;
            _validationActions[Util.GetPropertyName(() => inventory.LicensedActivity)] = ValidateLicensedActivity;
        }

        public override ValidationErrorInfo Validate(DocumentInventory domainObject)
        {
            var result = base.Validate(domainObject);

            if (domainObject.ProvidedDocumentList != null)
            {
                foreach (var doc in domainObject.ProvidedDocumentList)
                {
                    result.AddResult(_docValidator.Validate(doc));
                }
            }

            return result;
        }

        private string ValidateEmployeeName(DocumentInventory inventory)
        {
            return string.IsNullOrEmpty(inventory.EmployeeName) ? "Поле ФИО сотрудника должно быть заполнено" : null;
        }

        private string ValidateEmployeePosition(DocumentInventory inventory)
        {
            return string.IsNullOrEmpty(inventory.EmployeePosition) ? "Поле должность сотрудника должно быть заполнено" : null;
        }

        private string ValidateLicensedActivity(DocumentInventory inventory)
        {
            return string.IsNullOrEmpty(inventory.LicensedActivity) ? "Поле лицензируемая деятельность должно быть заполнено" : null;
        }

        private string ValidateLicenseHolder(DocumentInventory inventory)
        {
            return string.IsNullOrEmpty(inventory.LicenseHolder) ? "Поле лицензиат должно быть заполнено" : null;
        }

        private string ValidateStamp(DocumentInventory inventory)
        {
            return inventory.Stamp == new DateTime() ? "Поле дата принятия должно быть заполнено" : null;
        }

        private string ValidateRegNumber(DocumentInventory inventory)
        {
            return inventory.RegNumber <= 0 ? "Поле регистрационный номер должно быть заполнено" : null;
        }
    }
}