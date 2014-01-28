using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.BL;
using GU.HQ.BL.DomainLogic.QueueManage;
using GU.HQ.UI.View.ClaimView;
using GU.HQ.BL;
using GU.HQ.BL.Policy;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;
using GU.HQ.UI.ViewModel.Event;


namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimQueueDeRegSummaryVM : NotificationObject , IRebuildRequest
    {
        private Claim _claim;
        private IClaimStatusPolicy _statusPolicy = HqFacade.GetClaimStatusPolicy();
        private readonly IQueueManager _queueManager = HqFacade.GetQueueManager();

        public ClaimQueueDeRegSummaryVM(Claim claim) 
        {
            _claim = claim;
            ClaimRejectCommand = new DelegateCommand(ClaimReject);
            ClaimQueueDeRegVm = UIFacade.GetDomainValidateableVM(_claim.QueueDeReg);
        }

        /// <summary>
        /// результаты проверки внесения корректной информации 
        /// </summary>
        public void RaiseValidatingPropertyChanged()
        {
        }

        #region Binding command

        /// <summary>
        /// Предоставить жильё
        /// </summary>
        public DelegateCommand ClaimRejectCommand { get; private set; }
        public void ClaimReject()
        {
            var flagRebuild = false;

            try
            {
                if (_claim.IsDirty)
                {
                    if (NoticeUser.ShowQuestionYesNo("Для исключения учетного дела из очереди, его необходимо сохранить. Сохранить?") == MessageBoxResult.No)
                        return;

                    _claim = HqFacade.GetDataMapper<Claim>().Save(_claim);

                    flagRebuild = true;
                }

                var claim = _claim.Clone();

                claim.QueueDeReg = ClaimQueueDeReg.CreateInstance();

                var claimQueueDeRegVm =  UIFacade.GetDomainValidateableVM(claim.QueueDeReg);

                if (UIFacade.ShowValidateableDialogView(new ClaimQueueDeRegView(), claimQueueDeRegVm, "Исключить заявку из очереди."))
                {
                    _claim = _queueManager.DeRegClaimInQueue(claim, GuFacade.GetDbUser(), HqFacade.GetDataMapper<Claim>());

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
        /// Заявление в работе
        /// </summary>
        public bool IsWork
        {
            get { return _statusPolicy.CanSetStatus(_claim, ClaimStatusType.Rejected); }
        }

        public bool IsReject
        {
            get { return !IsWork; }
        }

        /// <summary>
        /// Полная форма инфрмации по выданному жилью
        /// </summary>
        private IDomainValidateableVM<ClaimQueueDeReg> _claimQueueDeRegVm;
        public IDomainValidateableVM<ClaimQueueDeReg> ClaimQueueDeRegVm
        {
            get { return _claimQueueDeRegVm; }
            set
            {
                if (_claimQueueDeRegVm == value) return;
                _claimQueueDeRegVm = value;
                RaisePropertyChanged(() => ClaimQueueDeRegVm);
            }
        }
     

        #endregion Binding Properties
    }
}
