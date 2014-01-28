using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Violation;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения этапа Направление ведомления о необходимости
    /// устранения нарушений
    /// </summary>
    public class NoticeViolationStepVm : AbstractSupervisionStepVm
    {
        /// <summary>
        /// Класс ViewModel для отображения этапа Направление ведомления о необходимости
        /// устранения нарушений
        /// </summary>
        public NoticeViolationStepVm(ISmartValidateableVm<ViolationNotice> violationNoticeVm,
                                     ISmartValidateableVm<ViolationResolveInfo> violationResolveInfoVM,
                                     ISmartValidateableVm<Notice> noticeVm)
        {
            IsViolated = false;
            IsNotice = false;
            IsInfoCreated = false;
            ViolationNoticeVm = violationNoticeVm;
            AvalonInteractor.RegisterCaller(_violationNoticeVm as IAvalonDockCaller);
            ViolationResolveInfoVm = violationResolveInfoVM;
            NoticeVm = noticeVm;
            CreateViolationNoticeCommand = new DelegateCommand(CreateViolationNotice, CanCreateViolationNotice);
            PrepareNoticeCommand = new DelegateCommand(PrepareNotice, CanPrepareNotice);
            RemoveNoticeCommand = new DelegateCommand(RemoveNotice, CanRemoveNotice);
            RemoveViolationNoticeCommand = new DelegateCommand(RemoveViolationNotice, CanRemoveViolationNotice);
            RemoveViolationResolveInfoCommand = new DelegateCommand(RemoveViolationResolveInfo, CanRemoveViolationResolveInfo);
        }

        /// <summary>
        /// Конструирует дочерные vm'ы
        /// </summary>
        protected override void Rebuild()
        {
            if (DossierFile.StepViolationNotice(ScenarioStep) != null)
            {
                IsViolated = true;
                _violationNoticeVm.Initialize(DossierFile.StepViolationNotice(ScenarioStep));
                RaisePropertyChanged(() => ViolationNoticeVm);
                ViolationNoticeVm.RaiseAllPropertyChanged();
            }

            if (DossierFile.StepViolationResolveInfo(ScenarioStep) != null)
            {
                IsInfoCreated = true;
                _violationResolveInfoVm.Initialize(DossierFile.StepViolationResolveInfo(ScenarioStep));
                RaisePropertyChanged(() => ViolationNoticeVm);
                ViolationNoticeVm.RaiseAllPropertyChanged();
            }

            if (DossierFile.StepNotice(ScenarioStep) != null)
            {
                IsNotice = true;
                _noticeVm.Initialize(DossierFile.StepNotice(ScenarioStep));
                RaisePropertyChanged(() => ViolationNoticeVm);
                ViolationNoticeVm.RaiseAllPropertyChanged();
            }
        }

        /// <summary>
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            CreateViolationNoticeCommand.RaiseCanExecuteChanged();
            PrepareNoticeCommand.RaiseCanExecuteChanged();
            RemoveNoticeCommand.RaiseCanExecuteChanged();
            RemoveViolationNoticeCommand.RaiseCanExecuteChanged();
            RemoveViolationResolveInfoCommand.RaiseCanExecuteChanged();
        }

        public override void RaiseIsValidChanged()
        {
            if (NoticeVm != null)
            {
                NoticeVm.RaiseIsValidChanged();
            }

            if (ViolationResolveInfoVm != null)
            {
                ViolationResolveInfoVm.RaiseIsValidChanged();
            }

            if (ViolationNoticeVm != null)
            {
                ViolationNoticeVm.RaiseIsValidChanged();
            }
        }

        #region Binding Properties

        private ISmartValidateableVm<ViolationNotice> _violationNoticeVm;

        public ISmartValidateableVm<ViolationNotice> ViolationNoticeVm
        {
            get { return _violationNoticeVm.Entity != null ? _violationNoticeVm : null; }
            private set { _violationNoticeVm = value; }
        }

        private ISmartValidateableVm<Notice> _noticeVm;

        public ISmartValidateableVm<Notice> NoticeVm
        {
            get { return _noticeVm.Entity != null ? _noticeVm : null; }
            private set { _noticeVm = value; }
        }

        private ISmartValidateableVm<ViolationResolveInfo> _violationResolveInfoVm;

        public ISmartValidateableVm<ViolationResolveInfo> ViolationResolveInfoVm
        {
            get { return _violationResolveInfoVm.Entity != null ? _violationResolveInfoVm : null; }
            private set { _violationResolveInfoVm = value; }
        }

        /// <summary>
        /// Флаг, указыващий на наличие нарушение
        /// </summary>
        private bool _isViolated;

        /// <summary>
        /// Возвращает или устанаваливает флаг, указыващий на наличие нарушений
        /// </summary>
        public bool IsViolated
        {
            get
            {
                return _isViolated;
            }
            set
            {
                if (_isViolated != value)
                {
                    _isViolated = value;
                    RaisePropertyChanged(() => IsViolated);
                }
            }
        }

        /// <summary>
        /// Флаг, указыващий на отправку уведомления
        /// </summary>
        private bool _isNotice;

        /// <summary>
        /// Возвращает или устанаваливает флаг, указыващий на отправку уведомления
        /// </summary>
        public bool IsNotice
        {
            get
            {
                return _isNotice;
            }
            set
            {
                if (_isNotice != value)
                {
                    _isNotice = value;
                    RaisePropertyChanged(() => IsNotice);
                }
            }
        }

        private bool _isInfoCreated;

        public bool IsInfoCreated
        {
            get
            {
                return _isInfoCreated;
            }
            set
            {
                if (_isInfoCreated != value)
                {
                    _isInfoCreated = value;
                    RaisePropertyChanged(() => IsInfoCreated);
                }
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand CreateViolationNoticeCommand { get; private set; }

        private void CreateViolationNotice()
        {
            try
            {
                _violationResolveInfoVm.Initialize(Superviser.PrepareViolationResolveInfo(ScenarioStep));
                _violationNoticeVm.Initialize(Superviser.PrepareViolationNotice(ScenarioStep));
                RaisePropertyChanged(() => ViolationResolveInfoVm);
                ViolationResolveInfoVm.RaiseAllPropertyChanged();
                RaisePropertyChanged(() => ViolationNoticeVm);
                ViolationNoticeVm.RaiseAllPropertyChanged();
                IsViolated = true;
                IsInfoCreated = true;
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

        private bool CanCreateViolationNotice()
        {
            try
            {
                return DossierFile.StepViolationNotice(ScenarioStep) == null && IsCurrentOrPrevious;
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

        public DelegateCommand PrepareNoticeCommand { get; private set; }

        private void PrepareNotice()
        {
            try
            {
                var notice = Superviser.AddNotice(NoticeType.ViolationResolve, ScenarioStep);
                _noticeVm.Initialize(notice); 
                RaisePropertyChanged(() => NoticeVm);
                NoticeVm.RaiseAllPropertyChanged();
                IsNotice = true;
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

        private bool CanPrepareNotice()
        {
            try
            {
                return DossierFile.StepViolationResolveInfo(ScenarioStep) != null && DossierFile.StepNotice(ScenarioStep) == null && IsCurrentOrPrevious;
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

        public DelegateCommand RemoveViolationNoticeCommand { get; private set; }

        private void RemoveViolationNotice()
        {
            try
            {
                IsViolated = false;
                DossierFile.RemoveStepViolationNotice(ScenarioStep);
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

        private bool CanRemoveViolationNotice()
        {
            try
            {
                return DossierFile.StepViolationNotice(ScenarioStep) != null;
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

        public DelegateCommand RemoveViolationResolveInfoCommand { get; private set; }

        private void RemoveViolationResolveInfo()
        {
            try
            {
                IsInfoCreated = false;
                DossierFile.RemoveStepViolationResolveInfo(ScenarioStep);
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

        private bool CanRemoveViolationResolveInfo()
        {
            try
            {
                return DossierFile.StepViolationResolveInfo(ScenarioStep) != null;
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

        public DelegateCommand RemoveNoticeCommand { get; private set; }

        private void RemoveNotice()
        {
            try
            {
                IsNotice = false;
                DossierFile.RemoveStepNotice(ScenarioStep);
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

        private bool CanRemoveNotice()
        {
            try
            {
                return DossierFile.StepNotice(ScenarioStep) != null;
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
