using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.BL.Validation
{
    public class IndRequisitesValidator : AbstractDomainValidator<IndRequisites>
    {
        public IndRequisitesValidator()
        {
            var indReqs = IndRequisites.CreateInstance();
            _validationActions[Util.GetPropertyName(() => indReqs.Surname)] = ValidateSurname;
            _validationActions[Util.GetPropertyName(() => indReqs.Name)] = ValidateName;
            _validationActions[Util.GetPropertyName(() => indReqs.Patronymic)] = ValidatePatronymic;
            _validationActions[Util.GetPropertyName(() => indReqs.Serial)] = ValidateSerial;
            _validationActions[Util.GetPropertyName(() => indReqs.Number)] = ValidateNumber;
            _validationActions[Util.GetPropertyName(() => indReqs.Organization)] = ValidateOrganization;
            _validationActions[Util.GetPropertyName(() => indReqs.Stamp)] = ValidateStamp;
        }

        private string ValidateStamp(IndRequisites arg)
        {
            return arg.Stamp == new DateTime() ? "Поле Дата выдачи документа должно быть заполнено" : null;
        }

        private string ValidateOrganization(IndRequisites arg)
        {
            return string.IsNullOrEmpty(arg.Organization) ? "Поле Организация выдавшая документ должно быть заполнено" : null;
        }

        private string ValidateNumber(IndRequisites arg)
        {
            return string.IsNullOrEmpty(arg.Number) ? "Поле Номер документа должно быть заполнено" : null;
        }

        private string ValidateSerial(IndRequisites arg)
        {
            return string.IsNullOrEmpty(arg.Serial) ? "Поле Серия документа должно быть заполнено" : null;
        }

        private string ValidatePatronymic(IndRequisites arg)
        {
            return string.IsNullOrEmpty(arg.Patronymic) ? "Поле отчество должно быть заполнено" : null;
        }

        private string ValidateName(IndRequisites arg)
        {
            return string.IsNullOrEmpty(arg.Name) ? "Поле Имя должно быть заполнено" : null;
        }

        private string ValidateSurname(IndRequisites arg)
        {
            return string.IsNullOrEmpty(arg.Surname) ? "Поле Фамилия должно быть заполнено" : null;
        }
    }
}