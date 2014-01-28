using System;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    public abstract class BaseOrderStepVm : AbstractSupervisionStepVm
    {
        /// <summary>
        /// Класс ViewModel для отображения процесса подготовки и издания приказа 
        /// о возврате или приёме документов.
        /// </summary>
        protected BaseOrderStepVm(ISmartValidateableVm<Notice> noticeVm)
        {
            IsPublish = false;
            IsNotice = false;
            NoticeVm = noticeVm;
            PrepareOrderNoticeCommand = new DelegateCommand(PrepareOrderNotice, CanPrepareOrderNotice);
            RemoveNoticeCommand = new DelegateCommand(RemoveNotice, CanRemoveNotice);
        }
        
        /// <summary>
        /// Конструирует дочерные vm'ы
        /// </summary>
        protected override void Rebuild()
        {
            if (DossierFile.StepNotice(ScenarioStep) != null)
            {
                IsNotice = true;
                _noticeVm.Initialize(DossierFile.StepNotice(ScenarioStep));
                RaisePropertyChanged(() => NoticeVm);
                NoticeVm.RaiseAllPropertyChanged();
            }
        }

        /// <summary>
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public override void RaiseStepCommandsCanExecute()
        {
            StepNextCommand.RaiseCanExecuteChanged();
            PrepareOrderNoticeCommand.RaiseCanExecuteChanged();
            RemoveNoticeCommand.RaiseCanExecuteChanged();
        }

        public override void RaiseIsValidChanged()
        {
            if (NoticeVm != null)
            {
                NoticeVm.RaiseIsValidChanged();
            }
        }

        #region Binding Properties

        protected ISmartValidateableVm<Notice> _noticeVm;

        public ISmartValidateableVm<Notice> NoticeVm
        {
            get
            {
                return _noticeVm.Entity == null? null : _noticeVm;
            }
            private set { _noticeVm = value; }
        }

        /// <summary>
        /// Флаг указыващий на подготовленность проекта приказа
        /// </summary>
        private bool _isPublish;

        /// <summary>
        /// Возвращает или устанаваливает флаг указыващий на подготовленность проекта приказа
        /// </summary>
        public bool IsPublish
        {
            get
            {
                return _isPublish;
            }
            set
            {
                if (_isPublish != value)
                {
                    _isPublish = value;
                    RaisePropertyChanged(() => IsPublish);
                }
            }
        }

        /// <summary>
        /// Флаг указыващий на отправку уведомления
        /// </summary>
        private bool _isNotice;

        /// <summary>
        /// Возвращает или устанаваливает флаг указыващий на отправку уведомления
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

        #endregion

        #region Binding Commands

        public DelegateCommand PrepareOrderNoticeCommand { get; private set; }

        protected virtual void PrepareOrderNotice()
        {
            try
            {
                var notice = Superviser.AddNotice(DossierFile.StepStandartOrder(ScenarioStep).OrderType.ToNoticeType(), ScenarioStep);
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

        protected virtual bool CanPrepareOrderNotice()
        {
            try
            {
                return DossierFile.StepStandartOrder(ScenarioStep) != null && DossierFile.StepNotice(ScenarioStep) == null;
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