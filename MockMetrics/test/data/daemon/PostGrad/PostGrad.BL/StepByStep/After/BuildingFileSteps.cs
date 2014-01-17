using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using PostGrad.Core.BL;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Licensing;
using PostGrad.Core.DomainModel.Person;

namespace PostGrad.BL.StepByStep.After
{
    public interface IBuildingFileSteps
    {
        Task Task { get; set; }
        int? InventoryRegNumber { get; set; }
        Employee ResponsibleEmployee { get; set; }
        TaskStatusType? TaskStatus { get; set; }
        string StatusNotice { get; set; }
        List<ProvidedDocument> ProvidedDocumentList { get; set; }
        void CheckDataCompleteness();
        DossierFile PrepareDossierFile();
        void SetupStatus(DossierFile dossierFile, ITaskStatusPolicy taskStatusPolicy);
        void SetupActivity(DossierFile dossierFile, ICacheRepository cacheRepository);
        void SetupInventory(DossierFile dossierFile, ITaskParser taskParser);
        void SetupEmployee(DossierFile dossierFile);
        void SetupScenario(DossierFile dossierFile, ICacheRepository cacheRepository);
        void SetupFirstSteps(DossierFile dossierFile);
    }

    public class BuildingFileSteps : IBuildingFileSteps
    {
        public BuildingFileSteps()
        {
            ProvidedDocumentList = new List<ProvidedDocument>();
        }

        #region Data

        public Task Task { get; set; }

        public int? InventoryRegNumber { get; set; }

        public Employee ResponsibleEmployee { get; set; }

        public TaskStatusType? TaskStatus { get; set; }

        public string StatusNotice { get; set; }

        public List<ProvidedDocument> ProvidedDocumentList { get; set; }

        #endregion

        #region Steps

        public void CheckDataCompleteness()
        {
            if (!IsDataComplete())
            {
                throw new BuildingDataNotCompleteException();
            }
        }

        private bool IsDataComplete()
        {
            if (Task == null)
            {
                return false;
            }

            if (!TaskStatus.HasValue)
            {
                return false;
            }

            if (TaskStatus.Value == TaskStatusType.Accepted)
            {
                return InventoryRegNumber.HasValue && ResponsibleEmployee != null;
            }
            else
            {
                return !string.IsNullOrEmpty(StatusNotice);
            }
        }

        public DossierFile PrepareDossierFile()
        {
            var dossierFile = DossierFile.CreateInstance();
            dossierFile.CreateDate = DateTime.Today;
            dossierFile.Task = Task;
            dossierFile.TaskId = dossierFile.Task.Id;
            return dossierFile;
        }

        public void SetupStatus(DossierFile dossierFile, ITaskStatusPolicy taskStatusPolicy)
        {
            if (taskStatusPolicy.CanSetStatus(TaskStatus.Value, dossierFile.Task))
            {
                dossierFile.Task = taskStatusPolicy.SetStatus(TaskStatus.Value, StatusNotice, dossierFile.Task);
            }
            else
            {
                throw new CantSetStatusException(TaskStatus.Value);
            }

            dossierFile.CurrentStatus = DossierFileStatus.Unbounded;
        }

        public void SetupActivity(DossierFile dossierFile, ICacheRepository cacheRepository)
        {
            var activity =
                cacheRepository.GetCache<LicensedActivity>()
                    .Single(t => t.ServiceGroupId == dossierFile.ServiceGroupId);
            dossierFile.LicensedActivity = activity;
        }

        public void SetupInventory(DossierFile dossierFile, ITaskParser taskParser)
        {
            dossierFile.DocumentInventory = DocumentInventory.CreateInstance();
            dossierFile.DocumentInventory.Stamp = DateTime.Today;
            dossierFile.DocumentInventory.RegNumber = InventoryRegNumber.Value;
            dossierFile.DocumentInventory.LicenseHolder = taskParser.ParseHolderInfo(Task).FullName;
            dossierFile.DocumentInventory.EmployeeName = ResponsibleEmployee.Name;
            dossierFile.DocumentInventory.EmployeePosition = ResponsibleEmployee.Position;
            dossierFile.DocumentInventory.LicensedActivity = dossierFile.LicensedActivity.Name;

            dossierFile.DocumentInventory.ProvidedDocumentList
                = new EditableList<ProvidedDocument>(ProvidedDocumentList);
        }

        public void SetupEmployee(DossierFile dossierFile)
        {
            dossierFile.Employee = ResponsibleEmployee;
            dossierFile.EmployeeId = ResponsibleEmployee.Id;
        }

        public void SetupScenario(DossierFile dossierFile, ICacheRepository cacheRepository)
        {
            dossierFile.Scenario = GetScenario(cacheRepository);
            dossierFile.ScenarioId = dossierFile.Scenario.Id;
            dossierFile.CurrentScenarioStepId =
                dossierFile.Scenario.ScenarioStepList.OrderBy(t => t.SortOrder).ToList()[1].Id;
        }

        private Scenario GetScenario(ICacheRepository cacheRepository)
        {
            var scenario = cacheRepository.GetCache<Scenario>().SingleOrDefault(s => s.ServiceId == Task.ServiceId);

            if (scenario == null ||
                scenario.ScenarioStepList == null ||
                scenario.ScenarioStepList.Count < 2)
            {
                throw new WrongScenarioSteplistException();
            }

            return scenario;
        }

        public void SetupFirstSteps(DossierFile dossierFile)
        {
            dossierFile.DossierFileStepList = new EditableList<DossierFileScenarioStep>();
            for (int i = 0; i < 2; i++)
            {
                var step = DossierFileScenarioStep.CreateInstance();
                step.ScenarioStepId = dossierFile.Scenario.ScenarioStepList.OrderBy(t => t.SortOrder).ToList()[i].Id;
                step.EmployeeId = dossierFile.EmployeeId;
                step.StartDate = DateTime.Now;
                dossierFile.DossierFileStepList.Add(step);
            }
        }

        #endregion
    }
}