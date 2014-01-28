using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Маппер сущностей Том лицензионного дела
    /// </summary>
    public class DossierFileDataMapper : AbstractDataMapper<DossierFile>
    {
        private readonly IDomainDataMapper<Task> _taskDataMapper;

        private readonly IDomainDataMapper<DossierFileScenarioStep> _fileStepDataMapper;

        private readonly IDomainDataMapper<HolderRequisites> _holderRequisitesMapper;

        private readonly IDomainDataMapper<LicenseDossier> _licenseDossierDataMapper;

        private readonly IDomainDataMapper<Employee> _employeeMapper;

        private readonly IDomainDataMapper<DocumentInventory> _inventoryMapper;
        private readonly IDomainDataMapper<License> _licenseMapper;

        private readonly IDictionaryManager _dictionaryManager;

        public DossierFileDataMapper(IDomainDataMapper<Task> taskDataMapper, 
                                     IDomainDataMapper<DossierFileScenarioStep> fileStepDataMapper, 
                                     IDomainDataMapper<HolderRequisites> holderRequisitesMapper, 
                                     IDomainDataMapper<LicenseDossier> licenseDossierDataMapper, 
                                     IDomainDataMapper<Employee> employeeMapper,
                                     IDomainDataMapper<DocumentInventory> inventoryMapper,
                                     IDomainDataMapper<License> licenseMapper, 
                                     IDictionaryManager dictionaryManager,
                                     IDomainContext domainContext)
            : base(domainContext)
        {
            _taskDataMapper = taskDataMapper;
            _fileStepDataMapper = fileStepDataMapper;
            _holderRequisitesMapper = holderRequisitesMapper;
            _licenseDossierDataMapper = licenseDossierDataMapper;
            _employeeMapper = employeeMapper;
            _inventoryMapper = inventoryMapper;
            _licenseMapper = licenseMapper;
            _dictionaryManager = dictionaryManager;
        }

        protected override DossierFile RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var dossierFile = dbManager.RetrieveDomainObject<DossierFile>(id);

            dossierFile.Task = _taskDataMapper.Retrieve(dossierFile.TaskId, dbManager);
            dossierFile.DocumentInventory = _inventoryMapper.Retrieve(dossierFile.Id, dbManager);

            LoadDossierActivityAndScenario(dossierFile, dbManager);

            if (dossierFile.LicenseId.HasValue)
            {
                dossierFile.License = _licenseMapper.Retrieve(dossierFile.LicenseId, dbManager);
            }

            if (dossierFile.DossierFileServiceResultId.HasValue)
            {
                dossierFile.DossierFileServiceResult =
                    dbManager.RetrieveDomainObject<DossierFileServiceResult>(dossierFile.DossierFileServiceResultId);
            }

            if (dossierFile.HolderRequisitesId.HasValue)
            {
                dossierFile.HolderRequisites = _holderRequisitesMapper.Retrieve(dossierFile.HolderRequisitesId, dbManager);
                dossierFile.HolderRequisites.LicenseHolder = dossierFile.LicenseDossier.LicenseHolder;
            }

            dossierFile.Employee = dbManager.RetrieveDomainObject<Employee>(dossierFile.EmployeeId);
            _employeeMapper.FillAssociations(dossierFile.Employee, dbManager);

            dossierFile.DossierFileStepList =
                new EditableList<DossierFileScenarioStep>(
                    dbManager.GetDomainTable<DossierFileScenarioStep>()
                             .Where(t => t.DossierFileId == dossierFile.Id)
                             .Select(t => t.Id)
                             .ToList()
                             .Select(t => _fileStepDataMapper.Retrieve(t, dbManager))
                             .ToList());

            foreach (var dossierFileScenarioStep in dossierFile.DossierFileStepList)
            {
                dossierFileScenarioStep.DossierFile = dossierFile;
            }

            return dossierFile;
        }

        protected override DossierFile SaveOperation(DossierFile obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            tmp.Task = _taskDataMapper.Save(tmp.Task, dbManager);
            tmp.TaskId = tmp.Task.Id;

            if (tmp.License != null)
            {
                tmp.License = _licenseMapper.Save(tmp.License, dbManager);
                tmp.LicenseId = tmp.License.Id;
            }

            if (tmp.DossierFileServiceResult != null)
            {
                dbManager.SaveDomainObject(tmp.DossierFileServiceResult);
                tmp.DossierFileServiceResultId = tmp.DossierFileServiceResult.Id;
            }

            if (tmp.HolderRequisites != null)
            {
                tmp.HolderRequisites.LicenseHolderId = tmp.LicenseDossier.LicenseHolderId;
                tmp.HolderRequisites = _holderRequisitesMapper.Save(tmp.HolderRequisites, dbManager);
                tmp.HolderRequisitesId = tmp.HolderRequisites.Id;
            }

            dbManager.SaveDomainObject(tmp);

            for (int i = 0; i < tmp.DossierFileStepList.Count; i++)
            {
                tmp.DossierFileStepList[i].DossierFileId = tmp.Id;
                tmp.DossierFileStepList[i].DossierFile = tmp;
                tmp.DossierFileStepList[i] = _fileStepDataMapper.Save(tmp.DossierFileStepList[i], dbManager);
            }

            if (obj.DeleteServiceResults != null)
            {
                foreach (var dossierFileServiceResult in obj.DeleteServiceResults)
                {
                    dossierFileServiceResult.MarkDeleted();
                    dbManager.SaveDomainObject(dossierFileServiceResult);
                }
            }

            return tmp;
        }

        protected override void FillAssociationsOperation(DossierFile obj, IDomainDbManager dbManager)
        {
            obj.Task = dbManager.RetrieveDomainObject<Task>(obj.TaskId);

            _taskDataMapper.FillAssociations(obj.Task, dbManager);

            LoadDossierActivityAndScenario(obj, dbManager);
        }

        private void LoadDossierActivityAndScenario(DossierFile obj, IDomainDbManager dbManager)
        {
            if (obj.LicenseDossierId.HasValue)
            {
                obj.LicenseDossier = dbManager.RetrieveDomainObject<LicenseDossier>(obj.LicenseDossierId);

                _licenseDossierDataMapper.FillAssociations(obj.LicenseDossier, dbManager);
            }

            obj.LicensedActivity =
                _dictionaryManager.GetDictionary<LicensedActivity>()
                                  .Single(t => t.ServiceGroupId == obj.ServiceGroupId);

            obj.Scenario = _dictionaryManager.GetDictionaryItem<Scenario>(obj.ScenarioId);
        }
    }
}
