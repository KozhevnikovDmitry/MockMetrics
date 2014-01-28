using System.Linq;
using Common.BL.Search;
using Common.DA.Interface;
using GU.BL.Search.SearchDomain;
using GU.DataModel;

namespace GU.BL.Search
{
    public class GuDomainObjectSearcher : AbstractDomainObjectSearcher
    {
        public GuDomainObjectSearcher()
        {
            _searchActions["Task"] = SearchTask;
            _searchActions["SearchTask"] = SearchByFakeTask;
        }

        #region Private

        private IQueryable<Task> SearchTask(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchTask = (Task)searchObj;
            var tasks = from Task t in manager.GetDomainTable<Task>()
                        where (string.IsNullOrEmpty(searchTask.CustomerFio) || t.CustomerFio.ToUpper().Contains(searchTask.CustomerFio.ToUpper()))
                              &&
                              (string.IsNullOrEmpty(searchTask.CustomerPhone) || t.CustomerPhone.ToUpper().Contains(searchTask.CustomerPhone.ToUpper()))
                              &&
                              (string.IsNullOrEmpty(searchTask.CustomerEmail) || t.CustomerEmail.ToUpper().Contains(searchTask.CustomerEmail.ToUpper()))
                              &&
                              (!searchTask.CreateDate.HasValue || t.CreateDate == searchTask.CreateDate)
                              &&
                              (!searchTask.DueDate.HasValue || t.DueDate == searchTask.DueDate)
                              &&
                              (t.ServiceId == searchTask.ServiceId)
                              &&
                              (t.CurrentState == searchTask.CurrentState)
                              &&
                              (searchTask.Id == 0 || searchTask.Id == t.Id) 
                        orderby t.CreateDate
                        select t;
            return tasks;
        }

        private IQueryable<Task> SearchByFakeTask(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchTask = (SearchTask)searchObj;
            var tasks = from Task t in manager.GetDomainTable<Task>()
                        where (string.IsNullOrEmpty(searchTask.CustomerFio) || t.CustomerFio.ToUpper().Contains(searchTask.CustomerFio.ToUpper()))
                              &&
                              (string.IsNullOrEmpty(searchTask.CustomerPhone) || t.CustomerPhone.ToUpper().Contains(searchTask.CustomerPhone.ToUpper()))
                              &&
                              (string.IsNullOrEmpty(searchTask.CustomerEmail) || t.CustomerEmail.ToUpper().Contains(searchTask.CustomerEmail.ToUpper()))
                              &&
                              (!searchTask.CreateDate.HasValue || t.CreateDate == searchTask.CreateDate)
                              &&
                              (!searchTask.DueDate.HasValue || t.DueDate == searchTask.DueDate)
                              &&
                              (!searchTask.FakeServiceId.HasValue || t.ServiceId == searchTask.FakeServiceId)
                              &&
                              (!searchTask.FakeCurrentState.HasValue || (int?)t.CurrentState == searchTask.FakeCurrentState)
                              &&
                              (!searchTask.FakeId.HasValue || searchTask.FakeId == t.Id)
                        orderby t.CreateDate
                        select t;
            return tasks;
        }

        #endregion
    }
}
