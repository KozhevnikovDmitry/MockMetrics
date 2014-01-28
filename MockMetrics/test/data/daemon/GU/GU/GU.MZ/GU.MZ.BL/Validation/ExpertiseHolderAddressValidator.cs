using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Validation
{
    public class ExpertiseHolderAddressValidator : AbstractDomainValidator<ExpertiseHolderAddress>
    {
        public ExpertiseHolderAddressValidator()
        {
            var orderAgree = ExpertiseHolderAddress.CreateInstance();
            _validationActions[Util.GetPropertyName(() => orderAgree.Address)] = ValidateAddress;
        }

        private string ValidateAddress(ExpertiseHolderAddress order)
        {
            return string.IsNullOrEmpty(order.Address) ? "Поле адрес осуществления деятельности должно быть заполнено" : null;
        }
    }
}
