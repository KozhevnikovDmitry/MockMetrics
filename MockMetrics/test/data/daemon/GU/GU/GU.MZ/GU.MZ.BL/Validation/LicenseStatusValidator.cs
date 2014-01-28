using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.Validation
{
    public class LicenseStatusValidator : AbstractDomainValidator<LicenseStatus>
    {
        public LicenseStatusValidator()
        {
            var address = LicenseStatus.CreateInstance();
            _validationActions[Util.GetPropertyName(() => address.Stamp)] = ValidateStamp;
        }

        private string ValidateStamp(LicenseStatus licenseStatus)
        {
            return licenseStatus.Stamp == new DateTime() ? "Поле дата создания должно быть заполнено" : null;
        }
    }
}
