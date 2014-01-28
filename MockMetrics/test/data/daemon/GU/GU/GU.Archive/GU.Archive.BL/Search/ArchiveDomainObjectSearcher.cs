using System.Linq;
using Common.BL.Search;
using Common.DA.Interface;
using GU.Archive.DataModel;

namespace GU.Archive.BL.Search
{
    /// <summary>
    /// Класс, предназначенный для поиска объектов <c>IDomainObject</c> по обрaзцам.
    /// </summary>
    public class ArchiveDomainObjectSearcher : AbstractDomainObjectSearcher
    {
        public ArchiveDomainObjectSearcher()
        {
            _searchActions["Address"] = SearchAddress;
            _searchActions["Employee"] = SearchEmployee;
            _searchActions["Organization"] = SearchOrganization;
            _searchActions["Post"] = SearchPost;
        }

        #region Private
        
        /// <summary>
        /// Возвращает список сущностей <c>Address</c>, частично совпадающих с объектом <c>searchAddress</c>.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Cписок сущностей <c>Address</c></returns>
        private IQueryable<Address> SearchAddress(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchAddress = (Address)searchObj;
            return from Address p in manager.GetDomainTable<Address>()
                   where (string.IsNullOrEmpty(searchAddress.Country) || p.Country.ToUpper().Contains(searchAddress.Country.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchAddress.CountryRegion) || p.CountryRegion.ToUpper().Contains(searchAddress.CountryRegion.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchAddress.Area) || p.Area.ToUpper().Contains(searchAddress.Area.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchAddress.City) || p.City.ToUpper().Contains(searchAddress.City.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchAddress.Zip) || p.Zip.ToUpper().Contains(searchAddress.Zip.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchAddress.Street) || p.Street.ToUpper().Contains(searchAddress.Street.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchAddress.House) || p.House.ToUpper().Contains(searchAddress.House.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchAddress.Build) || p.Build.ToUpper().Contains(searchAddress.Build.ToUpper()))
                         &&
                         (string.IsNullOrEmpty(searchAddress.Flat) || p.Flat.ToUpper().Contains(searchAddress.Flat.ToUpper()))
                   select p;
        }

        /// <summary>
        /// Возвращает список сущностей <c>Employee</c>, частично совпадающих с объектом searchEmployee.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Список сущностей <c>Employee</c></returns>
        private IQueryable<Employee> SearchEmployee(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchEmployee = (Employee)searchObj;
            var r = from Employee e in manager.GetDomainTable<Employee>()
                    where (string.IsNullOrEmpty(searchEmployee.Phone) || e.Phone.ToUpper().Contains(searchEmployee.Phone.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchEmployee.Position) || e.Position.ToUpper().Contains(searchEmployee.Position.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchEmployee.Email) || e.Email.ToUpper().Contains(searchEmployee.Email.ToUpper()))
                    orderby e.Id
                    select e;
            return r;
        }

        /// <summary>
        /// Возвращает список сущностей <c>Organization</c>, частично совпадающих с объектом searchOrg.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Список сущностей <c>Organization</c></returns>
        private IQueryable<Organization> SearchOrganization(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchOrg = (Organization)searchObj;
            var r = from Organization e in manager.GetDomainTable<Organization>()
                    where (string.IsNullOrEmpty(searchOrg.Inn) || e.Inn.ToUpper().Contains(searchOrg.Inn.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchOrg.Ogrn) || e.Ogrn.ToUpper().Contains(searchOrg.Ogrn.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchOrg.FullName) || e.FullName.ToUpper().Contains(searchOrg.FullName.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchOrg.ShortName) || e.ShortName.ToUpper().Contains(searchOrg.ShortName.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchOrg.HeadName) || e.HeadName.ToUpper().Contains(searchOrg.HeadName.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchOrg.Phone) || e.Phone.ToUpper().Contains(searchOrg.Phone.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchOrg.Fax) || e.Fax.ToUpper().Contains(searchOrg.Fax.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchOrg.Email) || e.Email.ToUpper().Contains(searchOrg.Email.ToUpper()))
                    orderby e.Id
                    select e;
            return r;
        }

        /// <summary>
        /// Возвращает список сущностей <c>Post</c>, частично совпадающих с объектом searchPost.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Список сущностей <c>Post</c></returns>
        private IQueryable<Post> SearchPost(IDomainObject searchObj, IDomainDbManager manager)
        {
            var post = (Post)searchObj;
            var r = from Post e in manager.GetDomainTable<Post>()
                    where (string.IsNullOrEmpty(post.RegistrationNum.ToString()) || e.RegistrationNum.ToString().Contains(post.RegistrationNum.ToString()))
                          &&
                          (post.DeliveryType == 0 || e.DeliveryType ==post.DeliveryType)
                          &&
                          (post.PostType == 0 || e.PostType == post.PostType)
                    orderby e.RegistrationNum
                    select e;
            return r;
        }

        #endregion
    }
}
