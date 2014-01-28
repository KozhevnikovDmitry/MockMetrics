using System;
using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class ClaimHouseProvidedValidator : AbstractDomainValidator<HouseProvided>
    {

        public ClaimHouseProvidedValidator()
        {
            var t = HouseProvided.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.DocumentDate)] = ValidateDocumentDate;
            _validationActions[Util.GetPropertyName(() => t.DocumentNum)] = ValidateDocumentNum;
            _validationActions[Util.GetPropertyName(() => t.HouseProvidedPrivBaseTypeId)] = ValidateHouseProvidedPrivBaseTypeId;
        }

        private string ValidateDocumentDate(HouseProvided houseProvided)
        {
            return houseProvided.DocumentDate == null ? "Предоставленное жильё. Дата решения должна быть указана." : null;
        }

        private string ValidateDocumentNum(HouseProvided houseProvided)
        {
            return !Regex.IsMatch(houseProvided.DocumentNum, @"^[а-яА-Я0-9]+$") ? "Предоставленное жильё. Номер решения должен быть указан. Номер документа может содержать только русские буквы и/или цифры" : null;
        }

        private string ValidateHouseProvidedPrivBaseTypeId(HouseProvided houseProvided)
        {
            if (houseProvided.IsPrivHouseProvided == 1)
                if (houseProvided.HouseProvidedPrivBaseTypeId == null || houseProvided.HouseProvidedPrivBaseTypeId < 1)
                    return "Предоставленное жильё. Необходимо указать основание внеочередного предоставления жилья.";
            
            return null;
        }
    }
}