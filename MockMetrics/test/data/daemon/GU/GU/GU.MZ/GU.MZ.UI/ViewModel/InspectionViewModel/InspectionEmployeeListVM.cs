using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.View.InspectionView;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.InspectionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения списка сотрудников, привлечённых к проверке
    /// </summary>
    public class InspectionEmployeeListVm : SmartListVm<InspectionEmployee>
    {
        public ScenarioStep ScenarioStep { get; set; }

        private SupervisionFacade _superviser;

        private IDialogUiFactory _uiFactory;

        public InspectionEmployeeListVm(SupervisionFacade superviser, IDialogUiFactory uiFactory)
        {
            _superviser = superviser;
            _uiFactory = uiFactory;
        }

        /// <summary>
        /// Добавляет новый результат проверки в список.
        /// </summary>
        protected override void AddItem()
        {
            try
            {
                if (!_superviser.GetAvailableEmployees(ScenarioStep).Any())
                {
                    NoticeUser.ShowInformation("Больше сотрудников к проверке привлечь нельзя");
                    return;
                }

                var vm = new AddInspectionEmployeeVm(_superviser.GetAvailableEmployees(ScenarioStep));
                if (_uiFactory.ShowDialogView(
                    new AddInspectionEmployeeView(), vm, "Привлечь сотрудника к проверке"))
                {
                    var inspectionEmployee = InspectionEmployee.CreateInstance();
                    inspectionEmployee.Employee = vm.Employee;
                    inspectionEmployee.EmployeeId = vm.Employee.Id;
                    Items.Add(inspectionEmployee);
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
    }
}
