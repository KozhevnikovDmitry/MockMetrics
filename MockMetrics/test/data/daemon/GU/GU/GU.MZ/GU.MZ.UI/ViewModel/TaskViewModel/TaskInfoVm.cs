using System;
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using GU.BL.Extensions;
using GU.DataModel;
using GU.MZ.UI.ViewModel.SmartViewModel;
using GU.UI.Extension;

namespace GU.MZ.UI.ViewModel.TaskViewModel
{
    public class TaskInfoVm : EntityInfoVm<Task>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public TaskInfoVm(IDomainDataMapper<Task> entityMapper, IDictionaryManager dictionaryManager) 
            : base(entityMapper)
        {
            _dictionaryManager = dictionaryManager;
        }

        #region Binding Properties

        public string TaskDataString
        {
            get
            {
                var result = string.Format("ID = {0}", Entity.Id);
                if (Entity.CreateDate.HasValue)
                {
                    result += string.Format(" Дата подачи: {0} {1}", Entity.CreateDate.Value.ToLongDateString(), Entity.CreateDate.Value.ToLongTimeString());
                }
                return result;
            }
        }

        public string ShortTaskString { get { return string.Format("ID = {0}", Entity.Id); } }

        public string ServiceDataString { get { return string.Format("{0} ({1})", Entity.Service.Name, Entity.Service.ServiceGroup.ServiceGroupName); } }

        public string ReceiverDataString { get { return string.Format("{0}, тел: {1},  email: {2}", Entity.CustomerFio, Entity.CustomerPhone, Entity.CustomerEmail); } }

        public string StatusDataString
        {
            get
            {
                var status = Entity.StatusList.OrderByDescending(t => t.Stamp).FirstOrDefault();
                if (status != null)
                {
                    return string.Format("{0} {1} {2}", ShortStatusString, status.Stamp.ToLongDateString(), status.Stamp.ToLongTimeString());
                }
                return ShortStatusString;
            }
        }

        public string ShortStatusString { get { return _dictionaryManager.GetEnumDictionary<TaskStatusType>()[(int)Entity.CurrentState]; } }

        public string IconPath { get { return Entity.CurrentState.GetIconPath(); } }

        public string TimeLeftString
        {
            get
            {
                if (Entity.DueDate.HasValue)
                {
                    return string.Format("Выполнить до : {0}", Entity.DueDate.Value.ToLongDateString());
                }
                return string.Empty;
            }
        }

        public bool IsDeadlineSoon
        {
            get
            {
                if (Entity.DueDate.HasValue)
                {
                    return Entity.DueDate.Value <= DateTime.Today.AddWorkingDays(3);
                }
                return false;
            }
        }

        public bool IsDeadlineFailed
        {
            get
            {
                if (Entity.DueDate.HasValue)
                {
                    return Entity.DueDate.Value < DateTime.Today;
                }
                return false;
            }
        }

        public string AgencyName { get { return Entity.Agency.Name; } }

        public string LastChangesStamp { get { return Entity.CommonData == null ? string.Empty : Entity.CommonData.Stamp.ToString(); } }

        #endregion
    }
}
