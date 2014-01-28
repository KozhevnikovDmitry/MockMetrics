using System;
using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;


namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimQueueRegVM : DomainValidateableVM<ClaimQueueReg>
    {
        public ClaimQueueRegVM(ClaimQueueReg entity, IDomainValidator<ClaimQueueReg> domainValidator, bool isValidateable = true) :
            base(entity, domainValidator, isValidateable)
        {
            BaseRegTypeList = HqFacade.GetDictionaryManager().GetDictionary<QueueBaseRegType>();
        }

        #region Binding Properties

        /// <summary>
        /// Список оснований регистрации
        /// </summary>
        public List<QueueBaseRegType> BaseRegTypeList { get; private set; }


        /// <summary>
        /// ID основания принятия заявления
        /// </summary>
        public int? QueueBaseRegTypeId 
        {
            get { return Entity.QueueBaseRegTypeId; }  
            set
            {
                Entity.QueueBaseRegTypeId = value;
            }
        }


        /// <summary>
        /// дата постановки на учет
        /// </summary>
        public DateTime? DocumentDate
        {
            get { return Entity == null ? (DateTime?) null: Entity.DocumentDate; }
            set
            {
                Entity.DocumentDate = value;
            }
        }

        /// <summary>
        /// Номер решения о постановке на учет
        /// </summary>
        public string DocumetNumber
        {
            get { return Entity == null? "" : Entity.DocumetNumber; }
            set
            {
                Entity.DocumetNumber = value;
            }
        }

        /// <summary>
        /// Регистрационный номер решения органа, осуществляющего принятие на учет
        /// </summary>
        public string AreaRegistrationNumber
        {
            get { return Entity == null ? "" : Entity.AreaRegistrationNumber; }
            set
            {
                Entity.AreaRegistrationNumber = value;
            }
        }

        #endregion Binding Properties
    }
}
