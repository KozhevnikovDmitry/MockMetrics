using System;
using System.Linq;
using Common.BL.Search;
using Common.DA.Interface;
using GU.HQ.DataModel;

namespace GU.HQ.BL.Search
{
    public class HqDomainObjectSearcher : AbstractDomainObjectSearcher
    {
        public HqDomainObjectSearcher()
        {
            _searchActions["SearchClaim"] = SearchClaim;
        }

        /// <summary>
        /// Поиск заявки
        /// </summary>
        /// <param name="searchObj"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        private IQueryable<Claim> SearchClaim(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchClaim = (Claim)searchObj;
            var claims = from Claim p in manager.GetDomainTable<Claim>()
                         where ((searchClaim.ClaimDate == null && p.ClaimDate != null) || (searchClaim.ClaimDate != null && p.ClaimDate.Value.Date == searchClaim.ClaimDate))
                         &&
                         (string.IsNullOrEmpty(searchClaim.Declarer.Sname) || p.Declarer.Sname.ToUpper().Contains(searchClaim.Declarer.Sname.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchClaim.Declarer.Name) || p.Declarer.Name.ToUpper().Contains(searchClaim.Declarer.Name.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchClaim.Declarer.Patronymic) || p.Declarer.Patronymic.ToUpper().Contains(searchClaim.Declarer.Patronymic.ToUpper()))
                         &&
                         ( searchClaim.CurrentStatusTypeId == 0 || p.CurrentStatusTypeId == searchClaim.CurrentStatusTypeId)
                          &&
                         (searchClaim.AgencyId == 0 || p.AgencyId == searchClaim.AgencyId)
                         orderby p.ClaimDate
                         select p;
            return claims;
        }
    }
}
