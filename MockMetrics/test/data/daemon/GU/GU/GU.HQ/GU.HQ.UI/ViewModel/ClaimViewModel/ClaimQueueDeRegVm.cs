using System;
using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;


namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimQueueDeRegVM :  DomainValidateableVM<ClaimQueueDeReg>
    {
        public ClaimQueueDeRegVM(ClaimQueueDeReg entity, IDomainValidator<ClaimQueueDeReg> validator, bool isValidateable = true) :
            base(entity, validator, isValidateable)
        {
            QueueBaseDeRegList = HqFacade.GetDictionaryManager().GetDictionary<QueueBaseDeRegType>();
        }

        #region Binding Properties

        /// <summary>
        /// Заявление снято с учета
        /// </summary>
        public bool IsRejected{ get { return Entity != null; } }

        /// <summary>
        /// Дата документа снятия с регистраци
        /// </summary>
        public DateTime? DocumentDate 
        {
            get { return Entity == null ? (DateTime?) null : Entity.DocumentDate; }
            set { Entity.DocumentDate = value; }
        }

        /// <summary>
        /// Номер документа снятия с регистрации:"
        /// </summary>
        public string DocumentNum
        {
            get { return Entity == null ? "" : Entity.DocumentNum; }
            set 
            {
                Entity.DocumentNum = value;
            }
        }

        /// <summary>
        /// Список оснований
        /// </summary>
        public List<QueueBaseDeRegType> QueueBaseDeRegList { get; private set; }


        /// <summary>
        /// ID основания снятия с учета
        /// </summary>
        public int QueueBaseDeRegTypeId
        {
            get { return Entity == null ? 0 : Entity.QueueBaseDeRegTypeId; }
            set
            {
                Entity.QueueBaseDeRegTypeId = value;
            }
        }

        #endregion Binding Properties
    }
}
