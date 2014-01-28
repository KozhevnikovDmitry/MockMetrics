using System;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimQueuePrivDeRegVM : DomainValidateableVM<QueuePrivDeReg>
    {

        public ClaimQueuePrivDeRegVM(QueuePrivDeReg entity, IDomainValidator<QueuePrivDeReg> domainValidator, bool isValidateable = true)
            : base(entity, domainValidator, isValidateable)
        {}

        #region Binding Properties

        public bool IsPrivDeReg { get { return Entity != null; } }

        /// <summary>
        /// Дата документа – основания утраты права на внеочередное предоставление жилого помещения (74)
        /// </summary>
        public DateTime? DocumentDate
        {
            get { return Entity == null ? (DateTime?)null : Entity.DocumentDate; }
            set { Entity.DocumentDate = value; }
        }

        /// <summary>
        /// Номер документа – основания утраты права на внеочередное предоставление жилого помещения (73)
        /// </summary>
        public string DocumentNum
        {
            get{ return Entity == null ? "" : Entity.DocumentNum; }
            set { Entity.DocumentNum = value; }
        }

        /// <summary>
        /// Дата решения об исключении из списка на внеочередное предоставление жилого помещения (76)
        /// </summary>
        public DateTime? DecisionDate
        {
            get { return Entity == null ? (DateTime?)null : Entity.DecisionDate; }
            set { Entity.DecisionDate = value; }
        }

        /// <summary>
        /// Номер документа – основания о включении в список на внеочередное предоставление жилого помещения (77)
        /// </summary>
        public string DecisionNum
        {
            get { return Entity == null ? "" : Entity.DecisionNum; }
            set { Entity.DecisionNum = value; }
        }

        /// <summary>
        /// примечание  (75)
        /// </summary>
        public string Note
        {
            get { return Entity == null ? "": Entity.Note; }
            set { Entity.Note = value; }
        }
        #endregion Binding Properties
    }
}
