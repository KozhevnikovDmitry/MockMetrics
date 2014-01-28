using System.Collections.Generic;
using System.Linq;

using GU.MZ.DataModel.Person;

using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.InspectionViewModel
{
    /// <summary>
    /// Класс ViewModel выбора сотрудника
    /// </summary>
    public class AddInspectionEmployeeVm : NotificationObject
    {
        /// <summary>
        /// Класс ViewModel выбора сотрудника 
        /// </summary>
        /// <param name="employeeList">Список сотрудников</param>
        public AddInspectionEmployeeVm(List<Employee> employeeList)
        {
            EmployeeList = employeeList;
            Employee = employeeList.First();
        }

        #region Binding Properties

        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        private Employee _employee;

        /// <summary>
        /// Возвращает или устанавливает выбранного сотрудника
        /// </summary>
        public Employee Employee
        {
            get
            {
                return _employee;
            }
            set
            {
                if (_employee != value)
                {
                    _employee = value;
                    RaisePropertyChanged(() => Employee);
                }
            }
        }

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public List<Employee> EmployeeList { get; private set; }

        #endregion
    }
}
