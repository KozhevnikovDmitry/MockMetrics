using System;
using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class ClaimQueuePrivDeRegValidator : AbstractDomainValidator<QueuePrivDeReg>
    {
         public ClaimQueuePrivDeRegValidator()
         {
             var t = QueuePrivDeReg.CreateInstance();
             _validationActions[Util.GetPropertyName(() => t.DecisionDate)] = ValidateDecisionDate;
             _validationActions[Util.GetPropertyName(() => t.DecisionNum)] = ValidateDecisionNum;
             _validationActions[Util.GetPropertyName(() => t.DocumentDate)] = ValidateDocumentDate;
             _validationActions[Util.GetPropertyName(() => t.DocumentNum)] = ValidateDocumentNum;
         }

         private string ValidateDecisionDate(QueuePrivDeReg queuePrivReg)
         {
             return queuePrivReg.DecisionDate == null ? " Снятие с регистрации внеочередников. Дата решения должна быть указана." : null;
         }

         private string ValidateDecisionNum(QueuePrivDeReg queuePrivReg)
         {
             return !Regex.IsMatch(queuePrivReg.DecisionNum, @"^[а-яА-Я0-9]+$") ? "Снятие с регистрации внеочередников. Номер решения должен быть указан. Номер решения может содержать только русские буквы и/или цифры." : null;
         }

         private string ValidateDocumentDate(QueuePrivDeReg queuePrivReg)
         {
             return queuePrivReg.DocumentDate == DateTime.MinValue ? "Снятие с регистрации внеочередников. Дата документа должна быть указана." : null;
         }

         private string ValidateDocumentNum(QueuePrivDeReg queuePrivReg)
         {
             return !Regex.IsMatch(queuePrivReg.DocumentNum, @"^[а-яА-Я0-9]+$") ? "Снятие с регистрации внеочередников. Номер документа должен быть указан. Номер документа может содержать только русские буквы и/или цифры." : null;
         }

    }
}