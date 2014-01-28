using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Holder;

namespace GU.MZ.BL.Validation
{
    public class LicenseHolderValidator : AbstractDomainValidator<LicenseHolder>
    {
        private readonly IDomainValidator<HolderRequisites> _requisitesValidator;

        public LicenseHolderValidator(IDomainValidator<HolderRequisites> requisitesValidator)
        {
            _requisitesValidator = requisitesValidator;
            var holder = LicenseHolder.CreateInstance();

            _validationActions[Util.GetPropertyName(() => holder.Inn)] = ValidateInn;
            _validationActions[Util.GetPropertyName(() => holder.Ogrn)] = ValidateOgrn;
        }

        public override ValidationErrorInfo Validate(LicenseHolder domainObject)
        {
            var errorInfo = base.Validate(domainObject);
            errorInfo.AddResult(_requisitesValidator.Validate(domainObject.ActualRequisites));
            return errorInfo;
        }

        private string ValidateInn(LicenseHolder arg)
        {
            return string.IsNullOrEmpty(arg.Inn) ? "Поле ИНН должно быть заполнено" : null;
        }

        private string ValidateOgrn(LicenseHolder arg)
        {
            return string.IsNullOrEmpty(arg.Ogrn) ? "Поле ОГРН должно быть заполнено" : null;
        }
    }
}