using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.BL.Validation
{
    public class JurRequisitesValidator : AbstractDomainValidator<JurRequisites>
    {
        public JurRequisitesValidator()
        {
            var jurReqs = JurRequisites.CreateInstance();
            _validationActions[Util.GetPropertyName(() => jurReqs.FullName)] = ValidateFullName;
            _validationActions[Util.GetPropertyName(() => jurReqs.ShortName)] = ValidateShortName;
            _validationActions[Util.GetPropertyName(() => jurReqs.FirmName)] = ValidateFirmName;
            _validationActions[Util.GetPropertyName(() => jurReqs.HeadName)] = ValidateHeadName;
            _validationActions[Util.GetPropertyName(() => jurReqs.HeadPositionName)] = ValidateHeadPositionName;
            _validationActions[Util.GetPropertyName(() => jurReqs.LegalFormId)] = ValidateLegalFormId;
        }

        private string ValidateLegalFormId(JurRequisites arg)
        {
            return arg.LegalFormId == 0 ? "Поле ОПФ должно быть заполнено" : null;
        }

        private string ValidateHeadPositionName(JurRequisites arg)
        {
            return string.IsNullOrEmpty(arg.HeadPositionName) ? "Поле Должность главы должно быть заполнено" : null;
        }

        private string ValidateHeadName(JurRequisites arg)
        {
            return string.IsNullOrEmpty(arg.HeadName) ? "Поле ФИО главы должно быть заполнено" : null;
        }

        private string ValidateFirmName(JurRequisites arg)
        {
            return string.IsNullOrEmpty(arg.FirmName) ? "Поле Фирменное наименование должно быть заполнено" : null;
        }

        private string ValidateShortName(JurRequisites arg)
        {
            return string.IsNullOrEmpty(arg.ShortName) ? "Поле Краткое наименование должно быть заполнено" : null;
        }

        private string ValidateFullName(JurRequisites arg)
        {
            return string.IsNullOrEmpty(arg.FullName) ? "Поле Полное наименование должно быть заполнено" : null;
        }
    }
}