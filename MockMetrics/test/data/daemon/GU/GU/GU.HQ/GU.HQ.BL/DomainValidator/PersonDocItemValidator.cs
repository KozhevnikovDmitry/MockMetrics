using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class PersonDocItemValidator : AbstractDomainValidator<PersonDoc>
    {
        public PersonDocItemValidator()
        {
            var t = PersonDoc.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.DateDoc)] = ValidateDateDoc;
            _validationActions[Util.GetPropertyName(() => t.DocumentTypeId)] = ValidateDocumentTypeId;
            _validationActions[Util.GetPropertyName(() => t.Seria)] = ValidateSeria;
            _validationActions[Util.GetPropertyName(() => t.Num)] = ValidateNum;
            _validationActions[Util.GetPropertyName(() => t.Org)] = ValidateOrg;
            _validationActions[Util.GetPropertyName(() => t.OrgCode)] = ValidateOrgCode;
        }

        /// <summary>
        /// Валидация даты выдачи документа
        /// </summary>
        /// <param name="personDoc"></param>
        /// <returns></returns>
        private string ValidateDateDoc(PersonDoc personDoc)
        {
            return personDoc.DateDoc == DateTime.MinValue ? "Поле 'Дата выдачи' не заполнено или заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация типа документа
        /// </summary>
        /// <param name="personDoc"></param>
        /// <returns></returns>
        private string ValidateDocumentTypeId(PersonDoc personDoc)
        {
            return personDoc.DocumentTypeId == 0 ? "Поле 'Тип документов' не заполнено или заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация серии
        /// </summary>
        /// <param name="personDoc"></param>
        /// <returns></returns>
        private string ValidateSeria(PersonDoc personDoc)
        {
            return personDoc.Seria.Length <= 0 ? "Поле 'Серия' не заполнено или заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация номера
        /// </summary>
        /// <param name="personDoc"></param>
        /// <returns></returns>
        private string ValidateNum(PersonDoc personDoc)
        {
            return personDoc.Num.Length <= 0 ? "Поле 'Тип документов' не заполнено или заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация организации
        /// </summary>
        /// <param name="personDoc"></param>
        /// <returns></returns>
        private string ValidateOrg(PersonDoc personDoc)
        {
            return personDoc.Org.Length <= 3 ? "Поле 'Кем выдан' не заполнено или заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация кода организации
        /// </summary>
        /// <param name="personDoc"></param>
        /// <returns></returns>
        private string ValidateOrgCode(PersonDoc personDoc)
        {
            return personDoc.OrgCode.Length <= 1 ? "Поле 'Код организации' не заполнено или заполнено не верно!" : null;
        }
    }
}
