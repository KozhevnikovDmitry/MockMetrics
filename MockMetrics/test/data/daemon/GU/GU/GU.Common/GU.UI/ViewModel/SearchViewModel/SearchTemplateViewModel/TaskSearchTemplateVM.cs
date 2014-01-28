using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.ViewModel;
using GU.DataModel;
using System.Windows.Data;
using GU.BL.Search.SearchDomain;

namespace GU.UI.ViewModel.SearchViewModel.SearchTemplateViewModel
{
    public class TaskSearchTemplateVM : NotificationObject
    {
        public TaskSearchTemplateVM(SearchTask searchTask)
        {
            SearchTask = searchTask;
            SearchTask.CustomerEmail = string.Empty;
            SearchTask.CustomerFio = string.IsNullOrEmpty(SearchTask.CustomerFio) ? string.Empty : SearchTask.CustomerFio;
            SearchTask.CustomerPhone = string.Empty;
            SearchTask.FakeServiceId = null;
            SearchTask.FakeCurrentState = null;
            TaskStatusList = new List<KeyValuePair<int?, string>>();
            TaskStatusList.Add(new KeyValuePair<int?, string>(-1, "<не указано>"));
            BL.GuFacade.GetDictionaryManager()
                .GetEnumDictionary<TaskStatusType>().ToList()
                .ForEach(t => TaskStatusList.Add(new KeyValuePair<int?, string>(t.Key, t.Value)));
            var list = BL.GuFacade.GetDictionaryManager().GetDictionary<Service>();
            var nullserv = Service.CreateInstance();
            nullserv.Id = 0;
            nullserv.Name = "<не указано>";
            nullserv.ServiceGroup = ServiceGroup.CreateInstance();
            nullserv.ServiceGroup.ServiceGroupName = "<не указано>";
            list.Add(nullserv);
            ServiceListView = new ListCollectionView(list.OrderBy(t => t.Id).ToList());
            ServiceListView.GroupDescriptions.Add(new PropertyGroupDescription("ServiceGroup.ServiceGroupName"));
        }

        public SearchTask SearchTask { get; private set; }

        #region Binding Properties

        public string TaskId
        {
            get
            {
                return SearchTask.FakeId.HasValue ? SearchTask.FakeId.ToString() : string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SearchTask.FakeId = null;
                }
                else
                {
                    try
                    {
                        SearchTask.FakeId = Convert.ToInt32(value);
                    }
                    catch (Exception)
                    {
                        SearchTask.FakeId = null;
                    }
                }
                RaisePropertyChanged(() => TaskId);
            }
        }

        public int? ServiceId
        {
            get
            {
                return SearchTask.FakeServiceId;
            }
            set
            {
                if (SearchTask.FakeServiceId != value)
                {
                    SearchTask.FakeServiceId = value == 0 ? null : value;
                    RaisePropertyChanged(() => ServiceId);
                }
            }
        }

        public int? CurrentState
        {
            get
            {
                return SearchTask.FakeCurrentState;
            }
            set
            {
                if (SearchTask.FakeCurrentState != value)
                {
                    SearchTask.FakeCurrentState = value == -1 ? null : value;
                    RaisePropertyChanged(() => CurrentState);
                }
            }
        }

        public ListCollectionView ServiceListView { get; set; }

        public List<KeyValuePair<int?, string>> TaskStatusList { get; private set; }

        public DateTime? CreateDate
        {
            get
            {
                return SearchTask.CreateDate;
            }
            set
            {
                if (SearchTask.CreateDate != value)
                {
                    SearchTask.CreateDate = value;
                    RaisePropertyChanged(() => CreateDate);
                }
            }
        }

        public DateTime? DueDate
        {
            get
            {
                return SearchTask.DueDate;
            }
            set
            {
                if (SearchTask.DueDate != value)
                {
                    SearchTask.DueDate = value;
                    RaisePropertyChanged(() => DueDate);
                }
            }
        }

        public string CustomerFio
        {
            get
            {
                return SearchTask.CustomerFio;
            }
            set
            {
                if (SearchTask.CustomerFio != value)
                {
                    SearchTask.CustomerFio = value;
                    RaisePropertyChanged(() => CustomerFio);
                }
            }
        }

        public string CustomerPhone
        {
            get
            {
                return SearchTask.CustomerPhone;
            }
            set
            {
                if (SearchTask.CustomerPhone != value)
                {
                    SearchTask.CustomerPhone = value;
                    RaisePropertyChanged(() => CustomerPhone);
                }
            }
        }

        public string CustomerEmail
        {
            get
            {
                return SearchTask.CustomerEmail;
            }
            set
            {
                if (SearchTask.CustomerEmail != value)
                {
                    SearchTask.CustomerEmail = value;
                    RaisePropertyChanged(() => CustomerEmail);
                }
            }
        }

        #endregion

        #region Binding Commands

        #endregion

    }
}
