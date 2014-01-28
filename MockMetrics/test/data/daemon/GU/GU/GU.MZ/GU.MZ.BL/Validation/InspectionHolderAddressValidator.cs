using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Validation
{
    public class InspectionHolderAddressValidator : AbstractDomainValidator<InspectionHolderAddress>
    {
        public InspectionHolderAddressValidator()
        {
            var orderAgree = InspectionHolderAddress.CreateInstance();
            _validationActions[Util.GetPropertyName(() => orderAgree.Address)] = ValidateAddress;
        }

        private string ValidateAddress(InspectionHolderAddress order)
        {
            return string.IsNullOrEmpty(order.Address) ? "Поле адрес осуществления деятельности должно быть заполнено" : null;
        }
    }
}
