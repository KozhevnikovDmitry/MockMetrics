using PostGrad.Core.BL;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Person;

namespace PostGrad.BL.StepByStep.After
{
    public class DossierFileBuildingFacade : IDossierFileBuilder
    {
        public DossierFileBuildingFacade(BuildingFileSteps buildingFileSteps, ICacheRepository cacheRepository, ITaskStatusPolicy taskStatusPolicy, ITaskParser taskParser)
        {
            _buildingFileSteps = buildingFileSteps;
            _cacheRepository = cacheRepository;
            _taskStatusPolicy = taskStatusPolicy;
            _taskParser = taskParser;
        }

        #region Dependencies

        private readonly BuildingFileSteps _buildingFileSteps;

        private readonly ICacheRepository _cacheRepository;

        private readonly ITaskStatusPolicy _taskStatusPolicy;

        private readonly ITaskParser _taskParser;

        #endregion


        #region Fluent 
        
        public virtual IDossierFileBuilder FromTask(Task task)
        {
            _buildingFileSteps.Task = task;
            return this;
        }

        public virtual IDossierFileBuilder WithInventoryRegNumber(int? inventoryRegNumber)
        {
            _buildingFileSteps.InventoryRegNumber = inventoryRegNumber;
            return this;
        }

        public virtual IDossierFileBuilder ToEmployee(Employee responsibleEmployee)
        {
            _buildingFileSteps.ResponsibleEmployee = responsibleEmployee;
            return this;
        }

        public virtual IDossierFileBuilder WithAcceptedStatus(string notice)
        {
            _buildingFileSteps.TaskStatus = TaskStatusType.Accepted;
            _buildingFileSteps.StatusNotice = notice;
            return this;
        }

        public virtual IDossierFileBuilder AddProvidedDocument(string documentName, int quantity)
        {
            var doc = ProvidedDocument.CreateInstance();
            doc.Name = documentName.Trim();
            doc.Quantity = quantity;

            if (string.IsNullOrEmpty(doc.Name))
            {
                throw new CantAddProvidedDocumentWithEmptyNameException();
            }

            if (doc.Quantity <= 0)
            {
                throw new CantAddProvidedDocumentWithNegativeQuantityException();
            }

            _buildingFileSteps.ProvidedDocumentList.Add(doc);

            return this;
        }

        public virtual IDossierFileBuilder WithRejectedStatus(string notice)
        {
            _buildingFileSteps.TaskStatus = TaskStatusType.Rejected;
            _buildingFileSteps.StatusNotice = notice;
            return this;
        }
        
        #endregion


        #region Building

        public DossierFile Build()
        {
            _buildingFileSteps.CheckDataCompleteness();

            var dossierFile = _buildingFileSteps.PrepareDossierFile();

            _buildingFileSteps.SetupStatus(dossierFile, _taskStatusPolicy);
            _buildingFileSteps.SetupActivity(dossierFile, _cacheRepository);
            _buildingFileSteps.SetupInventory(dossierFile, _taskParser);
            _buildingFileSteps.SetupEmployee(dossierFile);
            _buildingFileSteps.SetupScenario(dossierFile, _cacheRepository);
            _buildingFileSteps.SetupFirstSteps(dossierFile);

            return dossierFile;
        }

        #endregion
    }
}
