using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;
using Common.BL.Search.SearchModification;

using GU.BL.Policy;
using GU.DataModel;

using System.Linq;

namespace GU.BL.Search.Strategy
{
    /// <summary>
    /// Класс стратегия поиска сущностей Пользователь
    /// </summary>
    public class DbUserSearchStrategy : AbstractSearchStrategy<DbUser>
    {
        /// <summary>
        /// Класс стратегия поиска сущностей Пользователь
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        /// <param name="dataMapper">Маппер пользователей</param>
        public DbUserSearchStrategy(IDomainContext domainContext, IDomainDataMapper<DbUser> dataMapper)
            : base(domainContext, dataMapper)
        {
        }

        // TODO : КОСТЫЛЬ на фильтрацию пользователей по Agency, перенести в GuDbManager
        // TODO : При простом переносе в предикаты GuDbManager валится при ините, так как аутентификация пользователя происходит раньше инита BL.
        // TODO : убрать protected virtual с метода GetSearchAction, сделать снова private
        protected override SearchDelegate<DbUser> GetSearchAction(ISearchData searchData)
        {
            var query = base.GetSearchAction(searchData);
            SearchDelegate<DbUser> result =
                (s, d, c) =>
                query(s, d, c)
                    .Where(
                        t =>
                        UserPolicy.VisibleAgencyIds(GuFacade.GetDbUser()).Contains(t.Agency.Id)
                        && t.Id != GuFacade.GetDbUser().Id);
            return result;
        }
    }
}
