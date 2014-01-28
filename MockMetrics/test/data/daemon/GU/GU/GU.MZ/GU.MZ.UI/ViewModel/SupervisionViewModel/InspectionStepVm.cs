using System;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.ViewModel.InspectionViewModel;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения этапа ведения "Выездная проверка"
    /// </summary>
    public class InspectionStepVm : AbstractSupervisionStepVm
    {
        /// <summary>
        /// Класс ViewModel для отображения этапа ведения "Выездная проверка"
        /// </summary>
        public InspectionStepVm(ISmartValidateableVm<Inspection> inspectionVm,
                                InspectionEmployeeListVm employeeListVm,
                                InspectionExpertListVm expertListVm)
        {
            InspectionVm = inspectionVm;
            EmployeeListVm = employeeListVm;
            ExpertListVm = expertListVm;
            IsInspected = false;
            SetupInspectionCommand = new DelegateCommand(SetupInspection, CanSetupInspection);
            RemoveInspectionCommand = new DelegateCommand(RemoveInspection, CanRemoveInspection);
        }

        /// <summary>
        /// Конструирует дочерные vm'ы
        /// </summary>
        protected override void Rebuild()
        {
            if (DossierFile.StepInspection(ScenarioStep) != null)
            {
                IsInspected = true;

                _inspectionVm.Initialize(DossierFile.StepInspection(ScenarioStep));
                RaisePropertyChanged(() => InspectionVm);
                InspectionVm.RaiseAllPropertyChanged();

                _expertListVm.Initialize(DossierFile.StepInspection(ScenarioStep).InspectionExpertList);
                ExpertListVm.ScenarioStep = ScenarioStep;
                RaisePropertyChanged(() => ExpertListVm);
                ExpertListVm.RaiseItemsValidatingPropertyChanged();

                _employeeListVm.Initialize(DossierFile.StepInspection(ScenarioStep).InspectionEmployeeList);
                EmployeeListVm.ScenarioStep = ScenarioStep;
                RaisePropertyChanged(() => EmployeeListVm);
                EmployeeListVm.RaiseItemsValidatingPropertyChanged();
            }
            else
            {
                IsInspected = false;
            }
            RaiseStepCommandsCanExecute();
        }

        /// <summary>
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            SetupInspectionCommand.RaiseCanExecuteChanged();
            RemoveInspectionCommand.RaiseCanExecuteChanged();
        }

        public override void RaiseIsValidChanged()
        {
            if (InspectionVm != null)
            {
                InspectionVm.RaiseIsValidChanged();
            }
        }

        #region Binding Properties

        /// <summary>
        /// Флаг указыващий, на занесенность результатов проверки
        /// </summary>
        private bool _isInspected;

        private ISmartValidateableVm<Inspection> _inspectionVm;
        private InspectionEmployeeListVm _employeeListVm;
        private InspectionExpertListVm _expertListVm;

        /// <summary>
        /// Возвращает или устанаваливает флаг, указыващий на занесенность результатов проверки
        /// </summary>
        public bool IsInspected
        {
            get
            {
                return _isInspected;
            }
            set
            {
                if (_isInspected != value)
                {
                    _isInspected = value;
                    RaisePropertyChanged(() => IsInspected);
                }
            }
        }

        public ISmartValidateableVm<Inspection> InspectionVm
        {
            get { return _inspectionVm.Entity == null? null : _inspectionVm; }
            private set { _inspectionVm = value; }
        }

        public InspectionEmployeeListVm EmployeeListVm
        {
            get { return _employeeListVm; }
            private set { _employeeListVm = value; }
        }

        public InspectionExpertListVm ExpertListVm
        {
            get { return _expertListVm; }
            private set { _expertListVm = value; }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand SetupInspectionCommand { get; private set; }

        private void SetupInspection()
        {
            try
            {
                Superviser.PrepareHolderInspection(ScenarioStep);
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

        private bool CanSetupInspection()
        {
            return !IsInspected && IsCurrentOrPrevious;
        }

        public DelegateCommand RemoveInspectionCommand { get; private set; }

        private void RemoveInspection()
        {
            try
            {
                DossierFile.RemoveStepInspection(ScenarioStep);
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

        private bool CanRemoveInspection()
        {
            try
            {
                return IsInspected && IsCurrentOrPrevious;
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
