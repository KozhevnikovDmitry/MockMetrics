using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Inspect;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Класс валидатор сущностей Результат документарной проверки 
    /// </summary>
    public class DocumentExpertiseResultValidator : AbstractDomainValidator<DocumentExpertiseResult>
    {
        public DocumentExpertiseResultValidator()
        {
            var docExpResult = DocumentExpertiseResult.CreateInstance();
            _validationActions[Util.GetPropertyName(() => docExpResult.DocumentExpertiseId)] = ValidateDocumentExpertise;
        }

        private string ValidateDocumentExpertise(DocumentExpertiseResult documentExpertiseResult)
        {
            return documentExpertiseResult.DocumentExpertiseId == 0
                ? "Поле Проверяемый документ должно быть заполнено"
                : null;
        }
    }
}
