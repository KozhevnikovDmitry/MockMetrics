using System.Collections.Generic;

using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.BL.ReportMapping;
using Common.DA.Interface;

using GU.BL.Reporting.Data;
using GU.DataModel;

using System.Linq;

namespace GU.BL.Reporting.Mapping
{
    public class TaskDataListReport : DbReport
    {
        private readonly string _username;

        private readonly IDictionaryManager _dictionaryManager;

        private readonly IDomainDataMapper<Task> _taskDataMapper;

        public TaskDataListReport(IDomainContext domainContext, 
                                  string username, 
                                  IDictionaryManager dictionaryManager,
                                  IDomainDataMapper<Task> taskDataMapper)
            : base(domainContext)
        {
            _username = username;
            _dictionaryManager = dictionaryManager;
            _taskDataMapper = taskDataMapper;
            ViewPath = "Reporting/View/Common/TaskDataListReport.mrt";
        }

        protected override object RetrieveOperation(IDomainDbManager db)
        {
            var tasks = db.GetDomainTable<Task>().Select(t => t.Id).ToList();

            var result = new TaskDataList
                             {
                                 TaskData =
                                     tasks.Select(
                                         t =>
                                         new TaskDataReport(
                                             _taskDataMapper.Retrieve(t, db), _username, _dictionaryManager).RetrieveData() as TaskData)
                                          .ToList(),
                                 Username = _username
                             };

             return result;

            return new TaskDataList { TaskData = new List<TaskData>(), Username = _username };
        }
    }
}
