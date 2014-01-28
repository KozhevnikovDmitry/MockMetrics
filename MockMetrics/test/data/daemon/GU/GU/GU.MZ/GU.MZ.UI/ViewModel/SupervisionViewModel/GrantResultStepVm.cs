using System;
using Common.DA;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.UI.ViewModel.DossierFileViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения этапа заведения результата предоставления услуги для тома лицензионного дела
    /// </summary>
    public class GrantResultStepVm : AbstractSupervisionStepVm
    {
        public GrantResultStepVm(DossierFileServiceResultVm dossierFileServiceResultVm)
        {
            DossierFileServiceResultVm = dossierFileServiceResultVm;
            SetupServiceResultCommand = new DelegateCommand(SetupServiceResult, CanSetupServiceResult);
            RemoveServiceResultCommand = new DelegateCommand(RemoveServiceResult, CanRemoveServiceResult);
        }

        /// <summary>
        /// Сохраняет данные тома.
        /// </summary>
        public override void OnSave()
        {
            base.OnSave();
            RaiseRebuildRequested(DossierFile);
        }

        protected override void Rebuild()
        {
            if (DossierFile.CurrentFileStep != null && DossierFile.DossierFileServiceResult != null)
            {
                IsResultCreated = true;
                _dossierFileServiceResultVm.Initialize(DossierFile.DossierFileServiceResult);
                DossierFileServiceResultVm.GrantResultProcedureString = ScenarioStep.Name;
                RaisePropertyChanged(() => DossierFileServiceResultVm);
                DossierFileServiceResultVm.RaiseAllPropertyChanged();
                IsResultGranted = DossierFile.DossierFileServiceResult.PersistentState == PersistentState.Old;
            }
            else
            {
                IsResultCreated = false;
                IsResultGranted = false;
            } 
            RaiseStepCommandsCanExecute();
        }

        /// <summary>
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            SetupServiceResultCommand.RaiseCanExecuteChanged();
            RemoveServiceResultCommand.RaiseCanExecuteChanged();
        }

        public override void RaiseIsValidChanged()
        {
            if (DossierFileServiceResultVm != null)
            {
                DossierFileServiceResultVm.RaiseIsValidChanged();
            }
        }

        /// <summary>
        /// Возвращает true, если можно перейти к следующему этапу ведения тома
        /// </summary>
        /// <returns>Флаг возможности перехода к следующему этапу</returns>
        protected override bool CanStepNext()
        {
            return false;
        }

        #region Binding Properties

        /// <summary>
        /// Флаг указыващий, на заведённость результата предоставдения услуги
        /// </summary>
        private bool _isResultCreated;

        /// <summary>
        /// Возвращает или устанаваливает флаг, указыващий на заведённость результата предоставдения услуги
        /// </summary>
        public bool IsResultCreated
        {
            get
            {
                return _isResultCreated;
            }
            set
            {
                if (_isResultCreated != value)
                {
                    _isResultCreated = value;
                    RaisePropertyChanged(() => IsResultCreated);
                }
            }
        }


        /// <summary>
        /// Флаг указыващий, на сохранённость результата предоставдения услуги
        /// </summary>
        private bool _isResultGranted;

        /// <summary>
        /// Флаг указыващий, на сохранённость результата предоставдения услуги
        /// </summary>
        public bool IsResultGranted
        {
            get
            {
                return _isResultGranted;
            }
            set
            {
                if (_isResultGranted != value)
                {
                    _isResultGranted = value;
                    RaisePropertyChanged(() => IsResultGranted);
                }
            }
        }
      

        private DossierFileServiceResultVm _dossierFileServiceResultVm;

        /// <summary>
        /// Возвращает или устанавливает Vm редактирования результата предоставления услуги
        /// </summary>
        public DossierFileServiceResultVm DossierFileServiceResultVm
        {
            get { return _dossierFileServiceResultVm.Entity == null? null : _dossierFileServiceResultVm; }
            private set { _dossierFileServiceResultVm = value; }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand SetupServiceResultCommand { get; private set; }

        private void SetupServiceResult()
        {
            try
            {
                Superviser.GrantServiseResult();
                Rebuild();
                
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

        private bool CanSetupServiceResult()
        {
            return DossierFile.DossierFileServiceResult == null && IsCurrentOrPrevious;
        }

        public DelegateCommand RemoveServiceResultCommand { get; private set; }

        private void RemoveServiceResult()
        {
            try
            {
                IsResultCreated = false;
                DossierFileServiceResultVm = null;
                DossierFile.RemoveStepServiceResult();
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

        private bool CanRemoveServiceResult()
        {
            try
            {
                return IsResultCreated && DossierFileServiceResultVm != null;
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
