using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;

using GU.Archive.DataModel;

namespace GU.Archive.BL.Search.Strategy
{
    public class OrganizationSearchStrategy : AbstractSearchStrategy<Organization>
    {
        public OrganizationSearchStrategy(IDomainContext domainContext, IDomainDataMapper<Organization> dataMapper)
            : base(domainContext, dataMapper)
        {
        }
    }
}
