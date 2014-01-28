using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class ClaimQueueDeRegValidator : AbstractDomainValidator<ClaimQueueDeReg>
    {
        public ClaimQueueDeRegValidator()
        {
            var t = ClaimQueueDeReg.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.QueueBaseDeRegTypeId)] = ValidateQueueBaseDeRegTypeId;
            _validationActions[Util.GetPropertyName(() => t.DocumentDate)] = ValidateDocumentDate;
            _validationActions[Util.GetPropertyName(() => t.DocumentNum)] = ValidateDocumentNum;
        }

        private string ValidateQueueBaseDeRegTypeId(ClaimQueueDeReg claimQueueDeReg)
        {
            return claimQueueDeReg.QueueBaseDeRegTypeId == 0
                       ? "Снятие с регистрации. основание снятие с регистрации должно быть указано."
                       : null;
        }


        private string ValidateDocumentDate(ClaimQueueDeReg claimQueueDeReg)
        {
            return claimQueueDeReg.DocumentDate == null ? "Снятие с регистрации. Дата документа должна быть указана." : null;
        }

        private string ValidateDocumentNum(ClaimQueueDeReg claimQueueDeReg)
        {
            return !Regex.IsMatch(claimQueueDeReg.DocumentNum, @"^[а-яА-Я0-9]+$") ? "Снятие с регистрации. Номер документа должен быть указан. Номер документа может содержать только русские буквы и/или цифры." : null;
        }
    }
}