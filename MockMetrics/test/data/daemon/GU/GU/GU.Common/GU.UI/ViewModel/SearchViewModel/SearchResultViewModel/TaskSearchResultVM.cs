using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.SearchViewModel;
using GU.BL;
using GU.DataModel;
using GU.UI.Extension;
using GU.BL.Extensions;

namespace GU.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    public class TaskSearchResultVM : AbstractSearchResultVM<Task>
    {
        public TaskSearchResultVM(Task entity)
            : base(entity)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
            try
            {
                TaskDataString = string.Format("ID = {0}", Result.Id);
                AgencyName = Result.Agency.Name;
                LastChangesStamp = Result.CommonData == null ? string.Empty : Result.CommonData.Stamp.ToString();
                if (Result.CreateDate.HasValue)
                {
                    TaskDataString += string.Format(" Дата подачи: {0} {1}", Result.CreateDate.Value.ToLongDateString(), Result.CreateDate.Value.ToLongTimeString());
                }
                ShortTaskString = string.Format("ID = {0}", Result.Id);
                ServiceDataString = string.Format("{0} ({1})", Result.Service.Name, Result.Service.ServiceGroup.ServiceGroupName);
                ReceiverDataString = string.Format("{0}, тел: {1},  email: {2}", Result.CustomerFio, Result.CustomerPhone, Result.CustomerEmail);
                var status = Result.StatusList.OrderByDescending(t => t.Stamp).FirstOrDefault();
                ShortStatusString = GuFacade.GetDictionaryManager().GetEnumDictionary<TaskStatusType>()[(int)Result.CurrentState];
                if (status != null)
                {
                    StatusDataString = string.Format("{0} {1} {2}", ShortStatusString, status.Stamp.ToLongDateString(), status.Stamp.ToLongTimeString());
                }
                else
                {
                    StatusDataString = ShortStatusString;
                }
                IconPath = Result.CurrentState.GetIconPath();
                if (Result.DueDate.HasValue)
                {
                    TimeLeftString = string.Format("Выполнить до : {0}", Result.DueDate.Value.ToLongDateString());
                    //TODO :  сделано за три дня до дедлайна, видимо надо сделать погибче
                    IsDeadlineSoon = Result.DueDate.Value <= DateTime.Today.AddWorkingDays(3);
                    IsDeadlineFailed = Result.DueDate.Value < DateTime.Today;
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

        #region Binding Properties

        public string TaskDataString { get; private set; }

        public string ShortTaskString { get; private set; }

        public string ServiceDataString { get; private set; }

        public string ReceiverDataString { get; private set; }

        public string StatusDataString { get; private set; }

        public string ShortStatusString { get; private set; }

        public string IconPath { get; private set; }

        public string TimeLeftString { get; private set; }

        public bool IsDeadlineSoon { get; private set; }

        public bool IsDeadlineFailed { get; private set; }

        public string AgencyName { get; private set; }

        public string LastChangesStamp { get; private set; }
                
        #endregion
    }
}
