using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;

using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.Search.Strategy
{
    /// <summary>
    /// Класс стратегия поиска сущностей Эксперт
    /// </summary>
    public class ExpertSearchStrategy : AbstractSearchStrategy<Expert>
    {
        /// <summary>
        /// Класс стратегия поиска сущностей Эксперт
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        /// <param name="dataMapper">Маппер экспертов</param>
        public ExpertSearchStrategy(IDomainContext domainContext, IDomainDataMapper<Expert> dataMapper)
            : base(domainContext, dataMapper)
        {
            DomainJoinDictionary[typeof(IndividualExpertState)] = (query, innerQuery) => from t1 in query
                                                                                         join IndividualExpertState t2 in innerQuery on t1.Id equals t2.Id
                                                                                         select t1;

            DomainJoinDictionary[typeof(JuridicalExpertState)] = (query, innerQuery) => from t1 in query
                                                                                        join JuridicalExpertState t2 in innerQuery on t1.Id equals t2.Id
                                                                                        select t1;
        }
    }
}
