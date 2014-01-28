using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.BL;
using GU.HQ.BL;
using GU.HQ.BL.DomainLogic.QueueManage;
using GU.HQ.BL.Policy;
using GU.HQ.UI.View.ClaimView;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;
using GU.HQ.UI.ViewModel.ClaimViewModelViewModel;
using GU.HQ.UI.ViewModel.DeclarerViewModel;
using GU.HQ.UI.ViewModel.Event;


namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimQueueRegSummaryVM : NotificationObject, IRebuildRequest
    {
        private Claim _claim;
        private readonly IQueueManager _queueManager = HqFacade.GetQueueManager();
        private readonly IClaimStatusPolicy _claimStatusPolicy = HqFacade.GetClaimStatusPolicy();

        public ClaimQueueRegSummaryVM(Claim claim)
        {
            _claim = claim;
            CreateVMs();
            ClaimRegCommand = new DelegateCommand(this.RegClaim);
        }

        public void RaiseValidatingPropertyChanged()
        {
            DeclarerBaseRegSummaryVm.RaiseValidatingPropertyChanged();
            ClaimQueueRegVm.RaiseValidatingPropertyChanged();
            CategoryVm.RaiseItemsValidatingPropertyChanged();
            NoticeVm.RaiseItemsValidatingPropertyChanged();
        }

        private void CreateVMs()
        {
            DeclarerBaseRegSummaryVm = new DeclarerBaseRegSummaryVM(_claim);

            if (!IsReg) return;
        
            ClaimQueueRegVm = UIFacade.GetDomainValidateableVM(_claim.QueueReg);
            NoticeVm = new NoticeVM(_claim);
            CategoryVm = new ClaimCategoryVM(_claim);
        }

        /// <summary>
        /// метод необходим для обработки события в головном VM
        /// </summary>
        public void RaiseClaimStatusChanged()
        {
            RaisePropertyChanged(() => IsNoReg);
        }

        #region Command 
        /// <summary>
        /// Команда принятия заявления к рассмотрению.
        /// </summary>
        public DelegateCommand ClaimRegCommand { get; private set; }

        /// <summary>
        /// Поставить заявление в очередь
        /// </summary>
        private void RegClaim()
        {
            var flagRebuild = false;
            try
            {
                if (_claim.IsDirty)
                {
                    if (NoticeUser.ShowQuestionYesNo("Для постановки заявки в очередь, заявку необходимо сохранить. Сохранить?") == MessageBoxResult.No)
                        return;

                    _claim = HqFacade.GetDataMapper<Claim>().Save(_claim);

                    flagRebuild = true;
                }

                var claim = _claim.Clone();

                claim.QueueReg = ClaimQueueReg.CreateInstance();

                var claimQueueRegVm =  UIFacade.GetDomainValidateableVM(claim.QueueReg);

                if (UIFacade.ShowValidateableDialogView(new ClaimQueueRegView(), claimQueueRegVm, "Поставить заявку в очередь."))
                {
                    _claim = _queueManager.RegistrClaimInQueue(claim, GuFacade.GetDbUser(), HqFacade.GetDataMapper<Claim>());

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

        #region Binding Properties

        #region Common

        /// <summary>
        /// Заявление не поставленно в очередь
        /// </summary>
        public bool IsNoReg { get { return _claimStatusPolicy.CanSetStatus(_claim, ClaimStatusType.QueueReg); } }

        /// <summary>
        /// Заявление поставленно в очередь
        /// </summary>
        public bool IsReg { get { return _claim.QueueReg != null; } }

        #endregion Common

        /// <summary>
        /// Информация об уведомлениях
        /// </summary>
        private NoticeVM _noticeVm;
        public NoticeVM NoticeVm
        {
            get { return _noticeVm; }
            set
            {
                if (_noticeVm == value) return;
                _noticeVm = value;
                RaisePropertyChanged(() => NoticeVm);
            }
        }

        /// <summary>
        /// Информация о регистрации в очереди
        /// </summary>
        private IDomainValidateableVM<ClaimQueueReg> _claimQueueRegVm;
        public IDomainValidateableVM<ClaimQueueReg> ClaimQueueRegVm
        {
            get { return _claimQueueRegVm; }
            set
            {
                if (_claimQueueRegVm == value) return;
                _claimQueueRegVm = value;
                RaisePropertyChanged(() => ClaimQueueRegVm);
            }
        }

        /// <summary>
        /// информация о категориях учета
        /// </summary>
        private ClaimCategoryVM _categoryVm;
        public ClaimCategoryVM CategoryVm
        {
            get{ return _categoryVm; }
            set
            {
                if (_categoryVm == value) return;
                _categoryVm = value;
                RaisePropertyChanged(() => CategoryVm);
            }
        }

        /// <summary>
        /// Информация об основаниях учета который указал заявитель
        /// </summary>
        private DeclarerBaseRegSummaryVM _declarerBaseRegSummaryVm;
        public DeclarerBaseRegSummaryVM DeclarerBaseRegSummaryVm
        {
            get { return _declarerBaseRegSummaryVm; }
            set
            {
                if (_declarerBaseRegSummaryVm == value) return;
                _declarerBaseRegSummaryVm = value;
                RaisePropertyChanged(() => DeclarerBaseRegSummaryVm);
            }
        }

        #endregion Binding Properties

        #region Event Rebuild

        public event ViewModelRebuildRequested RebuildRequested;

        protected virtual void RaiseRebuildRequest()
        {
            if (RebuildRequested != null)
                RebuildRequested(this, new ClaimEventsArgs(_claim));
        }
        #endregion Event Rebuild
    }
}
