using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;

using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;

namespace GU.MZ.BL.Search.Strategy
{
    /// <summary>
    /// Класс стратегия поиска лицензионных дел
    /// </summary>
    public class LicenseDossierSearchStartegy : AbstractSearchStrategy<LicenseDossier>
    {
        /// <summary>
        /// Класс стратегия поиска лицензионных дел
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        /// <param name="dataMapper">Маппер дел</param>
        public LicenseDossierSearchStartegy(IDomainContext domainContext, IDomainDataMapper<LicenseDossier> dataMapper)
            : base(domainContext, dataMapper)
        {
            DomainJoinDictionary[typeof(LicenseHolder)] = (query, innerQuery) => from t1 in query
                                                                                 join LicenseHolder t2 in innerQuery on t1.LicenseHolderId equals t2.Id
                                                                                 select t1;


            DomainJoinDictionary[typeof(HolderRequisites)] = (query, innerQuery) => from t1 in query
                                                                                    join HolderRequisites t3 in innerQuery on t1.LicenseHolderId equals t3.LicenseHolderId
                                                                                    select t1;
        }
    }
}
