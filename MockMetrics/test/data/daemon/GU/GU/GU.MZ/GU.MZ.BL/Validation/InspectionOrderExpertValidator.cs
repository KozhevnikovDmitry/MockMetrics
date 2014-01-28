using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Validation
{
    public class InspectionOrderExpertValidator : AbstractDomainValidator<InspectionOrderExpert>
    {
        public InspectionOrderExpertValidator()
        {
            var orderExpert = InspectionOrderExpert.CreateInstance();
            _validationActions[Util.GetPropertyName(() => orderExpert.ExpertName)] = ValidateExpertName;
            _validationActions[Util.GetPropertyName(() => orderExpert.ExpertPosition)] = ValidateExpertPosition;
        }

        private string ValidateExpertPosition(InspectionOrderExpert order)
        {
            return string.IsNullOrEmpty(order.ExpertPosition) ? "Поле должность эксперта должно быть заполнено" : null;
        }

        private string ValidateExpertName(InspectionOrderExpert order)
        {
            return string.IsNullOrEmpty(order.ExpertName) ? "Поле ФИО эксперта должно быть заполнено" : null;
        }
    }
}
