using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class PersonDisabilityValidator : AbstractDomainValidator<PersonDisability>
    {
        public PersonDisabilityValidator()
        {
            var personDisability = PersonDisability.CreateInstance();
            _validationActions[Util.GetPropertyName(() => personDisability.DisabilityTypeId)] = ValidateDisabilityTypeId;
            _validationActions[Util.GetPropertyName(() => personDisability.DisabilityDate)] = ValidateDisabilityDate;
            _validationActions[Util.GetPropertyName(() => personDisability.IsPoorDocDate)] = ValidateIsPoorDocDate;
            _validationActions[Util.GetPropertyName(() => personDisability.IsPoorDocNum)] = ValidateIsPoorDocNum;
            _validationActions[Util.GetPropertyName(() => personDisability.UsznDocDate)] = ValidateUsznDocDate;
            _validationActions[Util.GetPropertyName(() => personDisability.UsznDocNum)] = ValidateUsznDocNum;
        }

        private string ValidateDisabilityTypeId(PersonDisability personDisability)
        {
            return personDisability.DisabilityTypeId < 1 ? "необходимо указать тип инвалидности" : null; 
        }

        private string ValidateDisabilityDate(PersonDisability personDisability)
        {
            return personDisability.DisabilityDate == null
                       ? "Необходимо указать дату по которую установлена инвалидность"
                       : null;
        }

        private string ValidateIsPoorDocDate(PersonDisability personDisability)
        {
            return personDisability.IsPoor == 1 && personDisability.IsPoorDocDate == null
                       ? "Необходимо указать дату решения управления социальной защиты населения о признании малоимущим"
                       : null;
        }

        private string ValidateIsPoorDocNum(PersonDisability personDisability)
        {
            return personDisability.IsPoor == 1 && string.IsNullOrEmpty(personDisability.IsPoorDocNum)
                       ? "Необходимо указать номер решения управления социальной защиты населения о признании малоимущим"
                       : null;
        }

        private string ValidateUsznDocDate(PersonDisability personDisability)
        {
            return personDisability.UsznDocDate == null
                       ? "Необходимо указать дату решения управления социальной защиты населения"
                       : null;
        }

        private string ValidateUsznDocNum(PersonDisability personDisability)
        {
            return string.IsNullOrEmpty(personDisability.UsznDocNum)
                       ? "Необходимо указать номер решения управления соципальной защиты населения"
                       : null;
        }
    }
}