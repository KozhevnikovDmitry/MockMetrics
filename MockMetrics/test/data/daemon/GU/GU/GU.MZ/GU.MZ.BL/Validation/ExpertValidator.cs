using System.Collections.Generic;
using System.Linq;
using Common.BL.Validation;
using Common.Types;

using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Класс валидатор сущностей Эксперт
    /// </summary>
    public class ExpertValidator : AbstractDomainValidator<Expert>
    {
        private readonly IEnumerable<IExpertStateValidator> _expertStateValidators;

        /// <summary>
        /// Класс валидатор сущностей Эксперт
        /// </summary>
        public ExpertValidator(IEnumerable<IExpertStateValidator> expertStateValidators)
        {
            _expertStateValidators = expertStateValidators;
            var expert = Expert.CreateInstance();
            _validationActions[Util.GetPropertyName(() => expert.AccreditateDocumentNumber)] = ValidateAccreditateDocumentNumber;
            _validationActions[Util.GetPropertyName(() => expert.AccreditationDueDate)] = ValidateAccreditationDueDate;
            _validationActions[Util.GetPropertyName(() => expert.AccreditateActivityid)] = ValidateAccreditateActivity;
        }

        #region Overrides of AbstractDomainValidator<Expert>

        /// <summary>
        /// Валидирует свойства доменного объекта. Возвращает результаты валидации.
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <returns>Объект хранящий результаты валидации</returns>
        public override ValidationErrorInfo Validate(Expert domainObject)
        {
            var errorInfo = base.Validate(domainObject);
            errorInfo.AddResult(_expertStateValidators.Single(t => t.ExpertStateType == domainObject.ExpertStateType)
                                                     .Validate(domainObject.ExpertState));
            return errorInfo;
        }

        #endregion

        private string ValidateAccreditateActivity(Expert expert)
        {
            return expert.AccreditateActivityid == 0 ? "Поле Деятельность должно быть заполнено" : null;
        }

        private string ValidateAccreditationDueDate(Expert expert)
        {
            return !expert.AccreditationDueDate.HasValue ? "Поле Дата окончания аккредитации должно быть заполнено" : null;
        }

        private string ValidateAccreditateDocumentNumber(Expert expert)
        {
            return string.IsNullOrEmpty(expert.AccreditateDocumentNumber) ? "Поле № свидетельства должно быть заполнено" : null;
        }
    }
}
