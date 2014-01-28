using System;
using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class ClaimQueuePrivRegValidator : AbstractDomainValidator<QueuePrivReg>
    {
        public ClaimQueuePrivRegValidator()
        {
            var t = QueuePrivReg.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.DateLaw)] = ValidateDateLaw;
            _validationActions[Util.GetPropertyName(() => t.DecisionDate)] = ValidateDecisionDate;
            _validationActions[Util.GetPropertyName(() => t.DecisionNum)] = ValidateDecisionNum;
            _validationActions[Util.GetPropertyName(() => t.DocumentDate)] = ValidateDocumentDate;
            _validationActions[Util.GetPropertyName(() => t.DocumentNum)] = ValidateDocumentNum;
            _validationActions[Util.GetPropertyName(() => t.QpBaseRegTypeId)] = ValidateQpBaseRegTypeId;
        }

        private string ValidateDecisionDate(QueuePrivReg queuePrivReg)
        {
            return queuePrivReg.DecisionDate == null ? "Постановка в очередь внеочередников. Дата решения должна быть указана." : null;
        }

        private string ValidateDecisionNum(QueuePrivReg queuePrivReg)
        {
            return !Regex.IsMatch(queuePrivReg.DecisionNum, @"^[а-яА-Я0-9]+$") ? "Постановка в очередь внеочередников. Номер решения должен быть указан. Номер решения может содержать только русские буквы и/или цифры" : null;
        }

        private string ValidateDocumentDate(QueuePrivReg queuePrivReg)
        {
            return queuePrivReg.DocumentDate == null ? "Постановка в очередь внеочередников. Дата документа должна быть указана." : null;
        }

        private string ValidateDocumentNum(QueuePrivReg queuePrivReg)
        {
            return !Regex.IsMatch(queuePrivReg.DocumentNum, @"^[а-яА-Я0-9]+$") ? "Постановка в очередь внеочередников. Номер документа должен быть указан. Номер документа может содержать только русские буквы и/или цифры" : null;
        }

        private string ValidateQpBaseRegTypeId(QueuePrivReg queuePrivReg)
        {
            return queuePrivReg.QpBaseRegTypeId == null ? "Постановка в очередь внеочередников. Необходимо указать причину регистрации заявки." : null;
        }

        private string ValidateDateLaw(QueuePrivReg queuePrivReg)
        {
            return queuePrivReg.DateLaw == null ? "Постановка в очередь внеочередников. Дата возникновения права должна быть указана." : null;
        }

    }
}