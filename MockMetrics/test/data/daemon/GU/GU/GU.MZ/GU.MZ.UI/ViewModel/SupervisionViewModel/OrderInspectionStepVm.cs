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
    /// о проведении выездной проверки
    /// </summary>
    public class OrderInspectionStepVm : BaseOrderStepVm
    {
        private readonly IDomainDataMapper<InspectionOrder> _orderMapper;

        public OrderInspectionStepVm(ISmartValidateableVm<Notice> noticeVm,
                                     ISmartValidateableVm<InspectionOrder> inspectionOrderVm,
                                     IDomainDataMapper<InspectionOrder> orderMapper) 
            : base(noticeVm)
        {
            _orderMapper = orderMapper;
            InspectionOrderVm = inspectionOrderVm;
            GoToInspectionOrderCommand = new DelegateCommand(GoToInspectionOrder, CanGoToInspectionOrder);
            PrepareInspectionOrderCommand = new DelegateCommand(PrepareInspectionOrder, () => !IsPublish && IsCurrentOrPrevious);
            RemoveInspectionOrderCommand = new DelegateCommand(RemoveInspectionOrder, CanRemovevInspectionOrder);
        }

        /// <summary>
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            PrepareInspectionOrderCommand.RaiseCanExecuteChanged();
            GoToInspectionOrderCommand.RaiseCanExecuteChanged();
            RemoveInspectionOrderCommand.RaiseCanExecuteChanged();
        }

        public override void RaiseIsValidChanged()
        {
            base.RaiseIsValidChanged();

            if (InspectionOrderVm != null)
            {
                InspectionOrderVm.RaiseIsValidChanged();
            }
        }

        protected override void Rebuild()
        {
            base.Rebuild();
            if (DossierFile.StepInspectionOrder(ScenarioStep) != null)
            {
                IsPublish = true;
                _inspectionOrderVm.Initialize(DossierFile.StepInspectionOrder(ScenarioStep));
                RaiseStepCommandsCanExecute();
            }
        }

        #region Binding Properties

        private ISmartValidateableVm<InspectionOrder> _inspectionOrderVm;

        public ISmartValidateableVm<InspectionOrder> InspectionOrderVm
        {
            get { return _inspectionOrderVm.Entity == null ? null : _inspectionOrderVm; }
            private set { _inspectionOrderVm = value; }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда подготовки проекта приказа о проведении документарной проверки
        /// </summary>
        public DelegateCommand PrepareInspectionOrderCommand { get; private set; }

        /// <summary>
        /// Подготавливает проект приказа о выездной проверкеS
        /// </summary>
        private void PrepareInspectionOrder()
        {
            try
            {
                var order = Superviser.PrepareInspectionOrder(ScenarioStep);

                _inspectionOrderVm.Initialize(order);

                IsPublish = true;
                RaiseStepCommandsCanExecute();
                RaisePropertyChanged(() => InspectionOrderVm);
                InspectionOrderVm.RaiseAllPropertyChanged();
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

        public DelegateCommand GoToInspectionOrderCommand { get; private set; }

        private void GoToInspectionOrder()
        {
            try
            {
                AvalonInteractor.RaiseOpenEditableDockable(
                    InspectionOrderVm.Entity.ToString(),
                    typeof(InspectionOrder),
                    _orderMapper.Retrieve(InspectionOrderVm.Entity.Id));
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

        private bool CanGoToInspectionOrder()
        {
            try
            {
                return IsPublish && InspectionOrderVm.Entity.PersistentState == PersistentState.Old;
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
                var notice = Superviser.AddNotice(NoticeType.PlaceInspection, ScenarioStep);
                IsNotice = true;
                _noticeVm.Initialize(notice);
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
                return DossierFile.StepInspectionOrder(ScenarioStep) != null && DossierFile.StepNotice(ScenarioStep) == null;
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

        public DelegateCommand RemoveInspectionOrderCommand { get; private set; }

        private void RemoveInspectionOrder()
        {
            try
            {
                AvalonInteractor.RaiseCloseEditableDockable(typeof(InspectionOrder), InspectionOrderVm.Entity.Id);
                IsPublish = false;
                DossierFile.RemoveStepInspectionOrder(ScenarioStep);
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

        private bool CanRemovevInspectionOrder()
        {
            try
            {
                return IsPublish && InspectionOrderVm != null;
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
