using System;
using Common.BL.DataMapping;
using Common.DA;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения процесса подготовки и издания приказа 
    /// о проведении документарной проверки
    /// </summary>
    public class OrderExpertiseStepVm : BaseOrderStepVm
    {
        private readonly IDomainDataMapper<ExpertiseOrder> _orderMapper;

        public OrderExpertiseStepVm(ISmartValidateableVm<Notice> noticeVm,
                                    ISmartValidateableVm<ExpertiseOrder> expertiseOrderVm, 
                                    IDomainDataMapper<ExpertiseOrder> orderMapper)
            : base(noticeVm)
        {
            _orderMapper = orderMapper;
            ExpertiseOrderVm = expertiseOrderVm;

            GoToExpertiseOrderCommand = new DelegateCommand(GoToExpertiseOrder, CanGoToExpertiseOrder);
            PrepareExpertiseOrderCommand = new DelegateCommand(PrepareExpertiseOrder, () => !IsPublish && IsCurrentOrPrevious);
            RemoveExpertiseOrderCommand = new DelegateCommand(RemoveExpertiseOrder, CanRemoveExpertiseOrder);
        }

        /// <summary>
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            PrepareExpertiseOrderCommand.RaiseCanExecuteChanged();
            RemoveExpertiseOrderCommand.RaiseCanExecuteChanged();
            GoToExpertiseOrderCommand.RaiseCanExecuteChanged();
        }

        public override void RaiseIsValidChanged()
        {
            base.RaiseIsValidChanged();

            if (ExpertiseOrderVm != null)
            {
                ExpertiseOrderVm.RaiseIsValidChanged();
            }
        }

        protected override void Rebuild()
        {
            base.Rebuild();
            if (DossierFile.StepExpertiseOrder(ScenarioStep) != null)
            {
                IsPublish = true;
                _expertiseOrderVm.Initialize(DossierFile.StepExpertiseOrder(ScenarioStep));
                RaiseStepCommandsCanExecute();
            }
        }

        #region Binding Properties

        private ISmartValidateableVm<ExpertiseOrder> _expertiseOrderVm;

        public ISmartValidateableVm<ExpertiseOrder> ExpertiseOrderVm
        {
            get { return _expertiseOrderVm.Entity == null ? null : _expertiseOrderVm; }
            private set { _expertiseOrderVm = value; }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда подготовки проекта приказа о проведении документарной проверки
        /// </summary>
        public DelegateCommand PrepareExpertiseOrderCommand { get; private set; }

        /// <summary>
        /// Подготавливает проект приказа о документарной проверке
        /// </summary>
        private void PrepareExpertiseOrder()
        {
            try
            {
                var order = Superviser.PrepareExpertiseOrder(ScenarioStep);

                _expertiseOrderVm.Initialize(order);

                IsPublish = true;
                RaiseStepCommandsCanExecute();
                RaisePropertyChanged(() => ExpertiseOrderVm);
                ExpertiseOrderVm.RaiseAllPropertyChanged();
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

        public DelegateCommand GoToExpertiseOrderCommand { get; private set; }

        private void GoToExpertiseOrder()
        {
            try
            {
                AvalonInteractor.RaiseOpenEditableDockable(
                    ExpertiseOrderVm.Entity.ToString(),
                    typeof(ExpertiseOrder),
                    _orderMapper.Retrieve(ExpertiseOrderVm.Entity.Id));
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

        private bool CanGoToExpertiseOrder()
        {
            try
            {
                return IsPublish && ExpertiseOrderVm != null && ExpertiseOrderVm.Entity.PersistentState == PersistentState.Old;
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

        protected override void PrepareOrderNotice()
        {
            try
            {
                var notice = Superviser.AddNotice(NoticeType.DocumentExpertise, ScenarioStep);
                _noticeVm.Initialize(notice);
                IsNotice = true;
                RaiseStepCommandsCanExecute();
                RaisePropertyChanged(() => NoticeVm);
                NoticeVm.RaiseAllPropertyChanged();
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

        protected override bool CanPrepareOrderNotice()
        {
            try
            {
                return DossierFile.StepExpertiseOrder(ScenarioStep) != null && DossierFile.StepNotice(ScenarioStep) == null;
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

        public DelegateCommand RemoveExpertiseOrderCommand { get; private set; }

        private void RemoveExpertiseOrder()
        {
            try
            {
                AvalonInteractor.RaiseCloseEditableDockable(typeof(ExpertiseOrder), ExpertiseOrderVm.Entity.Id);
                IsPublish = false;
                DossierFile.RemoveStepExpertiseOrder(ScenarioStep);
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

        private bool CanRemoveExpertiseOrder()
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
