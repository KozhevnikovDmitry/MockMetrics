using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.Search.Strategy
{
    /// <summary>
    /// Класс стратегия поиска объектов Лицензия.
    /// </summary>
    public class LicenseSearchStrategy : AbstractSearchStrategy<License>
    {
        /// <summary>
        /// Класс стратегия поиска объектов Лицензия.
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        /// <param name="dataMapper">Маппер лицензий</param>
        public LicenseSearchStrategy(IDomainContext domainContext, IDomainDataMapper<License> dataMapper)
            : base(domainContext, dataMapper)
        {
            DomainJoinDictionary[typeof(LicenseRequisites)] = (query, innerQuery) => from t1 in query
                                                                                     join LicenseRequisites t2 in innerQuery on t1.Id equals t2.LicenseId
                                                                                     select t1;
        }
    }
}
