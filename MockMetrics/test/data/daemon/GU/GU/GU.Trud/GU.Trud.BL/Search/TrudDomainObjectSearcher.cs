using System.Linq;
using Common.BL.Search;
using Common.DA.Interface;
using GU.Trud.DataModel;

namespace GU.Trud.BL.Search
{
    public class TrudDomainObjectSearcher : AbstractDomainObjectSearcher
    {
        public TrudDomainObjectSearcher()
        {
            _searchActions["TaskExport"] = SearchTaskExport;
        }

        #region Private

        private IQueryable<TaskExport> SearchTaskExport(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchTask = (TaskExport)searchObj;
            var tasks = from TaskExport t in manager.GetDomainTable<TaskExport>()
                        where (string.IsNullOrEmpty(searchTask.Filename) || t.Filename.ToUpper().Contains(searchTask.Filename.ToUpper()))
                              &&
                              (!searchTask.Stamp.HasValue || t.Stamp == searchTask.Stamp)
                              &&
                              (t.ExportTypeId == searchTask.ExportTypeId)
                              &&
                              (t.AgencyId == searchTask.AgencyId)
                        orderby t.Stamp descending, t.Id descending 
                        select t;
            return tasks;
        }

        #endregion
    }
}
