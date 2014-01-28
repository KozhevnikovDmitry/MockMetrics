using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Person;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel
{
    public class AddOrderAgreeVm : NotificationObject
    {
        public AddOrderAgreeVm(IDictionaryManager dictionaryManager)
        {
            EmployeeList = dictionaryManager.GetDynamicDictionary<Employee>();
            Employee = EmployeeList.First();
        }

        #region Binding Properties

        public Employee Employee { get; set; }

        public List<Employee> EmployeeList { get; set; } 

        #endregion
    }
}
