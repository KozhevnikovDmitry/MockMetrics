using Common.BL.DataMapping;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;
using Common.UI.WeakEvent.EventSubscriber;
using GU.HQ.BL;
using GU.HQ.BL.Policy;
using GU.HQ.DataModel;
using GU.HQ.UI.ViewModel.DeclarerViewModel;
using GU.HQ.UI.ViewModel.Event;


namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimVM : EditableVM<Claim>
    {
        private readonly IClaimStatusPolicy  _claimStatusPolicy = HqFacade.GetClaimStatusPolicy();

        public ClaimVM(Claim entity, 
                        IDomainObjectEventSubscriber<Claim> eventSubscriber, 
                        IDomainDataMapper<Claim> dataMapper, 
                        bool isEditable = false) 
            : base(entity, eventSubscriber, dataMapper, isEditable)
        {}

        protected override void Rebuild()
        {
            ClaimShortVm = new ClaimShortVM(Entity);
            ClaimSummaryVm = new ClaimSummaryVM(Entity);
            DeclarerSummaryVm = new DeclarerSummaryVm(Entity.Declarer);
            DeclarerRelativesVm = new DeclarerRelativesVM(Entity);
            ClaimQueueRegSummaryVm = new ClaimQueueRegSummaryVM(Entity);
            ClaimQueuePrivVm = new ClaimQueuePrivVM(Entity);
            ClaimHouseProvidedSummaryVm = new ClaimHouseProvidedSummaryVM(Entity);
            ClaimQueueDeRegSummaryVm = new ClaimQueueDeRegSummaryVM(Entity);
            ClaimStatusHistVm = new ClaimStatusHistVM(Entity);

            ClaimQueueRegSummaryVm.RebuildRequested += OnRebuildRequestedRebuild;
            ClaimQueuePrivVm.RebuildRequested += OnRebuildRequestedRebuild;
            ClaimHouseProvidedSummaryVm.RebuildRequested += OnRebuildRequestedRebuild;
            ClaimQueueDeRegSummaryVm.RebuildRequested += OnRebuildRequestedRebuild;
        }


        #region Event Metods

        private void OnRebuildRequestedRebuild(object sender, ClaimEventsArgs eventArgs)
        {
            Entity = eventArgs.Claim;
            Rebuild();
            RaiseIsDirtyChanged();
        }
    
        #endregion Event Metods

        #region Overrides of EditableVM<Claim>

        /// <summary>
        /// Сохраняет изменения в редактируемом объекте.
        /// </summary>
        protected override void Save()
        {
            var validator = HqFacade.GetValidator<Claim>();
            var validationResults = validator.Validate(Entity);
            if (!validationResults.IsValid)
            {
                UIFacade.ShowToolView(new ValidationsView(), new ValidationsVM(validationResults.Errors), "Ошибочно заполненные или не заполненные поля.");

                DeclarerSummaryVm.RaiseItemsValidatingPropertyChanged();
                DeclarerRelativesVm.RaiseItemsValidatingPropertyChanged();
                ClaimQueueRegSummaryVm.RaiseValidatingPropertyChanged();
                ClaimQueuePrivVm.RaiseValidatingPropertyChanged();
                ClaimHouseProvidedSummaryVm.RaiseValidatingPropertyChanged();
                ClaimQueueDeRegSummaryVm.RaiseValidatingPropertyChanged();
                
                return;
            }

            base.Save();
        }

        #endregion

        #region BindingProperties

        /// <summary>
        /// Информация о заявлении
        /// </summary>
        private ClaimShortVM _claimShortVm;
        public ClaimShortVM ClaimShortVm
        {
            get {return _claimShortVm; }
            set
            {
                if (_claimShortVm == value) return;
                _claimShortVm = value;
                RaisePropertyChanged(() => ClaimShortVm);
            }
        }

        /// <summary>
        /// общая информаци по заявлению
        /// </summary>
        private ClaimSummaryVM _claimSummaryVm;
        public ClaimSummaryVM ClaimSummaryVm
        {
            get { return _claimSummaryVm; }
            set
            {
                if (_claimSummaryVm == value) return;
                _claimSummaryVm = value;
                RaisePropertyChanged(() => ClaimSummaryVm);
            }
        }


        /// <summary>
        /// Заявитель
        /// </summary>
        private DeclarerSummaryVm _declarerSummaryVm;
        public DeclarerSummaryVm DeclarerSummaryVm  
        {
            get { return _declarerSummaryVm; }
            set
            {
                if (_declarerSummaryVm == value) return;
                _declarerSummaryVm = value;
                RaisePropertyChanged(() => DeclarerSummaryVm);
            }
        }

        
        /// <summary>
        /// Родственники заявителя
        /// </summary>
        private DeclarerRelativesVM _declarerRelativesVm;
        public DeclarerRelativesVM DeclarerRelativesVm
        {
            get { return _declarerRelativesVm; }
            set
            {
                if (_declarerRelativesVm == value) return;
                _declarerRelativesVm = value;
                RaisePropertyChanged(() => DeclarerRelativesVm);
            }
        }
        /// <summary>
        /// общая информаци по постановке на учет
        /// </summary>
        private ClaimQueueRegSummaryVM _claimQueueRegSummaryVm;
        public ClaimQueueRegSummaryVM ClaimQueueRegSummaryVm
        {
            get { return _claimQueueRegSummaryVm; }
            set
            {
                if (_claimQueueRegSummaryVm == value) return;
                _claimQueueRegSummaryVm = value;
                RaisePropertyChanged(() => ClaimQueueRegSummaryVm);
            }
        }

        /// <summary>
        /// информация о внеочередном предоставлении жилья
        /// </summary>
        private ClaimQueuePrivVM _claimQueuePrivVm;
        public ClaimQueuePrivVM ClaimQueuePrivVm
        {
            get { return _claimQueuePrivVm; }
            set
            {
                if (_claimQueuePrivVm == value) return;
                _claimQueuePrivVm = value;
                RaisePropertyChanged(() => ClaimQueuePrivVm);
            }
        }

        /// <summary>
        /// Предоставленное жильё
        /// </summary>
        private ClaimHouseProvidedSummaryVM _claimHouseProvidedSummaryVm;
        public ClaimHouseProvidedSummaryVM ClaimHouseProvidedSummaryVm
        {
            get { return _claimHouseProvidedSummaryVm; }
            set
            {
                if (_claimHouseProvidedSummaryVm == value) return;
                _claimHouseProvidedSummaryVm = value;
                RaisePropertyChanged(() => ClaimHouseProvidedSummaryVm);
            }
        }

        /// <summary>
        /// Информация о снятии с регистрации
        /// </summary>
        private ClaimQueueDeRegSummaryVM _claimQueueDeRegSummaryVm;
        public ClaimQueueDeRegSummaryVM ClaimQueueDeRegSummaryVm
        {
            get { return _claimQueueDeRegSummaryVm; }
            set
            {
                if (_claimQueueDeRegSummaryVm == value) return;
                _claimQueueDeRegSummaryVm = value;
                RaisePropertyChanged(() => ClaimQueueDeRegSummaryVm);
            }
        }

        /// <summary>
        /// Информаци о истории статусов Учетного дела
        /// </summary>
        private ClaimStatusHistVM _claimStatusHistVm;
        public ClaimStatusHistVM ClaimStatusHistVm
        {
            get { return _claimStatusHistVm; }
            set 
            {
                if (_claimStatusHistVm == value) return;
                _claimStatusHistVm = value;
                RaisePropertyChanged(() => ClaimStatusHistVm);
            }
        }
        #endregion
    }
}
