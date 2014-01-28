using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.Archive.BL;
using GU.Archive.DataModel;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.Archive.UI.ViewModel.PostViewModel
{
    public class AddPostExecutorVM : NotificationObject, IConfirmableVM
    {
        public AddPostExecutorVM()
        {
            using (var db = new ArchiveDbManager())
            {
                var list = db.GetDomainTable<Employee>().ToList();
                EmployeeList = new ListCollectionView(list);
            }
        }

        private Employee _employee;
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

        private ListCollectionView _employeeList;
        public ListCollectionView EmployeeList
        {
            get { return _employeeList; }
            set
            {
                if (_employeeList != value)
                {
                    _employeeList = value;
                    RaisePropertyChanged(() => EmployeeList);
                }
            }
        }

        private string _note;
        public string Note
        {
            get
            {
                return _note;
            }
            set
            {
                if (_note != value)
                {
                    _note = value;
                    RaisePropertyChanged(() => Note);
                }
            }
        }

        #region Implementation of IConfirmableVM

        public void Confirm()
        {
            if (Employee == null)
            {
                NoticeUser.ShowWarning("Необходимо указать исполнителя");
                return;
            }

            IsConfirmed = true;
        }

        public void ResetAfterFail()
        {
        }

        public bool IsConfirmed { get; private set; }

        #endregion
    }
}
