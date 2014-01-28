using System;
using Common.BL.Validation;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.ListViewModel;
using GU.BL;
using GU.HQ.BL;
using GU.HQ.DataModel;


namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimQueuePrivItemVM : AbstractListItemVM<QueuePriv>
    {
        public ClaimQueuePrivItemVM(QueuePriv entity, IDomainValidator<QueuePriv> domainValidator, bool isValidateable) 
            : base(entity, domainValidator, isValidateable)
        {
            ClaimQueuePrivRegVm =  UIFacade.GetDomainValidateableVM(entity.QueuePrivReg);
            ClaimQueuePrivDeRegVm = UIFacade.GetDomainValidateableVM(entity.QueuePrivDeReg);

            IsExpanded = Entity.QueuePrivDeRegId == null;
        }

        protected override void Initialize()
        {
        }

        #region Binding Properties

        #region Common

        public bool IsPrivReg{ get { return Entity.QueuePrivDeReg == null; } }

        public bool IsPrivDeReg { get { return Entity.QueuePrivDeReg != null; } }

        public bool IsExpanded { get; set; }

        public string ExpanderHeader { get { return "Право на внеочередное жильё от " + Entity.QueuePrivReg.DateLaw.Value.ToString("dd.MM.yyyy"); } }

        #endregion Common

        /// <summary>
        /// информация о регистрации в очереди внеочередников
        /// </summary>
        private IDomainValidateableVM<QueuePrivReg> _claimQueuePrivRegVm;
        public IDomainValidateableVM<QueuePrivReg> ClaimQueuePrivRegVm
        {
            get { return _claimQueuePrivRegVm; }
            set
            {
                if (_claimQueuePrivRegVm == value) return;
                _claimQueuePrivRegVm = value;
                RaisePropertyChanged(() => ClaimQueuePrivRegVm);
            }
        }

        /// <summary>
        /// информация о снятии с регистрации
        /// </summary>
        private IDomainValidateableVM<QueuePrivDeReg>  _claimQueuePrivDeRegVm;
        public IDomainValidateableVM<QueuePrivDeReg> ClaimQueuePrivDeRegVm
        {
            get { return _claimQueuePrivDeRegVm; }
            set
            {
                if (_claimQueuePrivDeRegVm == value) return;
                _claimQueuePrivDeRegVm = value;
                RaisePropertyChanged(() => ClaimQueuePrivDeRegVm);
            }
        }
        #endregion Binding Properties
    }
}