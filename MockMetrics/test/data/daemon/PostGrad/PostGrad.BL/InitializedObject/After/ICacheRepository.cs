using System.Collections.Generic;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel;

namespace PostGrad.BL.InitializedObject.After
{
    public interface ICacheRepository
    {
        T GetCachedItem<T>(object id) where T : IDomainObject;

        List<T> GetCache<T>() where T : IDomainObject;

        void Merge(ICacheRepository cacheRepository);

        void Initialize(IDomainDbManager dbManager);
    }
}
