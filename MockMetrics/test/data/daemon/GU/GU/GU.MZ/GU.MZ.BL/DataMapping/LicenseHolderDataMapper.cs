using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.MZ.DataModel.Holder;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер сущносей Лицензиат
    /// </summary>
    public class LicenseHolderDataMapper : AbstractDataMapper<LicenseHolder>
    {
        /// <summary>
        /// Маппер реквизитов
        /// </summary>
        private readonly IDomainDataMapper<HolderRequisites> _requisitesMapper;

        /// <summary>
        /// Класс маппер сущносей Лицензиат
        /// </summary>
        /// <param name="_requisitesMapper">Маппер реквизитов</param>
        /// <param name="domainContext">Доменный контекст</param>
        public LicenseHolderDataMapper(IDomainDataMapper<HolderRequisites> _requisitesMapper, 
                                       IDomainContext domainContext)
            : base(domainContext)
        {
            this._requisitesMapper = _requisitesMapper;
        }

        /// <summary>
        /// Получает из базы данных сущность Лицензиат
        /// </summary>
        /// <param name="id">Id лицензиата</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Лицензиат</returns>
        protected override LicenseHolder RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var licenseHolder = dbManager.RetrieveDomainObject<LicenseHolder>(id);

            var requisitesIds = (from h in dbManager.GetDomainTable<HolderRequisites>()
                                 where h.LicenseHolderId == licenseHolder.Id
                                 select h.Id).ToList();

            licenseHolder.RequisitesList = new EditableList<HolderRequisites>();

            foreach (var requisitesId in requisitesIds)
            {
                var requisites = _requisitesMapper.Retrieve(requisitesId, dbManager);
                requisites.LicenseHolder = licenseHolder;
                licenseHolder.RequisitesList.Add(requisites);
            }

            return licenseHolder;
        }

        /// <summary>
        /// Сохраняет данные Лицензиата в базу данных
        /// </summary>
        /// <param name="obj">Лицензиат</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        /// <returns>Сохранённый лицензиат</returns>
        protected override LicenseHolder SaveOperation(LicenseHolder obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            dbManager.SaveDomainObject(tmp);

            for (int i = 0; i < tmp.RequisitesList.Count; i++)
            {
                tmp.RequisitesList[i].LicenseHolderId = tmp.Id;
                tmp.RequisitesList[i] = _requisitesMapper.Save(tmp.RequisitesList[i], dbManager);
            }

            return tmp;
        }

        protected override void FillAssociationsOperation(LicenseHolder obj, IDomainDbManager dbManager)
        {
            obj.RequisitesList = 
                new EditableList<HolderRequisites>(dbManager.GetDomainTable<HolderRequisites>()
                                                            .Where(t => t.LicenseHolderId == obj.Id)
                                                            .Select(t => t.Id)
                                                            .ToList()
                                                            .Select(t => _requisitesMapper.Retrieve(t , dbManager))
                                                            .ToList());

            foreach (var holderRequisites in obj.RequisitesList)
            {
                _requisitesMapper.FillAssociations(holderRequisites, dbManager);
                holderRequisites.LicenseHolder = obj;
            }
        }
    }
}
