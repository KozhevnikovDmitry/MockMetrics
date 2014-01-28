using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.BL.Validation
{
    public class HolderRequisitesValidator : AbstractDomainValidator<HolderRequisites>
    {
        private readonly IDomainValidator<JurRequisites> _jurValidator;
        private readonly IDomainValidator<IndRequisites> _indValidator;
        private readonly IDomainValidator<Address> _addressValidator;

        public HolderRequisitesValidator(IDomainValidator<JurRequisites> jurValidator, 
                                         IDomainValidator<IndRequisites> indValidator,
                                         IDomainValidator<Address> addressValidator)
        {
            _jurValidator = jurValidator;
            _indValidator = indValidator;
            _addressValidator = addressValidator;
            var holderRequisites = HolderRequisites.CreateInstance();
            holderRequisites.Address = Address.CreateInstance();
            _validationActions[Util.GetPropertyName(() => holderRequisites.CreateDate)] = ValidateCreateDate;
            _validationActions[Util.GetPropertyName(() => holderRequisites.Address)] = ValidateAddress;
        }

        public override ValidationErrorInfo Validate(HolderRequisites domainObject)
        {
            var errorInfo = base.Validate(domainObject);

            if (domainObject.Address != null)
            {
                errorInfo.AddResult(_addressValidator.Validate(domainObject.Address));
            }

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

        private string ValidateCreateDate(HolderRequisites holderRequisites)
        {
            return holderRequisites.CreateDate != new DateTime() ? "Поле Дата заведения должно быть заполнено" : null;
        }

        private string ValidateAddress(HolderRequisites requisites)
        {
            if (requisites.Address != null)
            {
                var result = _addressValidator.Validate(requisites.Address);

                if (!result.IsValid)
                {
                    return "Поле Адрес заполнено неверно";
                }
            }

            return null;
        }
    }
}
