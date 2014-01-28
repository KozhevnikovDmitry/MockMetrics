using Common.BL.ReportMapping;

using GU.BL.Reporting.Data;
using GU.DataModel;

namespace GU.BL.Reporting.Mapping
{
    public class TaskInfoReport : IReport
    {
        private readonly Task _task;

        public TaskInfoReport(Task task)
        {
            _task = task;
            DataAlias = "data"; 
            ViewPath = "Reporting/View/Common/TaskInfoReport.mrt";
        }

        public object RetrieveData()
        {
            return new TaskInfo
                {
                    Agency = _task.Service.ServiceGroup.Agency.Name,
                    Service = string.Format("{0} ({1})", _task.Service.Name, _task.Service.ServiceGroup.ServiceGroupName),
                    Customer = _task.CustomerFio,
                    TaskId = _task.Id,
                    CreateDate = _task.CreateDate.Value,
                    AuthCode = _task.AuthCode
                };
        }

        public string ViewPath { get; private set; }

        public string DataAlias { get; private set; }
    }
}
