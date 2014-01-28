using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;

using GU.Archive.DataModel;

namespace GU.Archive.BL.Search.Strategy
{
    public class PostSearchStrategy : AbstractSearchStrategy<Post>
    {
        public PostSearchStrategy(IDomainContext domainContext, IDomainDataMapper<Post> dataMapper)
            : base(domainContext, dataMapper)
        {
        }
    }
}
