using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class DeclarerBaseRegItemValidator : AbstractDomainValidator<DeclarerBaseRegItem>
    {
        public DeclarerBaseRegItemValidator()
        {
            var t = DeclarerBaseRegItem.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.QueueBaseRegTypeId)] = ValidateQueueBaseRegTypeId;
        }

        /// <summary>
        /// Валидация основания указанного заявителем
        /// </summary>
        /// <param name="declarerBaseRegItem"></param>
        /// <returns></returns>
        private string ValidateQueueBaseRegTypeId(DeclarerBaseRegItem declarerBaseRegItem)
        {
            return declarerBaseRegItem.QueueBaseRegTypeId <= 0 ? "Поле 'Причина' не заполнено или заполнено не верно!" : null;
        }
    }
}
