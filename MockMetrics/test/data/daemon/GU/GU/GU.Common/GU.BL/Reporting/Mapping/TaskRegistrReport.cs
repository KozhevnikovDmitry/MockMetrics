using System;
using System.Linq;

using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.Types.Exceptions;

using GU.BL.Reporting.Data;
using GU.DataModel;

namespace GU.BL.Reporting.Mapping
{
    public class TaskRegistrReport : DbReport
    {
        private readonly string _username;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        private readonly IDictionaryManager _dictionaryManager;

        public TaskRegistrReport(string username, DateTime startDate, DateTime endDate, IDomainContext domainContext, IDictionaryManager dictionaryManager)
            : base(domainContext)
        {
            this._username = username;
            this._startDate = startDate;
            this._endDate = endDate;
            _dictionaryManager = dictionaryManager;
            ViewPath = "Reporting/View/Common/TaskRegistrReport.mrt";
        }

        protected override object RetrieveOperation(IDomainDbManager dbManager)
        {
            try
            {
                return new TaskRegistr
                    {
                        TaskRegistrStatList = (from t in dbManager.GetDomainTable<Task>().Where(t => t.CreateDate >= _startDate && t.CreateDate <= _endDate)
                                               join st in dbManager.GetDomainTable<TaskStatus>() on t.Id equals st.TaskId 
                                               where t.CurrentState == st.State
                                               select new TaskRegistr.TaskRegistrStat
                                                   {
                                                       StatusName = _dictionaryManager.GetEnumDictionary<TaskStatusType>()[(int)st.State], 
                                                       TaskId = t.Id,
                                                       Stamp = st.Stamp
                                                   }).OrderBy(t => t.Stamp).ToList(),
                        Username = _username,
                        StartDate = _startDate,
                        EndDate = _endDate
                    };
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при формировании данных отчёта по оказанным услугам", ex);
            }
        }
    }
}
