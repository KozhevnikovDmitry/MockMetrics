using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;

using GU.DataModel;

namespace GU.BL.Search.Strategy
{
    public class TaskSearchStrategy : AbstractSearchStrategy<Task>
    {
        public TaskSearchStrategy(IDomainContext domainContext, IDomainDataMapper<Task> dataMapper)
            : base(domainContext, dataMapper)
        {
            _defaultSearchFilter =
                task => task.CurrentState != TaskStatusType.Done && task.CurrentState != TaskStatusType.Rejected;

            _defaultOrder = task => task.CreateDate;
            _defaultOrderDirection = OrderDirection.Descending;
        }
    }
}
