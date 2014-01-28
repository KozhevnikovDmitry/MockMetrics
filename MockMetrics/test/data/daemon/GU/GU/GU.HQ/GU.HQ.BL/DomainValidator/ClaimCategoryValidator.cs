using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class CategoryValidator : AbstractDomainValidator<ClaimCategory>
    {
        public CategoryValidator()
        {
            var t = ClaimCategory.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.CategoryTypeId)] = ValidateCategoryTypeId;
        }

        /// <summary>
        /// Валидация категорий
        /// </summary>
        /// <param name="claimCategory"></param>
        /// <returns></returns>
        private string ValidateCategoryTypeId(ClaimCategory claimCategory)
        {
            return claimCategory.CategoryTypeId == 0 ? "Поле 'Категория учета' не заполнено или заполнено не верно!" : null;
        }
    }
}