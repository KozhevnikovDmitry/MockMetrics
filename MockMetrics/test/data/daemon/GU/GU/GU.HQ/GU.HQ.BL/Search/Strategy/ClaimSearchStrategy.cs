
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using GU.HQ.DataModel;

namespace GU.HQ.BL.Search.Strategy
{
    public class ClaimSearchStrategy : AbstractSearchStrategy<Claim>
    {
        public ClaimSearchStrategy(IDomainContext domainContext, IDomainDataMapper<Claim> dataMapper)
            : base(domainContext, dataMapper)
        {
            _defaultOrder = claim => claim.Id;
            _defaultOrderDirection = OrderDirection.Descending;

        }
    }
}
