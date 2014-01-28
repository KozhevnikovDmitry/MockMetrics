using System;
using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class ClaimQueueRegValidator : AbstractDomainValidator<ClaimQueueReg>
    {
        public ClaimQueueRegValidator()
        {
            var t = ClaimQueueReg.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.DocumentDate)] = ValidateDocumentDate;
            _validationActions[Util.GetPropertyName(() => t.DocumetNumber)] = ValidateDocumetNumber;
            _validationActions[Util.GetPropertyName(() => t.QueueBaseRegTypeId)] = ValidateQueueBaseRegTypeId;
        }

         private string ValidateDocumentDate(ClaimQueueReg claimQueueReg)
         {
             return claimQueueReg.DocumentDate == null ? "Постановка в очередь. Дата документа должна быть указана." : null;
         }

        private string ValidateDocumetNumber(ClaimQueueReg claimQueueReg)
        {
            return !Regex.IsMatch(claimQueueReg.DocumetNumber, @"^[а-яА-Я0-9]+$") ? "Постановка в очередь. Номер документа может содержать только русские буквы и/или цифры." : null;
        }

        private string ValidateQueueBaseRegTypeId(ClaimQueueReg claimQueueReg)
        {
            return claimQueueReg.QueueBaseRegTypeId == null ? "Постановка в очередь. Необходимо указать причину регистрации заявки." : null;
        }
    }
}