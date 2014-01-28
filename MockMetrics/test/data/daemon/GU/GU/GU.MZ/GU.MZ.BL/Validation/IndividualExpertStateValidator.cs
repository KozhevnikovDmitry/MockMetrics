using Common.BL.Validation;
using Common.Types;

using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Валидатор экспертов физических лиц
    /// </summary>
    public class IndividualExpertStateValidator : AbstractDomainValidator<IndividualExpertState>, IDomainValidator<IExpertState>, IExpertStateValidator
    {
        public ExpertStateType ExpertStateType 
        {
            get
            {
                return ExpertStateType.Individual;
            }
        }

        /// <summary>
        /// Валидатор экспертов физических лиц
        /// </summary>
        public IndividualExpertStateValidator()
        {
            var expertState = IndividualExpertState.CreateInstance();
            _validationActions[Util.GetPropertyName(() => expertState.Name)] = ValidateName;
            _validationActions[Util.GetPropertyName(() => expertState.Surname)] = ValidateSurname;
            _validationActions[Util.GetPropertyName(() => expertState.Patronymic)] = ValidatePatronymic;
            _validationActions[Util.GetPropertyName(() => expertState.Position)] = ValidatePosition;
            _validationActions[Util.GetPropertyName(() => expertState.OrganizationName)] = ValidateOrganizationName;
        }

        #region Overrides of IDomainValidator<IExpertState>

        public ValidationErrorInfo Validate(IExpertState domainObject)
        {
            return (this as IDomainValidator<IndividualExpertState>).Validate(domainObject as IndividualExpertState);
        }

        public string ValidateProperty(IExpertState domainObject, string propertyName)
        {
            return (this as IDomainValidator<IndividualExpertState>).ValidateProperty(domainObject as IndividualExpertState, propertyName);
        }

        #endregion


        #region Overrides of AbstractDomainValidator<IndividualExpertState>

        /// <summary>
        /// Валидирует свойства доменного объекта. Возвращает результаты валидации.
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <returns>Объект хранящий результаты валидации</returns>
        public override ValidationErrorInfo Validate(IndividualExpertState domainObject)
        {
            return domainObject == null ? new ValidationErrorInfo() : base.Validate(domainObject);
        }

        private string ValidateOrganizationName(IndividualExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.OrganizationName) ? "Поле Организация должно быть заполнено" : null;
        }

        private string ValidateName(IndividualExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.Name) ? "Поле Имя должно быть заполнено" : null;
        }

        private string ValidateSurname(IndividualExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.Surname) ? "Поле Фамилия должно быть заполнено" : null;
        }

        private string ValidatePatronymic(IndividualExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.Patronymic) ? "Поле Отчество должно быть заполнено" : null;
        }

        private string ValidatePosition(IndividualExpertState expertState)
        {
            return string.IsNullOrEmpty(expertState.Position) ? "Поле Должность должно быть заполнено" : null;
        }

        #endregion
    }
}
