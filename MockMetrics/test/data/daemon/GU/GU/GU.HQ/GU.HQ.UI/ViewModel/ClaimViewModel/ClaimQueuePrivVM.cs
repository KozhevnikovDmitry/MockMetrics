using System;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Common.Types.Exceptions;
using Common.UI;
using GU.BL;
using GU.HQ.BL;
using GU.HQ.BL.DomainLogic.QueueManage;
using GU.HQ.UI.View.ClaimView;
using GU.HQ.BL.Policy;
using GU.HQ.DataModel.Types;
using GU.HQ.UI.ViewModel.Event;
using GU.HQ.DataModel;
using Microsoft.Practices.Prism.ViewModel;


namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimQueuePrivVM : NotificationObject, IRebuildRequest
    {
        private Claim _claim;
        private readonly IClaimStatusPolicy _claimStatusPolicy = HqFacade.GetClaimStatusPolicy();
        private readonly IQueueManager _queueManager = HqFacade.GetQueueManager();


        public ClaimQueuePrivVM(Claim claim)
        {
            _claim = claim;
            _claimQueuePrivListVm = new ClaimQueuePrivListVM(_claim);

            ClaimPrivRegCommand = new DelegateCommand(ClaimPrivReg);
            ClaimPrivDeRegCommand = new DelegateCommand(ClaimPrivDeReg);
        }


        /// <summary>
        /// результаты проверки внесения корректной информации 
        /// </summary>
        public void RaiseValidatingPropertyChanged()
        {
        }
        
        
        #region Binding Command

        /// <summary>
        /// Регистрируем заявку в очереде внеочередников
        /// </summary>
        public DelegateCommand ClaimPrivRegCommand { get; private set; }
        private void ClaimPrivReg()
        {
            var flagRebuild = false;

            try
            {
                if (_claim.IsDirty)
                {
                    if (NoticeUser.ShowQuestionYesNo("Для постановки учетного дела в очередь внеочередников, его необходимо сохранить. Сохранить?")==MessageBoxResult.No)
                        return;

                    _claim = HqFacade.GetDataMapper<Claim>().Save(_claim);
                    flagRebuild = true;
                }

                var claim = _claim.Clone();

                var queuePrivReg = QueuePrivReg.CreateInstance();
                var queuePrivRegVm = UIFacade.GetDomainValidateableVM(queuePrivReg);

                if (UIFacade.ShowValidateableDialogView(new ClaimQueuePrivRegView(), queuePrivRegVm, "Поставить заявку в очередь внеочередников."))
                {
                    var queuePriv = QueuePriv.CreateInstance();
                    queuePriv.QueuePrivReg = queuePrivReg;

                    claim.QueuePrivList.Add(queuePriv);

                    _claim = _queueManager.RegistrClaimInQueuePriv(claim, GuFacade.GetDbUser(), HqFacade.GetDataMapper<Claim>());
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

        public DelegateCommand ClaimPrivDeRegCommand { get; private set; }

        /// <summary>
        /// исключаем заявку из очереди внеочередников
        /// </summary>
        private void ClaimPrivDeReg()
        {
            var flagRebuild = false;
            try
            {
                if (_claim.IsDirty)
                {
                    if (NoticeUser.ShowQuestionYesNo("Для исключения учетного дела из очереди внеочередников, его необходимо сохранить. Сохранить?")== MessageBoxResult.No)
                        return;

                    _claim = HqFacade.GetDataMapper<Claim>().Save(_claim);

                    flagRebuild = true;
                }

                var claim = _claim.Clone();

                var queuePrivDeReg = QueuePrivDeReg.CreateInstance();

                var queuePrivDeRegVm = UIFacade.GetDomainValidateableVM(queuePrivDeReg);

                if (UIFacade.ShowValidateableDialogView(new ClaimQueuePrivDeRegView(), queuePrivDeRegVm, "Исключить заявку из очереди внеочередников."))
                {
                    foreach (var qpEl in claim.QueuePrivList.Where(qpEl => qpEl.QueuePrivDeReg == null))
                        qpEl.QueuePrivDeReg = queuePrivDeRegVm.Entity;

                    _claim = _queueManager.DeRegClaimInQueuePriv(claim, GuFacade.GetDbUser(), HqFacade.GetDataMapper<Claim>());

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
        #endregion Binding Command

        #region Binding Properties

        /// <summary>
        /// Заявка не зарегистрирована
        /// </summary>
        public bool IsClaimNoReg { get { return _claimStatusPolicy.CanSetStatus(_claim, ClaimStatusType.QueueReg); } }
        
        /// <summary>
        /// Учетное дело не состоит в очереди внеочередников
        /// </summary>
        public bool IsNoPrivReg { get { return _claimStatusPolicy.CanSetStatus(_claim, ClaimStatusType.QueuePrivReg); } }

        /// <summary>
        /// Учетное дело состоит в очереди внеочередников
        /// </summary>
        public bool IsPrivReg { get { return _claimStatusPolicy.CanSetStatus(_claim, ClaimStatusType.QueuePrivDeReg); } }


        private ClaimQueuePrivListVM _claimQueuePrivListVm;
        public ClaimQueuePrivListVM ClaimQueuePrivListVm
        {
            get { return _claimQueuePrivListVm; }
            set
            {
                if (_claimQueuePrivListVm == value) return;
                _claimQueuePrivListVm = value;
                RaisePropertyChanged(() => ClaimQueuePrivListVm);
            }
        }

        #endregion Binding Properties

        #region Events

        public event ViewModelRebuildRequested RebuildRequested;

        protected virtual void RaiseRebuildRequest()
        {
            if (RebuildRequested != null)
                RebuildRequested(this, new ClaimEventsArgs(_claim));
        }

        #endregion Events
    }
}