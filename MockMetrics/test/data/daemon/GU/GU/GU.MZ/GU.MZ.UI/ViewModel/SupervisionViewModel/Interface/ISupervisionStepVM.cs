using Common.UI.Interface;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Interfaces;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.UI.ViewModel.EmployeeViewModel;
using GU.MZ.UI.ViewModel.SupervisionViewModel.Event;

using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel.Interface
{
    /// <summary>
    /// »нтерфейс классов ViewModel отображени€ этапов ведени€ тома.
    /// </summary>
    public interface ISupervisionStepVm : IBaseValidateableVm, IAvalonDockCaller, IRebuildRequestPublisher
    {
        /// <summary>
        /// Ётап ведени€ дела
        /// </summary>
        ScenarioStep ScenarioStep { get; }

        /// <summary>
        ///  оманда перехода к следующему этапу ведени€ тома
        /// </summary>
        DelegateCommand StepNextCommand { get; }

        /// <summary>
        /// —обытие, оповещающее о запросе на переход к следующему этапу
        /// </summary>
        event NextStepRequested StepNextRequested;

        /// <summary>
        /// ќповещает о смене режима доступности команд VM'а
        /// </summary>
        void RaiseStepCommandsCanExecute();

        void OnSave();

        void BaseInit(IDialogUiFactory uiFactory, ChooseResponsibleEmployeeVm chooseResponsibleEmployeeVm, ScenarioStep scenarioStep);

        void CustInit(SupervisionFacade superviser);

        bool IsInitialized { get; }
    }
}