using System;
using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class HouseProvidedValidator : AbstractDomainValidator<HouseProvided>
    {

        public HouseProvidedValidator()
        {
            var t = HouseProvided.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.DocumentDate)] = ValidateDocumentDate;
            _validationActions[Util.GetPropertyName(() => t.DocumentNum)] = ValidateDocumentNum;
        }

        private string ValidateDocumentDate(HouseProvided houseProvided)
        {
            return houseProvided.DocumentDate == DateTime.MinValue ? "Дата документа должна быть указана." : null;
        }

        private string ValidateDocumentNum(HouseProvided houseProvided)
        {
            return !Regex.IsMatch(houseProvided.DocumentNum, @"^[а-яА-Я0-9]+$") ? " Номер документа должен быть указан. Номер документа может содержать только русские буквы и/или цифры" : null;
        }

    }
}