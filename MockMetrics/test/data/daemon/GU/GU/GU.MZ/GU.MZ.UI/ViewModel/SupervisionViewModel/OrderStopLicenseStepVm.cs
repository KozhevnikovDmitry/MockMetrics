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
    /// Класс ViewModel для отображения процесса подготовки и издания приказа 
    /// о досрочном прекращении действия лицензии.
    /// </summary>
    public class OrderStopLicenseStepVm : BaseStandartOrderStepVm
    {
        public OrderStopLicenseStepVm(ISmartValidateableVm<StandartOrder> orderVm, 
                                      ISmartValidateableVm<Notice> noticeVm) 
            : base(orderVm, noticeVm)
        {
            StopLicenseOrderCommand = new DelegateCommand(StopLicenseOrder, () => !IsPublish && IsCurrentOrPrevious);
        }

        /// <summary>
        /// Осуществляет переход тома на следующий этап ведения
        /// </summary>
        protected override void StepNext()
        {
            try
            {
                if (DossierFile.IsDirty)
                {
                    NoticeUser.ShowInformation("Необходимо сохранить изменения в томе перед переходом к следующему этапу.");
                    return;
                }

                StepNext(TaskStatusType.Ready);
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
            StopLicenseOrderCommand.RaiseCanExecuteChanged();
        }
        
        #region Binding Commands

        public DelegateCommand StopLicenseOrderCommand { get; private set; }

        private void StopLicenseOrder()
        {
            try
            {
                var order = Superviser.PrepareStopLicenseOrder(ScenarioStep);

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
