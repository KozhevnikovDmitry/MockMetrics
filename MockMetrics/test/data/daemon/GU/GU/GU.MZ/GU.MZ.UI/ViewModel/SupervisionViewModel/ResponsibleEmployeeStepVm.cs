using System;
using System.Collections.Generic;
using Common.BL.DictionaryManagement;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Класс ViewModel для этапа ведения тома "Назначение отвественного исполнителя"
    /// </summary>
    public class ResponsibleEmployeeStepVm : AbstractSupervisionStepVm
    {
        /// <summary>
        /// Класс ViewModel для этапа ведения тома "Назначение отвественного исполнителя"
        /// </summary>
        public ResponsibleEmployeeStepVm(IDictionaryManager dictionaryManager,
                                         IEntityInfoVm<Employee> employeeInfoVm) 
        {
            EmployeeInfoVm = employeeInfoVm;
            EmployeeInfoVm.OnTakeEntity += OnOpenEmployee;
            EmployeeList = dictionaryManager.GetDynamicDictionary<Employee>();
        }

        protected override void Rebuild()
        {
            ResponsibleEmployeeId = DossierFile.EmployeeId;
            EmployeeInfoVm.Initialize(DossierFile.Employee);
        }

        /// <summary>
        /// Обрабатывает события запроса на открытие вкладки с сотрудником.
        /// </summary>
        private void OnOpenEmployee(Employee employee)
        {
            try
            {
                AvalonInteractor.RaiseOpenEditableDockable(employee.ToString(), typeof(Employee), employee);
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
        
        #region Binding Properties

        public IEntityInfoVm<Employee> EmployeeInfoVm { get; private set; }

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public List<Employee> EmployeeList { get; private set; }

        /// <summary>
        /// Id ответственного сотрудника
        /// </summary>
        private int _responsibleEmployeeId;

        /// <summary>
        /// Возрвщает или устанавливает Id ответсвенного сотрудника
        /// </summary>
        public int ResponsibleEmployeeId
        {
            get
            {
                return _responsibleEmployeeId;
            }
            set
            {
                if (_responsibleEmployeeId != value)
                {
                    _responsibleEmployeeId = value;
                    RaisePropertyChanged(() => ResponsibleEmployeeId);
                }
            }
        }

        #endregion
    }
}
