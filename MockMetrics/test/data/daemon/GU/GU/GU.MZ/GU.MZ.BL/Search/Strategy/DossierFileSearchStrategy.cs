using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;

using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.BL.Search.Strategy
{
    /// <summary>
    /// Стратегия поиска томов лицензионных дел
    /// </summary>
    public class DossierFileSearchStrategy : AbstractSearchStrategy<DossierFile>
    {
        /// <summary>
        /// Стратегия поиска томов лицензионных дел
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        /// <param name="dataMapper">Маппер томов</param>
        public DossierFileSearchStrategy(IDomainContext domainContext, IDomainDataMapper<DossierFile> dataMapper)
            : base(domainContext, dataMapper)
        {
            DomainJoinDictionary[typeof(Task)] = (query, innerQuery) => from t1 in query
                                                                        join Task t2 in innerQuery on t1.TaskId equals t2.Id
                                                                        select t1;

            DomainJoinDictionary[typeof(LicenseDossier)] = (query, innerQuery) => from t1 in query
                                                                                  join LicenseDossier t2 in innerQuery on t1.LicenseDossierId equals t2.Id
                                                                                  select t1;

            DomainJoinDictionary[typeof(LicenseHolder)] = (query, innerQuery) => from t1 in query
                                                                                 join LicenseHolder t2 in innerQuery on t1.LicenseDossier.LicenseHolderId equals t2.Id
                                                                                 select t1;

            DomainJoinDictionary[typeof(HolderRequisites)] = (query, innerQuery) => from t1 in query
                                                                                    join HolderRequisites t2 in innerQuery on t1.HolderRequisitesId equals t2.Id
                                                                                    select t1;

            DomainJoinDictionary[typeof(JurRequisites)] = (query, innerQuery) => from file in query
                                                                                 join JurRequisites jurReq in innerQuery on file.HolderRequisites.JurRequisitesId equals jurReq.Id
                                                                                 select file;

            DomainJoinDictionary[typeof(IndRequisites)] = (query, innerQuery) => from file in query
                                                                                 join IndRequisites jurReq in innerQuery on file.HolderRequisites.IndRequisitesId equals jurReq.Id
                                                                                 select file;

        }
    }
}
