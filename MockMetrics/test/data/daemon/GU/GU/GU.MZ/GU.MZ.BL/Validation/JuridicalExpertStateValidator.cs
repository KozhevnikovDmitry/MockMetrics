using Common.BL.Validation;
using Common.Types;

using GU.MZ.DataModel;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Валиадатор экспертов юридических лиц
    /// </summary>
    public class JuridicalExpertStateValidator : AbstractDomainValidator<JuridicalExpertState>, IDomainValidator<IExpertState>, IExpertStateValidator
    {
        public ExpertStateType ExpertStateType
        {
            get
            {
                return ExpertStateType.Juridical;
            }
        }

        private readonly IDomainValidator<Address> _addressValidator;

        public JuridicalExpertStateValidator(IDomainValidator<Address> addressValidator)
        {
            _addressValidator = addressValidator;
            var expertState = JuridicalExpertState.CreateInstance();
            expertState.Address = Address.CreateInstance();
            _validationActions[Util.GetPropertyName(() => expertState.FirmName)] = ValidateFirmname;
            _validationActions[Util.GetPropertyName(() => expertState.ShortName)] = ValidateShortname;
            _validationActions[Util.GetPropertyName(() => expertState.Inn)] = ValidateInn;
            _validationActions[Util.GetPropertyName(() => expertState.Ogrn)] = ValidateOgrn;
            _validationActions[Util.GetPropertyName(() => expertState.FullName)] = ValidateFullName;
            _validationActions[Util.GetPropertyName(() => expertState.HeadPositionName)] = ValidateHeadPositionName;
            _validationActions[Util.GetPropertyName(() => expertState.HeadName)] = ValidateHeadName;
            _validationActions[Util.GetPropertyName(() => expertState.LegalFormId)] = ValidateHeadLegalForm;
            _validationActions[Util.GetPropertyName(() => expertState.Address)] = ValidateAddress;
        }


        #region Overrides of IDomainValidator<IExpertState>

        public ValidationErrorInfo Validate(IExpertState domainObject)
        {
            return (this as IDomainValidator<JuridicalExpertState>).Validate(domainObject as JuridicalExpertState);
        }

        public string ValidateProperty(IExpertState domainObject, string propertyName)
        {
            return (this as IDomainValidator<JuridicalExpertState>).ValidateProperty(domainObject as JuridicalExpertState, propertyName);
        }

        #endregion


        #region Overrides of AbstractDomainValidator<JuridicalExpertState>

        public override ValidationErrorInfo Validate(JuridicalExpertState domainObject)
        {
            if (domainObject != null)
            {
                var result = base.Validate(domainObject);

                var address = (domainObject is JuridicalExpertState)
                    ? (domainObject as JuridicalExpertState).Address
                    : null;

                if (address != null)
                {
                    result.AddResult(_addressValidator.Validate(address)); 
                }

                return result;
            }

            return new ValidationErrorInfo();
        }


        private string ValidateFullName(JuridicalExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.FullName) ? "Поле полное наименование должно быть заполнено" : null;
        }

        private string ValidateFirmname(JuridicalExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.FirmName) ? "Поле фирменное наименование должно быть заполнено" : null;
        }

        private string ValidateShortname(JuridicalExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.ShortName) ? "Поле короткое наименование должно быть заполнено" : null;
        }

        private string ValidateInn(JuridicalExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.Inn) ? "Поле ИНН должно быть заполнено" : null;
        }

        private string ValidateOgrn(JuridicalExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.Ogrn) ? "Поле ОГРН должно быть заполнено" : null;
        }

        private string ValidateHeadName(JuridicalExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.HeadName) ? "Поле наименование исполнительного органа должно быть заполнено" : null;
        }

        private string ValidateHeadPositionName(JuridicalExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.HeadPositionName) ? "Поле исполнительный орган должно быть заполнено" : null;
        }

        private string ValidateHeadLegalForm(JuridicalExpertState expertState)
        {
            return expertState.LegalFormId == 0 ? "Поле ОПФ должно быть заполнено" : null;
        }

        private string ValidateAddress(IExpertState expertState)
        {
            var address = (expertState is JuridicalExpertState)
                    ? (expertState as JuridicalExpertState).Address
                    : null;

            if (address != null)
            {
                var result = _addressValidator.Validate(address);

                if (!result.IsValid)
                {
                    return "Поле Адрес заполнено неверно";
                }
            }

            return null;
        }

        #endregion
    }
}
