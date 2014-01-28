using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Inspect;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Класс валидатор сущностей Документарная проверка 
    /// </summary>
    public class DocumentExpertiseValidator : AbstractDomainValidator<DocumentExpertise>
    {
        private readonly IDomainValidator<DocumentExpertiseResult> _docExpResValidator;

        public DocumentExpertiseValidator(IDomainValidator<DocumentExpertiseResult> docExpResValidator)
        {
            _docExpResValidator = docExpResValidator;
            var documentExpertise = DocumentExpertise.CreateInstance();

            _validationActions[Util.GetPropertyName(() => documentExpertise.ActStamp)] = ValidateActStamp;
            _validationActions[Util.GetPropertyName(() => documentExpertise.StartStamp)] = ValidateStartStamp;
            _validationActions[Util.GetPropertyName(() => documentExpertise.EndStamp)] = ValidateEndStamp;
        } 
        
        public override ValidationErrorInfo Validate(DocumentExpertise domainObject)
        {
            var result = base.Validate(domainObject);

            foreach (var expResult in domainObject.ExperiseResultList)
            {
                var expResValInfo = _docExpResValidator.Validate(expResult);
                result.AddResult(expResValInfo, "Результат документарной проверки");
            }

            return result;
        }

        private string ValidateActStamp(DocumentExpertise documentExpertise)
        {
            if (documentExpertise.ActStamp == new DateTime())
                return "Поле Дата акта проверки должно быть заполнено";

            if (documentExpertise.ActStamp < documentExpertise.EndStamp)
            {
                return "Дата акта не может быть ранее даты окончания проверки";
            }

            return null;
        }

        private string ValidateStartStamp(DocumentExpertise documentExpertise)
        {
            if (documentExpertise.StartStamp == new DateTime())
                return "Поле Дата начала проверки должно быть заполнено";

            if (documentExpertise.StartStamp > documentExpertise.EndStamp)
            {
                return "Дата начала проверки не может быть позднее даты окончания проверки";
            }

            return null;
        }

        private string ValidateEndStamp(DocumentExpertise documentExpertise)
        {
            if (documentExpertise.EndStamp == new DateTime())
                return "Поле Дата окончания проверки должно быть заполнено";

            if (documentExpertise.StartStamp > documentExpertise.EndStamp)
            {
                return "Дата начала проверки не может быть позднее даты окончания проверки";
            }

            return null;
        }
    }
}
