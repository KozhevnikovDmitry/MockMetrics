using System;
using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.BL;
using GU.HQ.BL.Policy;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimHouseProvidedVM : DomainValidateableVM<HouseProvided>
    {
        private readonly IClaimStatusPolicy _claimStatusPolicy = HqFacade.GetClaimStatusPolicy();

        public ClaimHouseProvidedVM(HouseProvided entity, IDomainValidator<HouseProvided> domainValidator, bool isValidateable = true) :
            base(entity, domainValidator, isValidateable)
        {
            HouseProvidedPrivTypeList = HqFacade.GetDictionaryManager().GetDictionary<QueuePrivBaseRegType>();
        }

        #region Binding Properties

        /// <summary>
        /// Справочник превелигерованное предоставлеине жилья
        /// </summary>
        public List<QueuePrivBaseRegType> HouseProvidedPrivTypeList { get; private set; }

        /// <summary>
        /// внеочередное предоставление жилья
        /// </summary>
        public bool IsPrivHouseProvided
        {
            get { return Entity != null && Entity.IsPrivHouseProvided != 0; }
            set
            {
                Entity.IsPrivHouseProvided = value ? 1 : 0;
                RaisePropertyChanged(() => IsPrivHouseProvided);
            }
        }

        /// <summary>
        /// Основание внеочередного предоставления жилья
        /// </summary>
        public int? HouseProvidedPrivBaseTypeId
        {
            get { return Entity == null ? 0 : Entity.HouseProvidedPrivBaseTypeId; }
            set { Entity.HouseProvidedPrivBaseTypeId = value; }
        }

        /// <summary>
        /// Дата решения о предоставлении жилого помещения
        /// </summary>
        public DateTime? DocumentDate 
        {
            get { return Entity == null ? null : Entity.DocumentDate; }
            set { Entity.DocumentDate = value; }
        }

        /// <summary>
        /// Номер решения о предоставлении жилого помещения
        /// </summary>
        public string DocumentNum
        {
            get { return Entity == null ? "" : Entity.DocumentNum; }
            set { Entity.DocumentNum = value; }
        }

        /// <summary>
        /// Данные о наличии жилых помещений на праве собственности после предоставления жилого помещения по договору социального найма
        /// </summary>
        public string HomeOwn
        {
            get { return Entity == null ? "" : Entity.HomeOwn; }
            set { Entity.HomeOwn = value; }
        }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Note
        {
            get { return Entity == null ? "" : Entity.Note; }
            set {  Entity.Note = value; }
        }

        #endregion Binding Properties
    }
}
