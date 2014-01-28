using System;
using Common.Types.Exceptions;
using Common.UI;
using GU.DataModel;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// ViewModel для этапа предоставления(либо отказа в предоставлении) лицензии
    /// </summary> 
    public class OrderGrantLicenseStepVm : BaseStandartOrderStepVm
    {
        public OrderGrantLicenseStepVm(ISmartValidateableVm<StandartOrder> orderVm, 
                                       ISmartValidateableVm<Notice> noticeVm) 
            : base(orderVm, noticeVm)
        {
            GrantLicenseOrderCommand = new DelegateCommand(GrantLicenseOrder, () => !IsPublish && IsCurrentOrPrevious);
            NotGrantLicenseOrderCommand = new DelegateCommand(NotGrantLicenseOrder, () => !IsPublish && IsCurrentOrPrevious);
        }

        /// <summary>
        /// Осуществляет переход тома на следующий этап ведения
        /// </summary>
        protected override void StepNext()
        {
            try
            {
                if (Superviser.IsRejectedOrderPrepared(ScenarioStep) && DossierFile.IsRejected)
                {
                    RejectAndStepNext();
                }
                else
                {
                    StepNext(TaskStatusType.Ready);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }

        /// <summary>
        /// Возвращает true, если можно перейти к следующему этапу ведения тома
        /// </summary>
        /// <returns>Флаг возможности перехода к следующему этапу</returns>
        protected override bool CanStepNext()
        {
            return base.CanStepNext() && IsPublish;
        }

        /// <summary>
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            GrantLicenseOrderCommand.RaiseCanExecuteChanged();
            NotGrantLicenseOrderCommand.RaiseCanExecuteChanged();
        }

        #region Binding Commands

        public DelegateCommand GrantLicenseOrderCommand { get; private set; }

        private void GrantLicenseOrder()
        {
            try
            {
                var order = Superviser.PrepareGrantLicenseOrder(ScenarioStep);

                _orderVm.Initialize(order);

                IsPublish = true;
                RaiseStepCommandsCanExecute();
                RaisePropertyChanged(() => OrderVm);
                OrderVm.RaiseAllPropertyChanged();
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public DelegateCommand NotGrantLicenseOrderCommand { get; private set; }

        private void NotGrantLicenseOrder()
        {
            try
            {
                var order = Superviser.PrepareNotGrantLicenseOrder(ScenarioStep);

                _orderVm.Initialize(order);

                IsPublish = true;
                RaiseStepCommandsCanExecute();
                RaisePropertyChanged(() => OrderVm);
                OrderVm.RaiseAllPropertyChanged();
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #endregion
    }
}