using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.BL;
using GU.HQ.BL;
using GU.HQ.BL.DomainLogic.QueueManage;
using GU.HQ.BL.Policy;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;
using GU.HQ.UI.View.ClaimView;
using GU.HQ.UI.ViewModel.AddressViewModel;
using GU.HQ.UI.ViewModel.Event;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimHouseProvidedSummaryVM : NotificationObject, IRebuildRequest
    {
        private Claim _claim;
        private readonly IClaimStatusPolicy _claimStatusPolicy = HqFacade.GetClaimStatusPolicy();
        private readonly IQueueManager _queueManager = HqFacade.GetQueueManager();

        public ClaimHouseProvidedSummaryVM(Claim claim)
        {
            _claim = claim;

            ClaimHouseProvidedCommand = new DelegateCommand(ClaimHouseProvided);
            ClaimHouseProvidedAddressDescCommand = new DelegateCommand(ClaimHouseProvidedAddressDesc);

            ClaimHouseProvidedVm = UIFacade.GetDomainValidateableVM(_claim.HouseProvided);

            if (IsHasHouseDesc)
                AddressFullVm = new AddressFullVM(_claim.HouseProvided.Address);
        }

        /// <summary>
        /// результаты проверки внесения корректной информации 
        /// </summary>
        public void RaiseValidatingPropertyChanged()
        {
            ClaimHouseProvidedVm.RaiseValidatingPropertyChanged();
            if(AddressFullVm != null)
                AddressFullVm.RaiseValidatingPropertyChanged();
        }

        #region Command

        /// <summary>
        /// Команда предоставить жильё
        /// </summary>
        public DelegateCommand ClaimHouseProvidedCommand { get; private set; }

        private void ClaimHouseProvided()
        {
            var flagRebuild = false;
            try
            {
                if (_claim.IsDirty)
                {
                    if (NoticeUser.ShowQuestionYesNo("Для предоставления жилья, учетное дело необходимо сохранить. Сохранить?") == MessageBoxResult.No) return;
                    _claim = HqFacade.GetDataMapper<Claim>().Save(_claim);
                    flagRebuild = true;
                }

                var claim = _claim.Clone();

                claim.HouseProvided = HouseProvided.CreateInstance();

                var claimHouseProvidedVm =  UIFacade.GetDomainValidateableVM(claim.HouseProvided);

                if (UIFacade.ShowValidateableDialogView(new ClaimHouseProvidedView(), claimHouseProvidedVm, "Предоставить жильё."))
                {
                    _claim = _queueManager.ClaimHouseProvided(claim, GuFacade.GetDbUser(), HqFacade.GetDataMapper<Claim>());
                    flagRebuild = true;
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
            finally
            {
                if (flagRebuild) RaiseRebuildRequest();
            }
        }

        /// <summary>
        /// Команда внести информацию о жилье
        /// </summary>
        public DelegateCommand ClaimHouseProvidedAddressDescCommand { get; private set; }

        private void ClaimHouseProvidedAddressDesc()
        {
            _claim.HouseProvided.Address = Address.CreateInstance();
            _claim.HouseProvided.Address.AddressDesc = AddressDesc.CreateInstance();
            AddressFullVm = new AddressFullVM(_claim.HouseProvided.Address);

            RaisePropertyChanged(() => IsHasntHouseDesc);
            RaisePropertyChanged(() => IsHasHouseDesc);
        }

        #endregion Command

        #region Event Rebuild

        public event ViewModelRebuildRequested RebuildRequested;

        protected virtual void RaiseRebuildRequest()
        {
            if (RebuildRequested != null)
                RebuildRequested(this, new ClaimEventsArgs(_claim));
        }

        #endregion Event Rebuild


        #region Binding Properties

        /// <summary>
        /// Заявка не зарегистрирована
        /// </summary>
        public bool IsClaimNoReg { get { return _claimStatusPolicy.CanSetStatus(_claim, ClaimStatusType.QueueReg); } }

        /// <summary>
        /// Информация о решении предоставления жилья отсутствуте
        /// </summary>
        public bool IsNoHouse { get { return _claimStatusPolicy.CanSetStatus(_claim, ClaimStatusType.HouseProvided); } }

        /// <summary>
        /// Информация о решении предоставления жилья внесена
        /// </summary>
        public bool IsHasHouse { get { return _claim.HouseProvided != null;  } }

        /// <summary>
        /// информация о предоставленом жилье не внесена
        /// </summary>
        public bool IsHasntHouseDesc{ get { return _claim.HouseProvided != null && _claim.HouseProvided.Address == null; } }

        /// <summary>
        /// Информация о предоставленном жилье внесена
        /// </summary>
        public bool IsHasHouseDesc{get { return _claim.HouseProvided != null && _claim.HouseProvided.Address != null; }}

        /// <summary>
        /// Решение о предоставлении жилья
        /// </summary>
        private IDomainValidateableVM<HouseProvided> _claimHouseProvidedVm;
        public IDomainValidateableVM<HouseProvided> ClaimHouseProvidedVm
        {
            get { return _claimHouseProvidedVm; }
            set
            {
                if (_claimHouseProvidedVm == value) return;
                _claimHouseProvidedVm = value;
                RaisePropertyChanged(() => ClaimHouseProvidedVm);
            }
        }

        /// <summary>
        /// Информация о предоставленном жилье
        /// </summary>
        private AddressFullVM _addressFullVm;
        public AddressFullVM AddressFullVm 
        { 
            get { return _addressFullVm; }
            set 
            { 
                if (_addressFullVm == value) return;
                _addressFullVm = value;
                RaisePropertyChanged(() => AddressFullVm);
            }
        }

        #endregion Binding Properties
    }
}