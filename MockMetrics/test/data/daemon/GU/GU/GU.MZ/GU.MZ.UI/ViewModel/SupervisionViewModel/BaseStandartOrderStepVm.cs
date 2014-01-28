using System;
using Common.DA;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    public abstract class BaseStandartOrderStepVm : BaseOrderStepVm
    {
        public BaseStandartOrderStepVm(ISmartValidateableVm<StandartOrder> orderVm, 
                                       ISmartValidateableVm<Notice> noticeVm)
            : base(noticeVm)
        {
            OrderVm = orderVm;
            GoToStandartOrderCommand = new DelegateCommand(GoToStandartOrder, CanGoToStandartOrder);
            RemoveStandartOrderCommand = new DelegateCommand(RemoveStandartOrder, CanRemoveStandartOrder);
        }

        protected override void Rebuild()
        {
            base.Rebuild();
            if (DossierFile.StepStandartOrder(ScenarioStep) != null)
            {
                IsPublish = true;
                _orderVm.Initialize(DossierFile.StepStandartOrder(ScenarioStep));
                RaiseStepCommandsCanExecute();
            }
        }

        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            GoToStandartOrderCommand.RaiseCanExecuteChanged();
            RemoveStandartOrderCommand.RaiseCanExecuteChanged();
        }

        public override void RaiseIsValidChanged()
        {
            base.RaiseIsValidChanged();
            if (OrderVm != null)
            {
                OrderVm.RaiseIsValidChanged();
            }
        }

        #region Binding Properties

        protected ISmartValidateableVm<StandartOrder> _orderVm;

        public ISmartValidateableVm<StandartOrder> OrderVm
        {
            get
            {
                return _orderVm.Entity == null ? null : _orderVm;
            }
            set { _orderVm = value; }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand GoToStandartOrderCommand { get; private set; }

        private void GoToStandartOrder()
        {
            try
            {
                AvalonInteractor.RaiseOpenEditableDockable(
                    OrderVm.Entity.ToString(),
                    typeof(StandartOrder),
                    OrderVm.Entity);
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

        private bool CanGoToStandartOrder()
        {
            try
            {
                return IsPublish && OrderVm.Entity.PersistentState == PersistentState.Old;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
                return false;
            }
        }

        public DelegateCommand RemoveStandartOrderCommand { get; private set; }

        private void RemoveStandartOrder()
        {
            try
            {
                AvalonInteractor.RaiseCloseEditableDockable(typeof(StandartOrder), OrderVm.Entity.Id);
                IsPublish = false;
                DossierFile.RemoveStepStandartOrder(ScenarioStep);
                RaiseStepCommandsCanExecute();
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

        private bool CanRemoveStandartOrder()
        {
            try
            {
                return IsPublish;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
                return false;
            }
        }

        #endregion
    }
}