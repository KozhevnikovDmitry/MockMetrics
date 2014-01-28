using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using GU.DataModel;

namespace GU.ZLP.ViewModel.Report
{
    public class GetIntervalAndTaskStatusVM : NotificationObject
    {
        public GetIntervalAndTaskStatusVM()
        {
            Date1 = null; 
            Date2 = DateTime.Now;
            TaskStatusList = GU.BL.GuFacade.GetDictionaryManager().GetEnumDictionary<TaskStatusType>();
            TaskStatus = TaskStatusList.Keys.First();
        }

        #region Binding Properties

        private DateTime? _date1;

        public DateTime? Date1
        {
            get
            {
                return _date1;
            }
            set
            {
                if (_date1 != value)
                {
                    _date1 = value;
                    RaisePropertyChanged(() => Date1);
                }
            }
        }

        private DateTime? _date2;

        public DateTime? Date2
        {
            get
            {
                return _date2;
            }
            set
            {
                if (_date2 != value)
                {
                    _date2 = value;
                    RaisePropertyChanged(() => Date2);
                }
            }
        }

        private int _taskStatus;

        public int TaskStatus
        {
            get
            {
                return _taskStatus;
            }
            set
            {
                if (_taskStatus != value)
                {
                    _taskStatus = value;
                    RaisePropertyChanged(() => TaskStatus);
                }
            }
        }

        public Dictionary<int, string> TaskStatusList { get; private set; }      

        #endregion
    }
}
