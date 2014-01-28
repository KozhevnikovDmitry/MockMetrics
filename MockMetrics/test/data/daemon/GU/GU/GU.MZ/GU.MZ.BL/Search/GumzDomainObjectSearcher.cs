using System.Linq;
using Common.BL.Search;
using Common.DA.Interface;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.Search
{
    /// <summary>
    /// Класс, предназначенный для поиска объектов <c>IDomainObject</c> по обрaзцам.
    /// </summary>
    public class GumzDomainObjectSearcher : AbstractDomainObjectSearcher
    {
        /// <summary>
        /// Класс, предназначенный для поиска объектов <c>IDomainObject</c> по обрaзцам.
        /// </summary>
        public GumzDomainObjectSearcher()
        {
            _searchActions[typeof(Address).Name] = SearchAddress;
            _searchActions[typeof(Employee).Name] = SearchEmployee;
            _searchActions[typeof(License).Name] = SearchLicense;
            _searchActions[typeof(DossierFile).Name] = SearchDossierFile;
            _searchActions[typeof(LicenseDossier).Name] = SearchLicenseDossier;
            _searchActions[typeof(LicenseHolder).Name] = SearchLicenseHolder;
        }

        #region Private

        /// <summary>
        /// Возвращает запрос на получение сущностей <c>Address</c>, частично совпадающих с объектом <c>searchAddress</c>.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Запрос на получение сущностей <c>Address</c></returns>
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
        /// Возвращает запрос на получение сущностей <c>Employee</c>, частично совпадающих с объектом searchEmployee.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Список сущностей <c>Employee</c></returns>
        private IQueryable<Employee> SearchEmployee(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchEmployee = (Employee)searchObj;
            var r = from Employee e in manager.GetDomainTable<Employee>()
                    where (string.IsNullOrEmpty(searchEmployee.Name) || e.Name.ToUpper().Contains(searchEmployee.Name.ToUpper()))
                          && 
                          (string.IsNullOrEmpty(searchEmployee.Phone) || e.Phone.ToUpper().Contains(searchEmployee.Phone.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchEmployee.Position) || e.Position.ToUpper().Contains(searchEmployee.Position.ToUpper()))
                          &&
                          (string.IsNullOrEmpty(searchEmployee.Email) || e.Email.ToUpper().Contains(searchEmployee.Email.ToUpper()))
                    orderby e.Id
                    select e;
            return r;
        }

        /// <summary>
        /// Возвращает запрос на получение сущностей <c>License</c>, частично совпадающих с объектом searchLicense.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Запрос на получение сущностей <c>License</c></returns>
        private IQueryable<License> SearchLicense(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchLicense = (License)searchObj;
            var lic = from License l in manager.GetDomainTable<License>()
                      where (l.LicensedActivityId == searchLicense.LicensedActivityId)
                            &&
                            (l.CurrentStatus == searchLicense.CurrentStatus)
                      orderby l.GrantDate
                      select l;
            return lic;
        }

        /// <summary>
        /// Возвращает запрос на получение сущностей <c>License</c>, частично совпадающих с объектом searchLicense.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Запрос на получение сущностей <c>License</c></returns>
        private IQueryable<DossierFile> SearchDossierFile(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchDossierFile = (DossierFile)searchObj;
            var lic = (from DossierFile l in manager.GetDomainTable<DossierFile>()
                       select l).OrderBy(t => t.CurrentStatus).ThenBy(t => t.CreateDate);
            return lic;
        }

        /// <summary>
        /// Возвращает запрос на получение сущностей <c>LicenseHolder</c>, частично совпадающих с объектом <c>searchObj</c>.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Запрос на получение <c>LicenseHolder</c></returns>
        private IQueryable<IDomainObject> SearchLicenseHolder(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchHolder = (LicenseHolder)searchObj;
            var r = from LicenseHolder e in manager.GetDomainTable<LicenseHolder>()
                    select e;
            return r;
        }

        /// <summary>
        /// Возвращает запрос на получение сущностей <c>LicenseDossier</c>, частично совпадающих с объектом <c>searchObj</c>.
        /// </summary>
        /// <param name="searchObj">Образец для поиска</param>
        /// <param name="manager">Менеждер доступа к данным</param>
        /// <returns>Запрос на получение <c>LicenseDossier</c></returns>
        private IQueryable<IDomainObject> SearchLicenseDossier(IDomainObject searchObj, IDomainDbManager manager)
        {
            var searchDossier = (LicenseDossier)searchObj;
            var r = from LicenseDossier e in manager.GetDomainTable<LicenseDossier>()
                    select e;
            return r;
        }

        #endregion
    }
}
