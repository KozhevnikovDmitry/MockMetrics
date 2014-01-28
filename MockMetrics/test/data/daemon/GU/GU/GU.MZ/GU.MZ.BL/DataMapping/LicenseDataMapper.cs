using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер сущностей Лицензия
    /// </summary>
    public class LicenseDataMapper : AbstractDataMapper<License>
    {
        /// <summary>
        /// Маппер объектов с номенклатурой
        /// </summary>
        private readonly IDomainDataMapper<LicenseObject> _liсenseObjectMapper;

        private readonly IDomainDataMapper<LicenseRequisites> _requisitesMapper;

        /// <summary>
        /// Маппер лицензионных дел
        /// </summary>
        public IDomainDataMapper<LicenseDossier> LiсenseDossierMapper { get; set; }

        public LicenseDataMapper(IDomainDataMapper<LicenseObject> liсenseObjectMapper, IDomainDataMapper<LicenseRequisites> requisitesMapper, IDomainContext domainContext)
            : base(domainContext)
        {
            _liсenseObjectMapper = liсenseObjectMapper;
            _requisitesMapper = requisitesMapper;
        }

        /// <summary>
        /// Выгружает экземпляр Лицензии из базы данных
        /// </summary>
        /// <param name="id">Id лицензии</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Объект Лицензия</returns>
        protected override License RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var license = dbManager.RetrieveDomainObject<License>(id);

            license.LicenseDossier = dbManager.RetrieveDomainObject<LicenseDossier>(license.LicenseDossierId);

            LiсenseDossierMapper.FillAssociations(license.LicenseDossier, dbManager);

            var objectIds = dbManager.GetDomainTable<LicenseObject>()
                                   .Where(d => d.LicenseId == license.Id)
                                   .Select(d => d.Id)
                                   .ToList();

            license.LicenseObjectList = new EditableList<LicenseObject>(objectIds.Select(t => _liсenseObjectMapper.Retrieve(t, dbManager)).ToList());

            foreach (var licenseObject in license.LicenseObjectList)
            {
                licenseObject.License = license;
            }

            var requisistesIds = dbManager.GetDomainTable<LicenseRequisites>()
                                          .Where(d => d.LicenseId == license.Id)
                                          .Select(d => d.Id)
                                          .ToList();

            license.LicenseRequisitesList = new EditableList<LicenseRequisites>(requisistesIds.Select(t => _requisitesMapper.Retrieve(t, dbManager)).ToList());


            license.LicenseStatusList = new EditableList<LicenseStatus>(
                dbManager.GetDomainTable<LicenseStatus>()
                         .Where(d => d.LicenseId == license.Id)
                         .Select(d => d.Id)
                         .ToList().Select(t => dbManager.RetrieveDomainObject<LicenseStatus>(t)).OrderBy(t => t.Stamp).ToList());

            return license;
        }

        /// <summary>
        /// Сохраняет объект Лицензию в базу данных
        /// </summary>
        /// <param name="obj">Объект Лицензия</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        /// <returns>Сохранённый объект Лицензия</returns>
        protected override License SaveOperation(License obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            dbManager.SaveDomainObject(tmp);

            if (tmp.LicenseObjectList != null)
            {
                if (tmp.LicenseObjectList.DelItems != null)
                {
                    foreach (var delItem in tmp.LicenseObjectList.DelItems.Cast<LicenseObject>())
                    {
                        _liсenseObjectMapper.Delete(delItem, dbManager);
                    }
                }

                for (int i = 0; i < tmp.LicenseObjectList.Count; i++)
                {
                    tmp.LicenseObjectList[i].LicenseId = tmp.Id;
                    tmp.LicenseObjectList[i] = _liсenseObjectMapper.Save(tmp.LicenseObjectList[i], dbManager);
                }
            }

            if (tmp.LicenseRequisitesList != null)
            {
                if (tmp.LicenseRequisitesList.DelItems != null)
                {
                    foreach (var delItem in tmp.LicenseRequisitesList.DelItems.Cast<LicenseRequisites>())
                    {
                        _requisitesMapper.Delete(delItem, dbManager);
                    }
                }

                for (int i = 0; i < tmp.LicenseRequisitesList.Count; i++)
                {
                    tmp.LicenseRequisitesList[i].LicenseId = tmp.Id;
                    tmp.LicenseRequisitesList[i] = _requisitesMapper.Save(tmp.LicenseRequisitesList[i], dbManager);
                }
            }

            if (tmp.LicenseStatusList != null)
            {
                if (tmp.LicenseStatusList.DelItems != null)
                {
                    foreach (var delItem in tmp.LicenseStatusList.DelItems.Cast<LicenseStatus>())
                    {
                        delItem.MarkDeleted();
                        dbManager.SaveDomainObject(delItem);
                    }
                }

                foreach (LicenseStatus t in tmp.LicenseStatusList)
                {
                    t.LicenseId = tmp.Id;
                    dbManager.SaveDomainObject(t);
                }
            }

            return tmp;
        }

        /// <summary>
        /// Заполняет отображаемые ассоциации объекта Лицензии.
        /// </summary>
        /// <param name="obj">Объект Лицензия</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        protected override void FillAssociationsOperation(License obj, IDomainDbManager dbManager)
        {
            var actualRequisistesId =
                dbManager.GetDomainTable<LicenseRequisites>()
                         .Where(t => t.LicenseId == obj.Id)
                         .OrderByDescending(t => t.CreateDate)
                         .Select(t => t.Id)
                         .First();

            var actualRequisites = _requisitesMapper.Retrieve(actualRequisistesId, dbManager);

            obj.LicenseRequisitesList.Add(actualRequisites);
        }
    }
}
