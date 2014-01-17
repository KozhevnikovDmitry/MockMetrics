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

namespace PostGrad.BL.StepByStep.Before
{
    public class DossierFileBuilder : IDossierFileBuilder
    {
        #region Data

        private Task _task;

        private int? _inventoryRegNumber;

        private Employee _responsibleEmployee;

        private TaskStatusType? _taskStatus;

        private string _statusNotice;

        private List<ProvidedDocument> _providedDocumentList;

        #endregion

        #region Dependencies

        private readonly ICacheRepository _cacheRepository;

        private readonly ITaskStatusPolicy _taskStatusPolicy;

        private readonly ITaskParser _taskParser;

        #endregion

        public DossierFileBuilder(ICacheRepository cacheRepository, ITaskStatusPolicy taskStatusPolicy, ITaskParser taskParser)
        {
            _cacheRepository = cacheRepository;
            _taskStatusPolicy = taskStatusPolicy;
            _taskParser = taskParser;
            _providedDocumentList = new List<ProvidedDocument>();
        }

        #region Fluent

        public virtual IDossierFileBuilder FromTask(Task task)
        {
            _task = task;
            return this;
        }

        public virtual IDossierFileBuilder WithInventoryRegNumber(int? inventoryRegNumber)
        {
            _inventoryRegNumber = inventoryRegNumber;
            return this;
        }

        public virtual IDossierFileBuilder ToEmployee(Employee responsibleEmployee)
        {
            _responsibleEmployee = responsibleEmployee;
            return this;
        }

        public virtual IDossierFileBuilder WithAcceptedStatus(string notice)
        {
            _taskStatus = TaskStatusType.Accepted;
            _statusNotice = notice;
            return this;
        }

        public virtual IDossierFileBuilder WithRejectedStatus(string notice)
        {
            _taskStatus = TaskStatusType.Rejected;
            _statusNotice = notice;
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

            _providedDocumentList.Add(doc);

            return this;
        }

        #endregion

        public DossierFile Build()
        {
            if (!IsDataComplete())
            {
                throw new BuildingDataNotCompleteException();
            }

            var dossierFile = DossierFile.CreateInstance();
            dossierFile.CreateDate = DateTime.Today;
            dossierFile.Task = _task;
                SetBuildStatusToTask();
            dossierFile.TaskId = dossierFile.Task.Id;
            dossierFile.CurrentStatus = DossierFileStatus.Unbounded;
            dossierFile.Employee = _responsibleEmployee;
            dossierFile.EmployeeId = _responsibleEmployee.Id;

            var activity =
                _cacheRepository.GetCache<LicensedActivity>()
                    .Single(t => t.ServiceGroupId == dossierFile.ServiceGroupId);

            dossierFile.LicensedActivity = activity;

            dossierFile.DocumentInventory = DocumentInventory.CreateInstance();
            dossierFile.DocumentInventory.Stamp = DateTime.Today;
            dossierFile.DocumentInventory.RegNumber = _inventoryRegNumber.Value;
            dossierFile.DocumentInventory.LicenseHolder = _taskParser.ParseHolderInfo(_task).FullName;
            dossierFile.DocumentInventory.EmployeeName = _responsibleEmployee.Name;
            dossierFile.DocumentInventory.EmployeePosition = _responsibleEmployee.Position;
            dossierFile.DocumentInventory.LicensedActivity = activity.Name;
            dossierFile.DocumentInventory.ProvidedDocumentList 
                = new EditableList<ProvidedDocument>(_providedDocumentList);

            dossierFile.Scenario = _cacheRepository.GetCache<Scenario>().SingleOrDefault(s => s.ServiceId == _task.ServiceId);
            dossierFile.ScenarioId = dossierFile.Scenario.Id;
            dossierFile.CurrentScenarioStepId =
                dossierFile.Scenario.ScenarioStepList.OrderBy(t => t.SortOrder).ToList()[1].Id;
            AddFirstSteps(dossierFile);

            return dossierFile;
        }

        private void AddFirstSteps(DossierFile dossierFile)
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

        private void SetBuildStatusToTask()
        {

            if (_taskStatusPolicy.CanSetStatus(_taskStatus.Value, _task))
            {
                _taskStatusPolicy.SetStatus(_taskStatus.Value, _statusNotice, _task);
            }
            else
            {
                throw new CantSetStatusException(_taskStatus.Value);
            }
        }

        private bool IsDataComplete()
        {
            if (_task == null)
            {
                return false;
            }

            if (!_taskStatus.HasValue)
            {
                return false;
            }

            if (_taskStatus.Value == TaskStatusType.Accepted)
            {
                return _inventoryRegNumber.HasValue && _responsibleEmployee != null;
            }
            else
            {
                return !string.IsNullOrEmpty(_statusNotice);
            }
        }
    }
}
