using System;
using System.Collections.Generic;
using System.Linq;
using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.InitializedObject.Before
{
    public class CacheRepository : ICacheRepository
    {
        public CacheRepository(IDomainDbManager dbManager)
        {
            MergedRepositories = new List<ICacheRepository>();
            Caches = new Dictionary<Type, List<IDomainObject>>();

            Caches[typeof(LicenseStatus)] = GetDomainList<LicenseStatus>(dbManager);
            Caches[typeof(LicensedActivity)] = GetDomainList<LicensedActivity>(dbManager);
            Caches[typeof(ServiceResult)] = GetDomainList<ServiceResult>(dbManager);
        }

        public virtual List<ICacheRepository> MergedRepositories { get; protected set; }

        public virtual Dictionary<Type, List<IDomainObject>> Caches { get; protected set; }
        
        public T GetCachedItem<T>(object id) where T : IDomainObject
        {
            try
            {
                return GetCache<T>().Single(x => x.GetKeyValue() == id.ToString());
            }
            catch (InvalidOperationException)
            {
                throw new DictionaryItemNotFoundException(typeof(T));
            }
        }

        public virtual List<T> GetCache<T>() where T : IDomainObject
        {
            if (Caches.ContainsKey(typeof(T)))
            {
                return Caches[typeof(T)].Cast<T>().ToList();
            }
            
            foreach (var dictionaryManager in this.MergedRepositories)
            {
                try
                {
                    return dictionaryManager.GetCache<T>();
                }
                catch (DictionaryNotFoundException)
                {
                }
            }

            throw new DictionaryNotFoundException(typeof(T));
        }

        public void Merge(ICacheRepository cacheRepository)
        {
            MergedRepositories.Add(cacheRepository);
        }

        private List<IDomainObject> GetDomainList<T>(IDomainDbManager dbManager) where T : class, IDomainObject
        {
            return dbManager.GetDomainTable<T>().ToList<IDomainObject>();
        }
    }
}
