using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;

using GU.Trud.DataModel;

namespace GU.Trud.BL.Search.Strategy
{
    public class TaskExportSearchStrategy : AbstractSearchStrategy<TaskExport>
    {
        public TaskExportSearchStrategy(IDomainContext domainContext, IDomainDataMapper<TaskExport> dataMapper)
            : base(domainContext, dataMapper)
        {
        }
    }
}
