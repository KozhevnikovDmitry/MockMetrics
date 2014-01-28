using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.AcceptTask.AcceptException;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Person;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.AcceptTask
{
    /// <summary>
    /// Тесты на класс DossierFileBuilder
    /// </summary>
    [TestFixture]
    public class DossierFileBuilderTests
    {
        /// <summary>
        /// Тестируемый сборщик тома
        /// </summary>
        private DossierFileBuilder _builder;

        #region TestData

        /// <summary>
        /// Объект заявка
        /// </summary>
        private Task _task;

        /// <summary>
        /// Политика управления статусами статусами
        /// </summary>
        private ITaskStatusPolicy _taskStatusPolicy;

        private IParser _taskParser;

        /// <summary>
        /// Объект сотрудник
        /// </summary>
        private Employee _employee;

        /// <summary>
        /// Заявка со статусом Принято
        /// </summary>
        private Task _acceptedTask;

        /// <summary>
        /// Заявка со статусом Отклонено
        /// </summary>
        private Task _rejectedTask;

        /// <summary>
        /// Сценарий ведения тома
        /// </summary>
        private Scenario _scenario;

        /// <summary>
        /// Мок менеджера спраовчников
        /// </summary>
        private IDictionaryManager _dictionaryManager;

        #endregion

        /// <summary>
        /// Общие настройки тестов
        /// </summary>        
        [TestFixtureSetUp]
        public void SetupMethods()
        {
            _employee = Mock.Of<Employee>(t => t.Id == 1 && t.Name == "EmployeeName" && t.Position == "EmployeePosition");

            _scenario = Scenario.CreateInstance();
            _scenario.Id = 1;
            _scenario.ServiceId = 1;
            _scenario.ScenarioStepList = new List<ScenarioStep> 
            { 
                Mock.Of<ScenarioStep>(s => s.Id == 1 && s.SortOrder == 0),
                Mock.Of<ScenarioStep>(s => s.Id == 2 && s.SortOrder == 1)
            };

            _dictionaryManager =
                Mock.Of<IDictionaryManager>(
                    dm => dm.GetDictionary<Scenario>() == new List<Scenario> { _scenario }
                       && dm.GetDictionary<LicensedActivity>() == new List<LicensedActivity>
                           {
                               Mock.Of<LicensedActivity>(l => l.Id == 500100 && l.ServiceGroupId == 100500 && l.Name == "LicensedActivity")
                           });
        }

        /// <summary>
        /// Настройки перед каждым тестом
        /// </summary>  
        [SetUp]
        public void SetupTest()
        {
            var acceptedStatus = Mock.Of<TaskStatus>(t => t.Note == "Accepted"
                                                          && t.State == TaskStatusType.Accepted
                                                          && t.Stamp == DateTime.Now);

            _acceptedTask = Mock.Of<Task>(t => t.Id == 1
                                               && t.CurrentState == TaskStatusType.Accepted
                                               && t.ServiceId == 1
                                               && t.Service == Mock.Of<Service>(s => s.ServiceGroup == Mock.Of<ServiceGroup>(g => g.Id == 100500))
                                               && t.StatusList == new EditableList<TaskStatus> { acceptedStatus });

            _rejectedTask = Task.CreateInstance();
            _rejectedTask.Id = 1;
            _rejectedTask.ServiceId = 1;
            _rejectedTask.CurrentState = TaskStatusType.Rejected;
            var rejectedStatus = TaskStatus.CreateInstance();
            rejectedStatus.Note = "Rejected";
            rejectedStatus.State = TaskStatusType.Rejected;
            rejectedStatus.Stamp = DateTime.Now;
            _rejectedTask.StatusList.Add(rejectedStatus);

            _taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(tsp => tsp.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()) &&
                                                                  tsp.SetStatus(TaskStatusType.Accepted, It.IsAny<string>(), It.IsAny<Task>()) == _acceptedTask &&
                                                                  tsp.SetStatus(TaskStatusType.Rejected, It.IsAny<string>(), It.IsAny<Task>()) == _rejectedTask);
            _taskParser =
                Mock.Of<IParser>(
                    t => t.ParseHolderInfo(It.IsAny<Task>()) == new HolderInfo { FullName = "LicenseHolder" });

            _task = Task.CreateInstance();
            _task.Id = 1;
            _builder = new DossierFileBuilder(_dictionaryManager, _taskStatusPolicy, _taskParser);
        }

        /// <summary>
        /// Тест на корректность задания основания(заявки) для тома
        /// </summary>
        [Test]
        public void CorrectSetTaskTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.ToEmployee(_employee);
            _builder.WithInventoryRegNumber(1);
            _builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.TaskId, _task.Id);
        }

        /// <summary>
        /// Тест на корректность заполнения даты заведения тома
        /// </summary>
        [Test]
        public void CorrectCreateDateTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.CreateDate, DateTime.Today);
        }

        /// <summary>
        /// Тест на корректность добавления регистрационного номера описи
        /// </summary>
        [Test]
        public void CorrectWithInventoryRegNumberTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.DocumentInventory.RegNumber, 1);
        }

        /// <summary>
        /// Тест на корректность заполнения даты составления описи
        /// </summary>
        [Test]
        public void CorrectInventoryStampTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.DocumentInventory.Stamp, DateTime.Today);
        }

        /// <summary>
        /// Тест на корректность добавления ответственного сотрудника
        /// </summary>
        [Test]
        public void CorrectToEmployeeTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.EmployeeId, 1);
        }

        /// <summary>
        /// Тест на корректность добавления статуса "Принято к рассмотрению"
        /// </summary>
        [Test]
        public void CorrectWithAcceptedStatusTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.TaskState, TaskStatusType.Accepted);
            Assert.AreEqual(dossierFile.Task.StatusList.First().State, TaskStatusType.Accepted);
            Assert.AreEqual(dossierFile.Task.StatusList.First().Note, "Accepted");
        }

        /// <summary>
        /// Тест на корректность добавления статуса "Отклонено"
        /// </summary>
        [Test]
        public void CorrectWithRejectedStatusTest()
        {
            // Arrange
            _builder.FromTask(_rejectedTask);
            _builder.WithRejectedStatus("Rejected");

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.TaskState, TaskStatusType.Rejected);
            Assert.AreEqual(dossierFile.Task.StatusList.First().State, TaskStatusType.Rejected);
            Assert.AreEqual(dossierFile.Task.StatusList.First().Note, "Rejected");
        }

        /// <summary>
        /// Тест на попытку присвоить статус заявке, которой нельзя присвоить такой статус.
        /// </summary>
        [Test]
        public void SetWrongStatusTest()
        {
            // Arrange
            _taskStatusPolicy = Mock.Of<ITaskPolicy>(tsp => tsp.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()) == false);
            _builder = new DossierFileBuilder(_dictionaryManager, _taskStatusPolicy, _taskParser);
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus(string.Empty);

            // Act Assert 
            Assert.Throws<CantSetStatusException>(() => _builder.Create());
        }

        /// <summary>
        /// Тесты на попытку создания тома с неполными данными
        /// </summary>
        /// <param name="isEmployeeSetted">Флаг - назначен ли отвественный</param>
        /// <param name="inventoryRegNumber">Регистрационный номер описи</param>
        /// <param name="isAccepted">Флаг "Принято или Отклонено"</param>
        /// <param name="notice">Комметраний к статусу</param>
        [TestCase(false, null, true, null, ExpectedException = typeof(BuildingDataNotCompleteException))]
        [TestCase(false, 1, true, null, ExpectedException = typeof(BuildingDataNotCompleteException))]
        [TestCase(true, null, true, null, ExpectedException = typeof(BuildingDataNotCompleteException))]
        [TestCase(null, null, false, null, ExpectedException = typeof(BuildingDataNotCompleteException))]
        public void CreateWithIncomleteDataDossierFileBuilderTest(bool isEmployeeSetted,
                                                                  int? inventoryRegNumber,
                                                                  bool isAccepted,
                                                                  string notice)
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(inventoryRegNumber);

            if (isEmployeeSetted)
            {
                _builder.ToEmployee(_employee);
            }

            if (isAccepted)
            {
                _builder.WithAcceptedStatus(notice);
            }
            else
            {
                _builder.WithRejectedStatus(notice);
            }

            // Act
            _builder.Create();
        }

        /// <summary>
        /// Тест на попытку создания тома без указания заявки
        /// </summary>
        [Test]
        public void CreateWithoutTaskDossierFileBuilderTest()
        {
            _builder.ToEmployee(_employee);
            _builder.WithInventoryRegNumber(1);
            _builder.WithAcceptedStatus(string.Empty);

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(() => _builder.Create());
        }


        /// <summary>
        /// Тест на корректность задания сценрия ведения для тома
        /// </summary>
        [Test]
        public void CorrectSetScenarioTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.ToEmployee(_employee);
            _builder.WithInventoryRegNumber(1);
            _builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.ScenarioId, _scenario.Id);
        }

        /// <summary>
        /// Тест на корректность лицензируемой деятельности  для тома
        /// </summary>
        [Test]
        public void CorrectSetLicensedActivityTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.ToEmployee(_employee);
            _builder.WithInventoryRegNumber(1);
            _builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.LicensedActivity.Id, 500100);
        }

        /// <summary>
        /// Тест на корректно заполненый null'ом LicenseDossierId
        /// </summary>
        [Test]
        public void NullLicenseDossierIdTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.Null(dossierFile.LicenseDossierId);
        }

        /// <summary>
        /// Тест на правильное проставление статуса тома
        /// </summary>
        [Test]
        public void CorrectDossierFileCurrentStatusTest()
        {

            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.CurrentStatus, DossierFileStatus.Unbounded);
        }

        /// <summary>
        /// Тест на проставление корректного значения для текущего этапа ведения тома - 2
        /// </summary>
        [Test]
        public void CorrectDossierFileCurrentScenarioStepTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.CurrentScenarioStepId, _scenario.ScenarioStepList.OrderBy(t => t.SortOrder).ToList()[1].Id);
        }

        /// <summary>
        /// Тест на попытку создания тома с незаведённым сценарием
        /// </summary>
        [Test]
        public void WrongScenarioBuildTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus("Accepted");

            _scenario.ScenarioStepList = new List<ScenarioStep>();

            // Assert
            Assert.Throws<WrongScenarioSteplistException>(() => _builder.Create());
        }

        /// <summary>
        /// Тест на заведение первого этапа ведения тома
        /// </summary>
        [Test]
        public void CorrectFirstDossierFileScenarioStepTest()
        {
            _builder.FromTask(_acceptedTask);
            _builder.WithInventoryRegNumber(1);
            _builder.ToEmployee(_employee);
            _builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.DossierFileStepList.Count, 2);

            var firstStep = dossierFile.DossierFileStepList.First();
            Assert.AreEqual(firstStep.StartDate.Date, DateTime.Today);
            Assert.AreNotEqual(firstStep.ScenarioStepId, dossierFile.CurrentScenarioStepId);
            Assert.AreEqual(firstStep.EmployeeId, dossierFile.EmployeeId);

            var secondStep = dossierFile.DossierFileStepList.Last();
            Assert.AreEqual(secondStep.StartDate.Date, DateTime.Today);
            Assert.AreEqual(secondStep.ScenarioStepId, dossierFile.CurrentScenarioStepId);
            Assert.AreEqual(secondStep.EmployeeId, dossierFile.EmployeeId);
        }

        [Test]
        public void AddProvidedDocumentTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.ToEmployee(_employee);
            _builder.WithInventoryRegNumber(1);
            _builder.WithAcceptedStatus(string.Empty);
            _builder.AddProvidedDocument("      Document        ", 3);

            // Act
            var dossierFile = _builder.Create();
            var provDoc = dossierFile.DocumentInventory.ProvidedDocumentList.Single();

            // Assert
            Assert.AreEqual(provDoc.Name, "Document");
            Assert.AreEqual(provDoc.Quantity, 3);
        }

        [Test]
        public void CantAddProvidedDocumentWithWrongDataTest()
        {
            // Arrange
            _builder.FromTask(_acceptedTask);
            _builder.ToEmployee(_employee);
            _builder.WithInventoryRegNumber(1);
            _builder.WithAcceptedStatus(string.Empty);

            // Assert
            Assert.Throws<CantAddProvidedDocumentWithEmptyNameException>(() => _builder.AddProvidedDocument("  ", 3));
            Assert.Throws<CantAddProvidedDocumentWithNegativeQuantityException>(() => _builder.AddProvidedDocument("Doc", 0));
            Assert.Throws<CantAddProvidedDocumentWithNegativeQuantityException>(() => _builder.AddProvidedDocument("Doc", -1));
        }

        [Test]
        public void SetupInventoryDataFromDossierFileTest()
        {
            _builder.FromTask(_acceptedTask);
            _builder.ToEmployee(_employee);
            _builder.WithInventoryRegNumber(1);
            _builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = _builder.Create();

            // Assert
            Assert.AreEqual(dossierFile.DocumentInventory.LicenseHolder, "LicenseHolder");
            Assert.AreEqual(dossierFile.DocumentInventory.EmployeeName, "EmployeeName");
            Assert.AreEqual(dossierFile.DocumentInventory.EmployeePosition, "EmployeePosition");
            Assert.AreEqual(dossierFile.DocumentInventory.LicensedActivity, "LicensedActivity");
        }
    }
}
