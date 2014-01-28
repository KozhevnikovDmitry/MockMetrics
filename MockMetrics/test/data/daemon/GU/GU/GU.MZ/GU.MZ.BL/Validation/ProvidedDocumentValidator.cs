using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.Validation
{
    public class ProvidedDocumentValidator : AbstractDomainValidator<ProvidedDocument>
    {
        public ProvidedDocumentValidator()
        {
            var doc = ProvidedDocument.CreateInstance();
            _validationActions[Util.GetPropertyName(() => doc.Name)] = ValidateName;
            _validationActions[Util.GetPropertyName(() => doc.Quantity)] = ValidateQuantity;
        }

        private string ValidateQuantity(ProvidedDocument doc)
        {
            return doc.Quantity <= 0 ? "Поле количество документов должно быть заполнено" : null;
        }

        private string ValidateName(ProvidedDocument doc)
        {
            return string.IsNullOrEmpty(doc.Name) ? "Поле наименование документов должно быть заполнено" : null;
        }
    }
}
