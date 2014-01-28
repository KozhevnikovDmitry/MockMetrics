using System;
using Common.Types.Exceptions;
using Common.UI;
using GU.DataModel;
using GU.MZ.UI.ViewModel.DossierFileViewModel;
using GU.MZ.UI.ViewModel.OrderViewModel.Standart;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения процесса подготовки и издания приказа 
    /// о возврате или приёме документов.
    /// </summary> 
    public class OrderAcceptOrRejectStepVm : BaseStandartOrderStepVm
    {
        public OrderAcceptOrRejectStepVm(StandartOrderDataVm orderVm,
                                         NoticeVm noticeVm) 
            : base(orderVm, noticeVm)
        {
            PrepareAcceptOrderCommand = new DelegateCommand(PrepareAcceptOrder, () => !IsPublish && IsCurrentOrPrevious);
            PrepareRejectOrderCommand = new DelegateCommand(PrepareRejectOrder, () => !IsPublish && IsCurrentOrPrevious);
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
                    StepNext(TaskStatusType.Working);
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
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            PrepareAcceptOrderCommand.RaiseCanExecuteChanged();
            PrepareRejectOrderCommand.RaiseCanExecuteChanged();
        }

        #region Binding Commands

        public DelegateCommand PrepareAcceptOrderCommand { get; private set; }

        private void PrepareAcceptOrder()
        {
            try
            {
                var order = Superviser.PrepareAcceptTaskOrder(ScenarioStep);
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

        public DelegateCommand PrepareRejectOrderCommand { get; private set; }

        private void PrepareRejectOrder()
        {
            try
            {
                var order = Superviser.PrepareReturnTaskOrder(ScenarioStep);
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
