using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Common.Types.Exceptions;
using Common.UI;
using Common.UI.CustomControl;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.SupervisionViewModel.Event;
using GU.MZ.UI.ViewModel.SupervisionViewModel.Interface;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Класс ViewModel для вкладку "Ведение тома лицензионного дела"
    /// </summary>
    public class FileSupervisionVm : NotificationObject, IAvalonDockCaller, IRebuildRequestPublisher, IBaseValidateableVm, IDisposable
    {
        private readonly ISupervisionViewFacotry _uiFactory;
        private readonly SupervisionFacade _superviser;

        /// <summary>
        /// Том лицензионного дела  
        /// </summary>
        public DossierFile DossierFile { get; private set; }
        
        /// <summary>
        /// Слушатель слабых событий запроса на переход к следующему этапу ведения
        /// </summary>
        private readonly IWeakEventListener _nextStepListener;

        /// <summary>
        /// Слушатель слабых событий запроса на пересобирание vm'а редактирования тома
        /// </summary>
        private readonly IWeakEventListener _vmRebuildListener;

        /// <summary>
        /// Класс ViewModel для вкладку "Ведение тома лицензионного дела"
        /// </summary>
        public FileSupervisionVm(ISupervisionViewFacotry uiFactory, SupervisionFacade superviser)
        {
            _uiFactory = uiFactory;
            _superviser = superviser;
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            NextStepCommand = new DelegateCommand(NextStep, CanNextStep);
            PreviousStepCommand = new DelegateCommand(PreviousStep, CanPreviousStep);
            _nextStepListener = new WeakEventListener<EventArgs>((s,e) => OnStepNextRequested());
            _vmRebuildListener =
                new WeakEventListener<ViewModelRebuildRequestedEventArgs>(OnRebuildRequsted);
            CollectSteps();
        }

        private void CollectSteps()
        {
            var steps = _uiFactory.ResolveSupervisionSteps();
            StepViewList = new List<UserControl>();
            foreach (var step in steps)
            {
                step.Key.DataContext = step.Value;
                NextStepRequestedWeakEventManager.AddListener(step.Value, _nextStepListener);
                VmRebuildRequestedWeakEventManager.AddListener(step.Value, _vmRebuildListener);
                AvalonInteractor.RegisterCaller(step.Value);
                StepViewList.Add(step.Key);
            }
        }


        public void Initialize(DossierFile dossierFile)
        {
            DossierFile = dossierFile;
            var stepVms = StepViewList.Select(t => t.DataContext).Cast<ISupervisionStepVm>().ToList();

            foreach (var stepVm in stepVms)
            {
                stepVm.CustInit(_superviser);
            }

            if (CurrentStepView == null)
            {
                int currentStepIndex =
                    stepVms.IndexOf(stepVms.SingleOrDefault(t => t.ScenarioStep.Id == dossierFile.CurrentScenarioStepId));
                CurrentStepView = StepViewList[currentStepIndex];
            }
        }


        #region Events

        /// <summary>
        /// Событие запроса на пересобирание vm'a редактирования тома
        /// </summary>
        public event ViewModelRebuildRequested RebuildRequested;

        #endregion

        #region EventHandling

        /// <summary>
        /// Обрабатывает запрос на переход к следующему этапу ведения тома
        /// </summary>
        private void OnStepNextRequested()
        {
            try
            {
                NextStepCommand.Execute();
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
        /// Пробрасывает событие запроса на пересобирание vm'а редактирования тома
        /// </summary>
        private void OnRebuildRequsted(object sender, ViewModelRebuildRequestedEventArgs eventArgs)
        {
            if (RebuildRequested != null)
            {
                RebuildRequested(this, eventArgs);
            }
        }

        #endregion

        #region Supervision Mapping

        /// <summary>
        /// Флаг указывающий на то, что текущий этап ведения дела имеет специфичный алгоритм маппинга тома.
        /// </summary>
        public bool IsSpecificMapping 
        { 
            get
            {
                return _superviser.IsSpecificMapping;
            }
        }

        /// <summary>
        /// Сохраняет данные тома.
        /// </summary>
        /// <returns>Сохранённый том</returns>
        /// <exception cref="VMException">Специфическая операция сохранения не определена</exception>
        public DossierFile SaveDossierFile()
        {
            if (!IsSpecificMapping)
            {
               throw new VMException("Специфическая операция сохранения не определена");   
            }

            NextStepCommand.RaiseCanExecuteChanged();
            _superviser.SaveDossierFile();
            return _superviser.DossierFile;
        }

        #endregion

        #region Binding Properties
        

        /// <summary>
        /// Возвращает строку с с информацией о текущем этапе ведения тома лицензионного дела
        /// </summary>
        public string CurrentStepInfo
        {
            get
            {
                return CurrentStepVM.ScenarioStep.Name;
            }
        }

        /// <summary>
        /// Коллекция View этапов ведения дела
        /// </summary>
        public List<UserControl> StepViewList { get; private set; }

        /// <summary>
        /// View текущего этапа ведения тома.
        /// </summary>
        private UserControl _сurrentStepView;

        /// <summary>
        /// Возвращает или устанавливает View текущего этапа ведения дела
        /// </summary>
        public UserControl CurrentStepView
        {
            get
            {
                return _сurrentStepView;
            }
            set
            {
                if (_сurrentStepView != value)
                {
                    _сurrentStepView = value;
                    RaisePropertyChanged(() => CurrentStepView);
                }
            }
        }

        /// <summary>
        /// Возвращает ViewModel отображения текущего этапа ведения тома.
        /// </summary>
        protected ISupervisionStepVm CurrentStepVM
        {
            get
            {
                return CurrentStepView.DataContext as ISupervisionStepVm;
            }
        }

        /// <summary>
        /// Направление перелистывания этапов
        /// </summary>
        private AnimatedDirection _animatedDirection;

        /// <summary>
        /// Возвращает или устанавливает направление перелистывания этапов.
        /// </summary>
        public AnimatedDirection AnimatedDirection
        {
            get
            {
                return _animatedDirection;
            }
            set
            {
                if (_animatedDirection != value)
                {
                    _animatedDirection = value;
                    RaisePropertyChanged(() => AnimatedDirection);
                }
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand NotifyContentChangedCommand
        {
            get
            {
                return new DelegateCommand(() => RaisePropertyChanged(() => CurrentStepView));
            }
        }

        /// <summary>
        /// Команда перехода к следующему этапу ведения тома лицензионного дела.
        /// </summary>
        public DelegateCommand NextStepCommand { get; private set; }

        /// <summary>
        /// Осуществляет переход к следующему этапу ведения тома лицензионного дела.
        /// </summary>
        private void NextStep()
        {
            try
            {
                AnimatedDirection = AnimatedDirection.Forward;
                CurrentStepVM.RaiseStepCommandsCanExecute();
                CurrentStepView = StepViewList.ElementAt(StepViewList.IndexOf(CurrentStepView) + 1);
                CurrentStepVM.RaiseStepCommandsCanExecute();
                NextStepCommand.RaiseCanExecuteChanged();
                PreviousStepCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => CurrentStepInfo);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Unexpectable error", ex));
            }
        }

        /// <summary>
        /// Возвращает флаг возможности перехода к следующему этапу ведения тома лицензионного дела.
        /// </summary>
        /// <returns>Флаг возможности перехода к следующему этапу</returns>
        private bool CanNextStep()
        {
            return !Equals(CurrentStepView, StepViewList.Last());
        }

        /// <summary>
        /// Команда перехода к предыдущему этапу ведения тома лицензионного дела.
        /// </summary>
        public DelegateCommand PreviousStepCommand { get; private set; }

        /// <summary>
        /// Осуществляет переход к предыдущему этапу ведения тома лицензионного дела.
        /// </summary>
        private void PreviousStep()
        {
            try
            {
                AnimatedDirection = AnimatedDirection.Backward;
                CurrentStepView = StepViewList.ElementAt(StepViewList.IndexOf(CurrentStepView) - 1);
                NextStepCommand.RaiseCanExecuteChanged();
                PreviousStepCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => CurrentStepInfo);
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
        /// Возвращает флаг возможности перехода к предыдущему этапу ведения тома лицензионного дела.
        /// </summary>
        /// <returns>Флаг возможности перехода к предыдущему этапу</returns>
        private bool CanPreviousStep()
        {
            return !Equals(CurrentStepView, StepViewList.First());
        }

        #endregion

        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion

        #region IBaseValidateableVm

        public void RaiseIsValidChanged()
        {
            foreach (var stepVm in StepViewList.Select(t => t.DataContext).OfType<ISupervisionStepVm>())
            {
                stepVm.RaiseIsValidChanged();
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _superviser.Dispose();
        }

        #endregion
    }
}
