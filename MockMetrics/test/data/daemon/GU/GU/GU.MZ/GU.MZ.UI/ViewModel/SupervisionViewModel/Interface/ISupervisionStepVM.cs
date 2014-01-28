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
    /// ��������� ������� ViewModel ����������� ������ ������� ����.
    /// </summary>
    public interface ISupervisionStepVm : IBaseValidateableVm, IAvalonDockCaller, IRebuildRequestPublisher
    {
        /// <summary>
        /// ���� ������� ����
        /// </summary>
        ScenarioStep ScenarioStep { get; }

        /// <summary>
        /// ������� �������� � ���������� ����� ������� ����
        /// </summary>
        DelegateCommand StepNextCommand { get; }

        /// <summary>
        /// �������, ����������� � ������� �� ������� � ���������� �����
        /// </summary>
        event NextStepRequested StepNextRequested;

        /// <summary>
        /// ��������� � ����� ������ ����������� ������ VM'�
        /// </summary>
        void RaiseStepCommandsCanExecute();

        void OnSave();

        void BaseInit(IDialogUiFactory uiFactory, ChooseResponsibleEmployeeVm chooseResponsibleEmployeeVm, ScenarioStep scenarioStep);

        void CustInit(SupervisionFacade superviser);

        bool IsInitialized { get; }
    }
}