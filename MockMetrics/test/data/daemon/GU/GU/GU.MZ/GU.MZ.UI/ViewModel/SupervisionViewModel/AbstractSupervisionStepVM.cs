using System;
using System.Linq;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.UI.View.EmployeeView;
using GU.MZ.UI.ViewModel.EmployeeViewModel;
using GU.MZ.UI.ViewModel.SupervisionViewModel.Event;
using GU.MZ.UI.ViewModel.SupervisionViewModel.Interface;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Базовый класс для классов ViewModel отображения этапов ведения тома.
    /// </summary>
    public abstract class AbstractSupervisionStepVm : NotificationObject, ISupervisionStepVm
    {
        /// <summary>
        /// Супервайзер тома
        /// </summary>
        protected SupervisionFacade Superviser;
        protected IDialogUiFactory UiFactory;

        /// <summary>
        /// Базовый класс для классов ViewModel отображения этапов ведения тома.
        /// </summary>
        protected AbstractSupervisionStepVm()
        {
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            StepNextCommand = new DelegateCommand(StepNext, CanStepNext);
        }
        
        #region Properties

        protected bool IsCurrentOrPrevious
        {
            get
            {
                return Superviser.DossierFile.CurrentScenarioStep.SortOrder >= ScenarioStep.SortOrder; 
            }
        }

        protected ChooseResponsibleEmployeeVm ChooseResponsibleEmployeeVm { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Событие запроса на пересобирание vm'a редактирования тома
        /// </summary>
        public event ViewModelRebuildRequested RebuildRequested;

        /// <summary>
        /// Возбуждает событие запроса на пересобирание vm'a редактирования тома
        /// </summary>
        /// <param name="dossierFile">Сохранённый том</param>
        protected void RaiseRebuildRequested(DossierFile dossierFile)
        {
            if (RebuildRequested != null)
            {
                RebuildRequested(this, new ViewModelRebuildRequestedEventArgs(dossierFile));
            }
        }

        /// <summary>
        /// Событие, оповещающее о запросе на переход к следующему этапу
        /// </summary>
        public event NextStepRequested StepNextRequested;

        /// <summary>
        /// Возбуждает событие, оповещающее о запросе на переход к следующему этапу
        /// </summary>
        protected void RaiseStepNextRequested()
        {
            if (StepNextRequested != null)
            {
                StepNextRequested(this, new EventArgs());
            }
        }

        #endregion

        #region ISupervisionStepVm

        /// <summary>
        /// Том лицензионного дела  
        /// </summary>
        protected DossierFile DossierFile
        {
            get
            {
                return Superviser.DossierFile;
            }
        }

        /// <summary>
        /// Этап ведения тома.
        /// </summary>
        public ScenarioStep ScenarioStep { get; private set; }

        /// <summary>
        /// Команда перехода к следующему этапу ведения тома
        /// </summary>
        public DelegateCommand StepNextCommand { get; private set; }

        /// <summary>
        /// Осуществляет переход тома на следующий этап ведения
        /// </summary>
        protected virtual void StepNext()
        {
            try
            {
                ChooseResponsibleEmployeeVm.Initialize(DossierFile.Employee,
                                                       Superviser.GetNextScenarioStep());
                if (UiFactory.ShowDialogView(new ChooseResponsibleEmployeeView(),
                                              ChooseResponsibleEmployeeVm,
                                              "Переход к следующему этапу"))
                {
                    var nextResponsibleEmployee =
                        ChooseResponsibleEmployeeVm.EmployeeList.Single(t => t.Id == ChooseResponsibleEmployeeVm.EmployeeId);

                    Superviser.StepNext(nextResponsibleEmployee);
                    RaiseStepNextRequested();
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

        protected void StepNext(TaskStatusType taskStatusType)
        {
            if (DossierFile.IsDirty)
            {
                NoticeUser.ShowInformation("Необходимо сохранить изменения в томе перед переходом к следующему этапу.");
                return;
            }

            if (!Superviser.CanSetStatus(taskStatusType))
            {
                NoticeUser.ShowWarning("Невозможно перейти к следующему этапу с текущим статусом заявки.");
                return;
            }

            ChooseResponsibleEmployeeVm.Initialize(DossierFile.Employee,
                                                   Superviser.GetNextScenarioStep());

            if (UiFactory.ShowDialogView(
                new ChooseResponsibleEmployeeView(), ChooseResponsibleEmployeeVm, "Переход к следующему этапу"))
            {
                var nextResponsibleEmployee =
                    ChooseResponsibleEmployeeVm.EmployeeList.Single(t => t.Id == ChooseResponsibleEmployeeVm.EmployeeId);

                Superviser.StepNextWithStatus(nextResponsibleEmployee, taskStatusType);
                RaiseStepNextRequested();
                RaiseRebuildRequested(DossierFile);
            }
        }

        protected void RejectAndStepNext()
        {
            if (Superviser.IsRejectedOrderPrepared(ScenarioStep) && DossierFile.IsRejected)
            {
                if (NoticeUser.ShowQuestionYesNo(
                    "Издан приказ запрещающий предоставление услуги. Присвоить заявке статус Отклонена и сохранить?") ==
                    MessageBoxResult.Yes)
                {
                    if (Superviser.CanSetStatus(TaskStatusType.Rejected))
                    {
                        ChooseResponsibleEmployeeVm.Initialize(DossierFile.Employee,
                                                     Superviser.GetNextScenarioStep());
                        if (UiFactory.ShowDialogView(new ChooseResponsibleEmployeeView(), ChooseResponsibleEmployeeVm, "Переход к следующему этапу"))
                        {
                            var nextResponsibleEmployee =
                                ChooseResponsibleEmployeeVm.EmployeeList.Single(t => t.Id == ChooseResponsibleEmployeeVm.EmployeeId);

                            Superviser.StepNextWithStatus(nextResponsibleEmployee, TaskStatusType.Ready);
                            RaiseStepNextRequested();
                            RaiseRebuildRequested(DossierFile);
                        }
                    }
                    else
                    {
                        throw new BLLException("Невозможно выставить статус Отклонено для заявления");
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает true, если можно перейти к следующему этапу ведения тома
        /// </summary>
        /// <returns>Флаг возможности перехода к следующему этапу</returns>
        protected virtual bool CanStepNext()
        {
            try
            {
                return Superviser.CanStepNextFromStep(ScenarioStep);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
                return false;
            }
        }

        /// <summary>
        /// Оповещает о смене режима доступности команд VM'а
        /// </summary>
        public virtual void RaiseStepCommandsCanExecute()
        {
            StepNextCommand.RaiseCanExecuteChanged();
        }

        public virtual void OnSave()
        {
            StepNextCommand.RaiseCanExecuteChanged();
        }

        public void BaseInit(IDialogUiFactory uiFactory, ChooseResponsibleEmployeeVm chooseResponsibleEmployeeVm, ScenarioStep scenarioStep)
        {
            ScenarioStep = scenarioStep;
            UiFactory = uiFactory;
            ChooseResponsibleEmployeeVm = chooseResponsibleEmployeeVm;
        }
        public virtual void CustInit( SupervisionFacade superviser)
        {
            Superviser = superviser;
            Rebuild();
        }

        public bool IsInitialized
        {
            get { return Superviser != null; } 
        }


        protected abstract void Rebuild();

        #endregion

        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion

        #region IBaseValidateableVM

        public virtual void RaiseIsValidChanged()
        {
            
        }

        #endregion
    }
}
