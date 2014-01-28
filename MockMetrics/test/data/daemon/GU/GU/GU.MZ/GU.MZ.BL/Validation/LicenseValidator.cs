using Common.BL.DictionaryManagement;
using Common.BL.Validation;
using Common.Types;

using GU.MZ.DataModel.Licensing;

using System.Linq;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Валидатор объектов лицензий
    /// </summary>
    public class LicenseValidator : AbstractDomainValidator<License>
    {
        /// <summary>
        /// Валидатор объектов с номенклатурой
        /// </summary>
        private readonly IDomainValidator<LicenseObject> _licenseObjectValidator;

        private readonly IDomainValidator<LicenseRequisites> _licenseRequisitesValidator;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        private readonly IDictionaryManager _dictionaryManager;

        /// <summary>
        /// Валидатор объектов лицензий
        /// </summary>
        /// <param name="licenseObjectValidator">Валидатор объектов с номенклатурой</param>
        /// <param name="licenseRequisitesValidator"></param>
        /// <param name="dictionaryManager">Менеджер справочников</param>
        public LicenseValidator(IDomainValidator<LicenseObject> licenseObjectValidator, IDomainValidator<LicenseRequisites> licenseRequisitesValidator, IDictionaryManager dictionaryManager)
        {
            _licenseObjectValidator = licenseObjectValidator;
            _licenseRequisitesValidator = licenseRequisitesValidator;
            _dictionaryManager = dictionaryManager;
            var license = License.CreateInstance();
            _validationActions[Util.GetPropertyName(() => license.RegNumber)] = ValidateRegNumber;
            _validationActions[Util.GetPropertyName(() => license.GrantDate)] = ValidateGrantDate;
            _validationActions[Util.GetPropertyName(() => license.BlankNumber)] = ValidateBlankNumber;
            _validationActions[Util.GetPropertyName(() => license.GrantOrderRegNumber)] = ValidateGrantOrderRegNumber;
            _validationActions[Util.GetPropertyName(() => license.GrantOrderStamp)] = ValidateGrantOrderStamp;
            _validationActions[Util.GetPropertyName(() => license.CurrentStatus)] = ValidateLicenseStatusId;
            _validationActions[Util.GetPropertyName(() => license.LicensedActivityId)] = ValidateActivityId;
            _validationActions[Util.GetPropertyName(() => license.LicenseDossierId)] = ValidateDossierId;
        }

        public override ValidationErrorInfo Validate(License domainObject)
        {
            var errorInfo = base.Validate(domainObject);
            if (domainObject.ActualRequisites != null)
            {
                errorInfo.AddResult(_licenseRequisitesValidator.Validate(domainObject.ActualRequisites));
            }
            else
            {
                errorInfo.AddError("Реквизиты лицензии не указаны");
            }
            return errorInfo;
        }

        private string ValidateRegNumber(License license)
        {
            return string.IsNullOrEmpty(license.RegNumber) ? "Поле регистрационный номер должно быть заполнено" : null;
        }

        private string ValidateGrantDate(License license)
        {
            return license.GrantDate.HasValue ? null : "Поле дата предоставления должно быть заполнено";
        }

        private string ValidateBlankNumber(License license)
        {
            return string.IsNullOrEmpty(license.BlankNumber) ? "Поле номер бланка должно быть заполнено" : null;
        }

        private string ValidateGrantOrderRegNumber(License license)
        {
            return string.IsNullOrEmpty(license.GrantOrderRegNumber) ? "Поле регистрационный номер решения о предоставлении лицензии должно быть заполнено" : null;
        }

        private string ValidateGrantOrderStamp(License license)
        {
            return license.GrantOrderStamp.HasValue ? null : "Поле дата решения о предоставлении лицензии должно быть заполнено";
        }

        private string ValidateDossierId(License license)
        {
            return license.LicenseDossierId == 0 ? "C лицензией должно быть ассоциировано лицензионное дело" : null;
        }

        private string ValidateActivityId(License license)
        {
            if (license.LicensedActivityId == 0)
            {
                return "Поле лицензируемая деятельность должно быть заполнено";
            }

            if (license.LicenseObjectList == null)
            {
                return null;
            }

            var subactivities =
                _dictionaryManager.GetDictionary<LicensedSubactivity>()
                                  .Where(t => t.LicensedActivityId == license.LicensedActivityId)
                                  .Select(t => t.Id);
            
            if (license.LicenseObjectList.Any(t => t.ObjectSubactivityList.Any(s => !subactivities.Contains(s.LicensedSubactivityId))))
            {
                return
                    @"Вид лицензируемой деятельности в лицензии не соответствует набору работ\услуг в объектах с номенклатурой.";
            }
            return null;

        }

        private string ValidateLicenseStatusId(License license)
        {
            return license.CurrentStatus == 0 ? "Поле статус лицензии должно быть заполнено" : null;
        }
    }
}
