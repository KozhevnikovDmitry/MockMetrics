using System;
using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;


namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimQueuePrivRegVM : DomainValidateableVM<QueuePrivReg>
    {
        public ClaimQueuePrivRegVM(QueuePrivReg entity, IDomainValidator<QueuePrivReg> domainValidator, bool isValidateable = true)
            : base(entity, domainValidator, isValidateable)
        {
            QueuePrivBaseRegTypeList = HqFacade.GetDictionaryManager().GetDictionary<QueuePrivBaseRegType>();
        }

        #region Binding Properties

        /// <summary>
        /// Список оснований регистрации
        /// </summary>
        public List<QueuePrivBaseRegType> QueuePrivBaseRegTypeList { get; private set; }


        /// <summary>
        /// основание регистрации в очереди внеочередников
        /// </summary>
        public int? QpBaseRegTypeId
        {
            get { return Entity.QpBaseRegTypeId; }
            set { Entity.QpBaseRegTypeId = value; }
        }

        /// <summary>
        /// Дата решения о включении в список на внеочередное предоставление жилого помещения (64)
        /// </summary>
        public DateTime? DecisionDate
        {
            get { return Entity.DecisionDate; }
            set { Entity.DecisionDate = value; }
        }

        /// <summary>
        /// Номер решения о включении в список на внеочередное предоставление жилого помещения (65)
        /// </summary>
        public string DecisionNum
        {
            get { return Entity.DecisionNum; }
            set { Entity.DecisionNum = value; }
        }

        /// <summary>
        /// Дата документа - основания решения о включении в список на внеочередное предоставление жилого помещения (67)
        /// </summary>
        public DateTime? DocumentDate
        {
            get { return Entity.DocumentDate; }
            set { Entity.DocumentDate = value; }
        }

        /// <summary>
        /// Номер документа – основания о включении в список на внеочередное предоставление жилого помещения (68)
        /// </summary>
        public string DocumentNum
        {
            get { return Entity.DocumentNum; }
            set { Entity.DocumentNum = value; }
        }


        /// <summary>
        /// Дата возникновения права на внеочередное предоставление жилого помещения (63)
        /// </summary>
        public DateTime? DateLaw
        {
            get { return Entity.DateLaw; }
            set { Entity.DateLaw = value; }
        }

        #endregion Binding Properties
    }
}
