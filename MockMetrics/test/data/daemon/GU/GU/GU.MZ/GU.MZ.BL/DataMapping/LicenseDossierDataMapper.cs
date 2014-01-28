using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Маппер сущностей Лицензионное дело.
    /// </summary>
    public class LicenseDossierDataMapper : AbstractDataMapper<LicenseDossier>
    {
        private readonly IDomainDataMapper<LicenseHolder> _holderMapper;

        public IDomainDataMapper<DossierFile> DossierFileMapper { get; set; }

        public IDomainDataMapper<License> LicenseMapper { get; set; }

        public LicenseDossierDataMapper(IDomainDataMapper<LicenseHolder> holderMapper, 
                                        IDomainContext domainContext)
            : base(domainContext)
        {
            _holderMapper = holderMapper;
        }

        protected override LicenseDossier RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var result = dbManager.RetrieveDomainObject<LicenseDossier>(id);

            FillAssociationsOperation(result, dbManager);

            var lisenseIds = dbManager.GetDomainTable<License>()
                                      .Where(l => l.LicenseDossierId == result.Id &&
                                                  l.LicensedActivityId == result.LicensedActivityId )
                                      .Select(l => l.Id)
                                      .ToList();

            result.LicenseList = new EditableList<License>(lisenseIds.Select(l => LicenseMapper.Retrieve(l, dbManager)).ToList());

            var fileIds = dbManager.GetDomainTable<DossierFile>()
                                   .Where(d => d.LicenseDossierId == result.Id)
                                   .Select(d => d.Id)
                                   .ToList();

            result.DossierFileList = new EditableList<DossierFile>(fileIds.Select(d => DossierFileMapper.Retrieve(d, dbManager)).ToList());

            return result;
        }

        protected override LicenseDossier SaveOperation(LicenseDossier obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            tmp.LicenseHolder = _holderMapper.Save(tmp.LicenseHolder, dbManager);

            tmp.LicenseHolderId = tmp.LicenseHolder.Id;

            dbManager.SaveDomainObject(tmp);

            return tmp;
        }

        protected override void FillAssociationsOperation(LicenseDossier obj, IDomainDbManager dbManager)
        {
            obj.LicenseHolder = _holderMapper.Retrieve(obj.LicenseHolderId, dbManager);
        }
    }
}
