using System.Linq;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.Validation
{
    /// <summary>
    /// Валидатор заявки
    /// </summary>
    public class ClaimValidator : AbstractDomainValidator<Claim>
    {
        private readonly IDomainValidator<Person> _personValidator; 
        private readonly IDomainValidator<DeclarerRelative> _declarerRelativeValdator;
        private readonly IDomainValidator<ClaimQueueReg> _claimQueueRegValidator;
        private readonly IDomainValidator<QueuePrivReg> _claimQueuePrivRegValidator;
        private readonly IDomainValidator<QueuePrivDeReg> _claimQueuePrivDeRegValidator;
        private readonly IDomainValidator<HouseProvided> _houseProvidedValidator;
        private readonly IDomainValidator<ClaimQueueDeReg> _claimQueueDeRegValidator;
        private readonly IDomainValidator<Address> _addressValidator;
        private readonly IDomainValidator<AddressDesc> _addressDescValidator;
        private readonly IDomainValidator<Notice> _noticeValidator;
        private readonly IDomainValidator<DeclarerBaseRegItem> _declarerBaseRegItemValidator;
        private readonly IDomainValidator<DeclarerBaseReg> _declarerBaseRegValidator;
        private readonly IDomainValidator<PersonDoc> _personDocValidator;
        private readonly IDomainValidator<ClaimCategory> _claimCategoryValidator;

        public ClaimValidator(IDomainValidator<Person> personValidator,
                            IDomainValidator<DeclarerRelative> declarerRelativeValdator, 
                            IDomainValidator<ClaimQueueReg> claimQueueRegValidator,
                            IDomainValidator<QueuePrivReg> claimQueuePrivRegValidator,
                            IDomainValidator<QueuePrivDeReg> claimQueuePrivDeRegValidator,
                            IDomainValidator<HouseProvided> houseProvidedValidator,
                            IDomainValidator<ClaimQueueDeReg> claimQueueDeRegValidator,
                            IDomainValidator<Address> addressValidator,
                            IDomainValidator<AddressDesc> addressDescValidator,
                            IDomainValidator<Notice>  noticeValidator,
                            IDomainValidator<DeclarerBaseRegItem> declarerBaseRegItemValidator,
                            IDomainValidator<DeclarerBaseReg> declarerBaseRegValidator,
                            IDomainValidator<PersonDoc> personDocValidator,
                            IDomainValidator<ClaimCategory> claimCategoryValidator)
        {
            _personValidator = personValidator;
            _declarerRelativeValdator = declarerRelativeValdator;
            _claimQueueRegValidator = claimQueueRegValidator;
            _claimQueuePrivRegValidator = claimQueuePrivRegValidator;
            _claimQueuePrivDeRegValidator = claimQueuePrivDeRegValidator;
            _houseProvidedValidator = houseProvidedValidator;
            _claimQueueDeRegValidator = claimQueueDeRegValidator;
            _addressValidator = addressValidator;
            _addressDescValidator = addressDescValidator;
            _noticeValidator = noticeValidator;
            _declarerBaseRegValidator = declarerBaseRegValidator;
            _declarerBaseRegItemValidator = declarerBaseRegItemValidator;
            _personDocValidator = personDocValidator;
            _claimCategoryValidator = claimCategoryValidator;


            var t = Claim.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.ClaimCategories)] = ValidateClaimCategories;
        }

        #region Overrides of AbstractDomainValidator<Claim>

        /// <summary>
        /// Валидирует свойства доменного объекта. Возвращает результаты валидации.
        /// </summary>
        /// <param name="claim">Объект заявка</param>
        /// <returns>Объект хранящий результаты валидации</returns>
        public override ValidationErrorInfo Validate(Claim claim)
        {
            //данные Claim
            var errorInfo = base.Validate(claim);
            
            //завитель
            if(claim.Declarer != null)
                errorInfo.AddResult(_personValidator.Validate(claim.Declarer));

            //список окументов заявителя
            if (claim.Declarer != null && claim.Declarer.Documents != null)
                claim.Declarer.Documents.ForEach(t => errorInfo.AddResult(_personDocValidator.Validate(t)));

            //список родственников
            if (claim.Relatives != null)
                claim.Relatives.ForEach(t => errorInfo.AddResult(_declarerRelativeValdator.Validate(t)));
            
            // постановка на учет
            if(claim.QueueReg != null)
                errorInfo.AddResult(_claimQueueRegValidator.Validate(claim.QueueReg));

            // категории учета
            if (claim.ClaimCategories != null)
                claim.ClaimCategories.ForEach(t => errorInfo.AddResult(_claimCategoryValidator.Validate(t)));

            // уведомления
            if (claim.Notices != null)
                claim.Notices.ForEach(t => errorInfo.AddResult(_noticeValidator.Validate(t)));

            //  основания указанные заявителем при подаче заявления о постановке на учет
            if (claim.DeclarerBaseReg != null)
                errorInfo.AddResult(_declarerBaseRegValidator.Validate(claim.DeclarerBaseReg));

            // основания указанные заявителем при подаче заявления о постановке на учет непосредственно список
            if (claim.DeclarerBaseReg != null && claim.DeclarerBaseReg.BaseRegItems != null)
                claim.DeclarerBaseReg.BaseRegItems.ForEach(t => errorInfo.AddResult(_declarerBaseRegItemValidator.Validate(t)));
            
            // постоновка снятие с внеочередников
            if (claim.QueuePrivList != null)
            {
                claim.QueuePrivList.ForEach(t => errorInfo.AddResult(_claimQueuePrivRegValidator.Validate(t.QueuePrivReg)));

                foreach (var el in claim.QueuePrivList.Where(el => el.QueuePrivDeReg != null))
                    errorInfo.AddResult(_claimQueuePrivDeRegValidator.Validate(el.QueuePrivDeReg));
            }

            // предоставленное жильё
            if(claim.HouseProvided != null)
                errorInfo.AddResult(_houseProvidedValidator.Validate(claim.HouseProvided));

            //Адрес предоставленного жилья
            if (claim.HouseProvided != null && claim.HouseProvided.Address != null)
                errorInfo.AddResult(_addressValidator.Validate(claim.HouseProvided.Address));

            //Детализация предоставленного жилья
            if (claim.HouseProvided != null && claim.HouseProvided.Address != null && claim.HouseProvided.Address.AddressDesc != null)
                errorInfo.AddResult(_addressDescValidator.Validate(claim.HouseProvided.Address.AddressDesc));
            
            // исключение из очереди
            if (claim.QueueDeReg != null)
                errorInfo.AddResult(_claimQueueDeRegValidator.Validate(claim.QueueDeReg));

            return errorInfo;
        }

        #endregion

        #region Validate

        /// <summary>
        /// Валидация списка категорий учета заявки
        /// </summary>
        /// <returns></returns>
        private  string ValidateClaimCategories(Claim claim)
        {
            return claim.ClaimCategories.Count() != claim.ClaimCategories.GroupBy(t => t.CategoryTypeId).Count()
                       ? "Две или более категории учета заявки совпадают!"
                       : null;
        }

        #endregion
    }
}
