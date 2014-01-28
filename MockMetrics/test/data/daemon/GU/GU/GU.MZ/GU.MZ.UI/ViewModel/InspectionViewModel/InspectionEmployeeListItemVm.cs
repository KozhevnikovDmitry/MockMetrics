using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.InspectionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных сотрудника в списке привлечённых к проверке
    /// </summary>
    public class InspectionEmployeeListItemVm : SmartListItemVm<InspectionEmployee>
    {
        public override void Initialize(InspectionEmployee entity)
        {
            base.Initialize(entity);
            EmployeeName = Entity.Employee.Name;
            EmployeeWorkData = string.Format(
                "{0}; тел {1}; email {2}", Entity.Employee.Position, Entity.Employee.Phone, Entity.Employee.Email);
        }

        #region Binding Properties

        private string _employeeName;

        /// <summary>
        /// Строка с ФИО о сотрудника
        /// </summary>
        public string EmployeeName
        {
            get { return _employeeName; }
            private set
            {
                if (value != _employeeName)
                {
                    _employeeName = value;
                    RaisePropertyChanged(() => EmployeeName);
                }
            }
        }

        private string _employeeWorkData;

        /// <summary>
        /// Строка с рабочей информацией сотрудника
        /// </summary>
        public string EmployeeWorkData
        {
            get { return _employeeWorkData; }
            private set
            {
                if (value != _employeeWorkData)
                {
                _employeeWorkData = value;
                    RaisePropertyChanged(() => EmployeeWorkData);
                }
            }
        }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            RaisePropertyChanged(() => EmployeeName);
            RaisePropertyChanged(() => EmployeeWorkData);
            
        }
    }
}
