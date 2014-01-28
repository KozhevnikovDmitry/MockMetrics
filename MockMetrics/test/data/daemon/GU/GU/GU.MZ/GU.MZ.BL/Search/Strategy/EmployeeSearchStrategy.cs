using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;

using GU.DataModel;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.Search.Strategy
{
    /// <summary>
    /// Класс стратегия поиска объектов Сотрудник.
    /// </summary>
    public class EmployeeSearchStrategy : AbstractSearchStrategy<Employee>
    {
        /// <summary>
        /// Класс стратегия поиска объектов Сотрудник.
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        /// <param name="dataMapper">Маппер сотрудников</param>
        public EmployeeSearchStrategy(IDomainContext domainContext, IDomainDataMapper<Employee> dataMapper)
            : base(domainContext, dataMapper)
        {
            DomainJoinDictionary[typeof(DbUser)] = (query, innerQuery) => from t1 in query
                                                                          join DbUser t2 in innerQuery on t1.DbUserId equals t2.Id
                                                                          select t1;
        }
    }
}
