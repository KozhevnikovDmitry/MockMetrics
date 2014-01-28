using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;

using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.BL.Search.Strategy
{
    /// <summary>
    /// Класс стратегия поиска Лицензиатов
    /// </summary>
    public class LicenseHolderSearchStrategy : AbstractSearchStrategy<LicenseHolder>
    {
        /// <summary>
        /// Класс стратегия поиска Лицензиатов
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        /// <param name="dataMapper">Маппер лицензиатов</param>
        public LicenseHolderSearchStrategy(IDomainContext domainContext, IDomainDataMapper<LicenseHolder> dataMapper)
            : base(domainContext, dataMapper)
        {
            DomainJoinDictionary[typeof(HolderRequisites)] = (query, innerQuery) => from t1 in query
                                                                                    join HolderRequisites t2 in innerQuery on t1.Id equals t2.LicenseHolderId
                                                                                    select t1;

            DomainJoinDictionary[typeof(JurRequisites)] = (query, innerQuery) => from t1 in query
                                                                                 join HolderRequisites t2 in innerQuery on t1.Id equals t2.LicenseHolderId
                                                                                 join JurRequisites t3 in innerQuery on t2.JurRequisitesId equals t3.Id
                                                                                 select t1;

            DomainJoinDictionary[typeof(IndRequisites)] = (query, innerQuery) => from t1 in query
                                                                                 join HolderRequisites t2 in innerQuery on t1.Id equals t2.LicenseHolderId
                                                                                 join IndRequisites t3 in innerQuery on t2.IndRequisitesId equals t3.Id
                                                                                 select t1;
        }
    }
}
