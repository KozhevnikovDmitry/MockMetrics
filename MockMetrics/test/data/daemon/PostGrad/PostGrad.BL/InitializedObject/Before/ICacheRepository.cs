using System.Collections.Generic;
using PostGrad.Core.DomainModel;

namespace PostGrad.BL.InitializedObject.Before
{
    public interface ICacheRepository
    {
        T GetCachedItem<T>(object id) where T : IDomainObject;

        List<T> GetCache<T>() where T : IDomainObject;

        void Merge(ICacheRepository cacheRepository);
    }
}
