using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.BL.Validation
{
    public class LicenseRequisitesValidator : AbstractDomainValidator<LicenseRequisites>
    {
        private readonly IDomainValidator<JurRequisites> _jurValidator;
        private readonly IDomainValidator<IndRequisites> _indValidator;
        private readonly IDomainValidator<Address> _addressValidator;

        public LicenseRequisitesValidator(IDomainValidator<JurRequisites> jurValidator,
                                          IDomainValidator<IndRequisites> indValidator,
                                          IDomainValidator<Address> addressValidator)
        {
            _jurValidator = jurValidator;
            _indValidator = indValidator;
            _addressValidator = addressValidator;
            var holderRequisites = LicenseRequisites.CreateInstance();
            _validationActions[Util.GetPropertyName(() => holderRequisites.CreateDate)] = ValidateCreateDate;
        }

        public override ValidationErrorInfo Validate(LicenseRequisites domainObject)
        {
            var errorInfo = base.Validate(domainObject);
            errorInfo.AddResult(_addressValidator.Validate(domainObject.Address));

            if (domainObject.JurRequisites != null)
            {
                errorInfo.AddResult(_jurValidator.Validate(domainObject.JurRequisites));
            }
            else
            {
                errorInfo.AddResult(_indValidator.Validate(domainObject.IndRequisites));
            }

            return errorInfo;
        }

        private string ValidateCreateDate(LicenseRequisites holderRequisites)
        {
            return holderRequisites.CreateDate == new DateTime() ? "Поле Дата заведения должно быть заполнено" : null;
        }
    }
}