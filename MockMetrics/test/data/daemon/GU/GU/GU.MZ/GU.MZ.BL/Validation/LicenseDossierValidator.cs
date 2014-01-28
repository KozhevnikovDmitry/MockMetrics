using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.Validation
{
    public class LicenseDossierValidator : AbstractDomainValidator<LicenseDossier>
    {
        public LicenseDossierValidator()
        {
            var dossier = LicenseDossier.CreateInstance();

            _validationActions[Util.GetPropertyName(() => dossier.RegNumber)] = ValidateRegNumber;
            _validationActions[Util.GetPropertyName(() => dossier.CreateDate)] = ValidateCreateDate;
            _validationActions[Util.GetPropertyName(() => dossier.LicensedActivityId)] = ValidateActivityId;
        }

        private string ValidateRegNumber(LicenseDossier licenseDossier)
        {
            return string.IsNullOrEmpty(licenseDossier.RegNumber) ? "Поле регистрационный номер должно быть заполнено" : null;
        }

        private string ValidateCreateDate(LicenseDossier licenseDossier)
        {
            return licenseDossier.CreateDate != new DateTime() ? null : "Поле дата создания должно быть заполнено";
        }

        private string ValidateActivityId(LicenseDossier licenseDossier)
        {
            return licenseDossier.LicensedActivityId == 0 ? "Поле лицензируемая деятельность должно быть заполнено" : null;
        }
    }
}