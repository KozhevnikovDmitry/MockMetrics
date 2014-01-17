using System.Collections.Generic;
using PostGrad.Core.DomainModel;

namespace PostGrad.Core.BL
{
    public interface ICacheRepository
    {
        T GetCachedItem<T>(object id) where T : IDomainObject;

        List<T> GetCache<T>() where T : IDomainObject;
    }
}
