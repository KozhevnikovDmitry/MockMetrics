using Common.BL.Validation;
using Common.Types;

using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Класс валидатор сущностей Объект с номенклатурой
    /// </summary>
    public class LicenseObjectValidator : AbstractDomainValidator<LicenseObject>
    {
        /// <summary>
        /// Валиадатор адресов
        /// </summary>
        private readonly IDomainValidator<Address> _addressValidator;

        /// <summary>
        /// Класс валидатор сущностей Объект с номенклатурой
        /// </summary>
        /// <param name="addressValidator">Валиадатор адресов</param>
        public LicenseObjectValidator(IDomainValidator<Address> addressValidator)
        {
            _addressValidator = addressValidator;
            var licenseObject = LicenseObject.CreateInstance();
            _validationActions[Util.GetPropertyName(() => licenseObject.Name)] = ValidateName;
            _validationActions[Util.GetPropertyName(() => licenseObject.GrantOrderRegNumber)] = ValidateGrantOrderRegNumber;
            _validationActions[Util.GetPropertyName(() => licenseObject.GrantOrderStamp)] = ValidateGrantOrderStamp;
            _validationActions[Util.GetPropertyName(() => licenseObject.LicenseObjectStatusId)] = ValidateLicenseObjectStatusId;
            _validationActions[Util.GetPropertyName(() => licenseObject.Address)] = ValidateAddress;
            _validationActions[Util.GetPropertyName(() => licenseObject.ObjectSubactivityList)] = ValidateObjectSubactivityList;
        }

        private string ValidateAddress(LicenseObject licenseObject)
        {
            if (licenseObject.Address == null)
            {
                return "Поле адрес должно быть заполнено";
            }

            var results = _addressValidator.Validate(licenseObject.Address);
            if (!results.IsValid)
            {
                return "Поле адрес должно быть заполнено корректными данными";
            }

            return null;
        }

        private string ValidateLicenseObjectStatusId(LicenseObject licenseObject)
        {
            return licenseObject.LicenseObjectStatusId == 0 ? "Поле статус объекта должно быть заполнено" : null;
        }

        private string ValidateName(LicenseObject licenseObject)
        {
            return string.IsNullOrEmpty(licenseObject.Name) ? "Поле наименование должно быть заполнено" : null;
        }

        private string ValidateGrantOrderRegNumber(LicenseObject licenseObject)
        {
            return string.IsNullOrEmpty(licenseObject.GrantOrderRegNumber) ? "Поле номер решения о предоставлении лицензии должно быть заполнено" : null;
        }

        private string ValidateGrantOrderStamp(LicenseObject licenseObject)
        {
            return !licenseObject.GrantOrderStamp.HasValue ? "Поле дата принятия решения о предоставлении лицензии должно быть заполнено" : null;
        }

        private string ValidateObjectSubactivityList(LicenseObject licenseObject)
        {
            if (licenseObject.ObjectSubactivityList == null || licenseObject.ObjectSubactivityList.Count == 0)
            {
                return "Должна быть выбрана хотя бы одна лицензируемая деятельность";
            }

            return null;
        }
    }
}
