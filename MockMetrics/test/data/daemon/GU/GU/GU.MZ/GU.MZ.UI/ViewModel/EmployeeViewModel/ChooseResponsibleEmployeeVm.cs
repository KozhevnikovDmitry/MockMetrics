using System.Collections.Generic;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Person;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.EmployeeViewModel
{
    /// <summary>
    /// Класс View для диалога выбора отвественного соисполнителя для следующего этапа
    /// </summary>
    public class ChooseResponsibleEmployeeVm : NotificationObject
    {
        /// <summary>
        /// Класс View для диалога выбора отвественного соисполнителя для следующего этапа
        /// </summary>
        public ChooseResponsibleEmployeeVm(IDictionaryManager dictionaryManager)
        {
            EmployeeList = dictionaryManager.GetDynamicDictionary<Employee>();
        }

        public void Initialize(Employee employee, ScenarioStep scenarioStep)
        {
            EmployeeId = employee.Id;
            NextScenarioStepName = scenarioStep.Name;
        }

        /// <summary>
        /// Наименование следующего этапа
        /// </summary>
        public string NextScenarioStepName { get; private set; }

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public List<Employee> EmployeeList { get; set; }

        /// <summary>
        /// Id выбранного сотрудника
        /// </summary>
        private int _employeeId;

        /// <summary>
        /// Возвращает или устанавливает id выбранного сотрудника
        /// </summary>
        public int EmployeeId
        {
            get
            {
                return _employeeId;
            }
            set
            {
                if (_employeeId != value)
                {
                    _employeeId = value;
                    RaisePropertyChanged(() => EmployeeId);
                }
            }
        }
    }
}
