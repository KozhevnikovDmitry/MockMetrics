using System;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.ViewModel.ExpertiseViewModel;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Класс ViewModel для занесения результатов документарной проверки
    /// </summary>
    public class ExpertiseStepVm : AbstractSupervisionStepVm
    {
        public ExpertiseStepVm(ISmartValidateableVm<DocumentExpertise> documentExpertiseVm,
                               DocExpertiseResultListVm documentExpertiseResultsVm)
        {
            DocumentExpertiseVm = documentExpertiseVm;
            DocumentExpertiseResultsVm = documentExpertiseResultsVm;
            IsExpertised = false;
            SetupExpertiseCommand = new DelegateCommand(SetupExpertise, CanSetupExpertise);
            RemoveExpertiseCommand = new DelegateCommand(RemoveExpertise, CanRemoveExpertise);
        }

        /// <summary>
        /// Конструирует дочерные vm'ы
        /// </summary>
        protected override void Rebuild()
        {
            if (DossierFile.StepDocumentExpertise(ScenarioStep) != null)
            {
                IsExpertised = true;

                _documentExpertiseVm.Initialize(DossierFile.StepDocumentExpertise(ScenarioStep));
                RaisePropertyChanged(() => DocumentExpertiseVm);
                DocumentExpertiseVm.RaiseAllPropertyChanged();

                _documentExpertiseResultsVm.Initialize(DossierFile.StepDocumentExpertise(ScenarioStep).ExperiseResultList);
                DocumentExpertiseResultsVm.ScenarioStep = ScenarioStep;
                RaisePropertyChanged(() => DocumentExpertiseResultsVm);
                DocumentExpertiseResultsVm.RaiseItemsValidatingPropertyChanged();
            }
            else
            {
                IsExpertised = false;
            }
            RaiseStepCommandsCanExecute();
        }

        /// <summary>
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public override void RaiseStepCommandsCanExecute()
        {
            base.RaiseStepCommandsCanExecute();
            SetupExpertiseCommand.RaiseCanExecuteChanged();
            RemoveExpertiseCommand.RaiseCanExecuteChanged();
        }

        public override void RaiseIsValidChanged()
        {
            if (DocumentExpertiseVm != null)
            {
                DocumentExpertiseVm.RaiseIsValidChanged();
            }
        }

        #region Binding Properties

        /// <summary>
        /// Флаг указыващий, на занесенность результатов проверки
        /// </summary>
        private bool _isExpertised;

        /// <summary>
        /// Возвращает или устанаваливает флаг, указыващий на занесенность результатов проверки
        /// </summary>
        public bool IsExpertised
        {
            get
            {
                return _isExpertised;
            }
            set
            {
                if (_isExpertised != value)
                {
                    _isExpertised = value;
                    RaisePropertyChanged(() => IsExpertised);
                }
            }
        }

        private ISmartValidateableVm<DocumentExpertise> _documentExpertiseVm;

        public ISmartValidateableVm<DocumentExpertise> DocumentExpertiseVm
        {
            get { return _documentExpertiseVm.Entity == null ? null : _documentExpertiseVm; }
            private set { _documentExpertiseVm = value; }
        }

        private DocExpertiseResultListVm _documentExpertiseResultsVm;

        public DocExpertiseResultListVm DocumentExpertiseResultsVm
        {
            get { return _documentExpertiseResultsVm; }
            private set { _documentExpertiseResultsVm = value; }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда заведения документарно проверки
        /// </summary>
        public DelegateCommand SetupExpertiseCommand { get; private set; }

        /// <summary>
        /// Заводит документарную проверку
        /// </summary>
        private void SetupExpertise()
        {
            try
            {
                Superviser.PrepareExpertise(ScenarioStep);
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

        /// <summary>
        /// Возвращает true, если можно завести документарную проверку
        /// </summary>
        /// <returns>Флаг возможности заведения документарной проверки</returns>
        private bool CanSetupExpertise()
        {
            return !IsExpertised && IsCurrentOrPrevious;
        }

        public DelegateCommand RemoveExpertiseCommand { get; private set; }

        private void RemoveExpertise()
        {
            try
            {
                DossierFile.RemoveStepDocumentExpertise(ScenarioStep);
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

        private bool CanRemoveExpertise()
        {
            try
            {
                return IsExpertised && IsCurrentOrPrevious;
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
