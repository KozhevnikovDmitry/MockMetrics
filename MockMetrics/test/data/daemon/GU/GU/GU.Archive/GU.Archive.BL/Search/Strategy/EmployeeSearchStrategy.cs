using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;

using GU.Archive.DataModel;

namespace GU.Archive.BL.Search.Strategy
{
    public class EmployeeSearchStrategy : AbstractSearchStrategy<Employee>
    {
        public EmployeeSearchStrategy(IDomainContext domainContext, IDomainDataMapper<Employee> dataMapper)
            : base(domainContext, dataMapper)
        {
        }
    }
}
