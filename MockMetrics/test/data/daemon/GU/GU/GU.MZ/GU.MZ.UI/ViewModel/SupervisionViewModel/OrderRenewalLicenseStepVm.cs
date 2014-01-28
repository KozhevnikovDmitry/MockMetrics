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
    /// ViewModel для этапа переоформления(либо отказа в переоформлении) лицензии
    /// </summary> 
    public class OrderRenewalLicenseStepVm : BaseStandartOrderStepVm
    {
        public OrderRenewalLicenseStepVm(ISmartValidateableVm<StandartOrder> orderVm, 
                                         ISmartValidateableVm<Notice> noticeVm) 
            : base(orderVm, noticeVm)
        {
            RenewalLicenseOrderCommand = new DelegateCommand(RenewalLicenseOrder, () => !IsPublish && IsCurrentOrPrevious);
            NotRenewalLicenseOrderCommand = new DelegateCommand(NotRenewalLicenseOrder, () => !IsPublish && IsCurrentOrPrevious);
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
            RenewalLicenseOrderCommand.RaiseCanExecuteChanged();
            NotRenewalLicenseOrderCommand.RaiseCanExecuteChanged();
        }

        #region Binding Commands

        public DelegateCommand RenewalLicenseOrderCommand { get; private set; }

        private void RenewalLicenseOrder()
        {
            try
            {
                var order = Superviser.PrepareRenewalLicenseOrder(ScenarioStep);

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

        /// <summary>
        /// Команда подготовки проекта приказа
        /// </summary>
        public DelegateCommand NotRenewalLicenseOrderCommand { get; private set; }

        /// <summary>
        /// Подготавливает проект приказа о возврате
        /// </summary>
        private void NotRenewalLicenseOrder()
        {
            try
            {
                var order = Superviser.PrepareNotRenewalLicenseOrder(ScenarioStep);

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
