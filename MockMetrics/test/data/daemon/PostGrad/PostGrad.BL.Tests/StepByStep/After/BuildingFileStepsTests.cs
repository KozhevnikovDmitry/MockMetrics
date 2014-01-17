using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PostGrad.BL.StepByStep.After;
using PostGrad.Core.BL;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Licensing;
using PostGrad.Core.DomainModel.Person;

namespace PostGrad.BL.Tests.StepByStep.After
{
    [TestFixture]
    public class BuildingFileStepsTests
    {
        [Test]
        public void PrepareDossierFileTest()
        {
            // Arrange
            var task = Mock.Of<Task>(t => t.Id == 1);
            var steps = new BuildingFileSteps
            {
                Task = task
            };

            // Act
            var dossierFile = steps.PrepareDossierFile();

            // Assert
            Assert.AreEqual(dossierFile.CreateDate, DateTime.Today);
            Assert.AreEqual(dossierFile.Task, task);
            Assert.AreEqual(dossierFile.TaskId, 1);
        }

        [Test]
        public void SetupStatusTest()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var dossierFile = Mock.Of<DossierFile>(t => t.Task == task);
            var taskStatusPolicy = new Mock<ITaskStatusPolicy>();
            taskStatusPolicy.Setup(t => t.CanSetStatus(TaskStatusType.Accepted, task)).Returns(true);
            var steps = new BuildingFileSteps
            {
                TaskStatus = TaskStatusType.Accepted,
                StatusNotice = "Accepted"
            };

            // Act
            steps.SetupStatus(dossierFile, taskStatusPolicy.Object);

            // Assert 
            taskStatusPolicy.Verify(t => t.SetStatus(TaskStatusType.Accepted, "Accepted", task), Times.Once);
            Assert.AreEqual(dossierFile.CurrentStatus, DossierFileStatus.Unbounded);
        }

        [Test]
        public void CantSetStatusTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()) == false);
            var steps = new BuildingFileSteps
            {
                TaskStatus = TaskStatusType.Accepted
            };


            // Assert 
            Assert.Throws<CantSetStatusException>(() => steps.SetupStatus(dossierFile, taskStatusPolicy));
        }

        [Test]
        public void IncompleteDataWithNullTaskTest()
        {
            // Arrange 
            var steps = new BuildingFileSteps();

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(steps.CheckDataCompleteness);
        }

        [Test]
        public void IncompleteDataWithNullTaskStatusTest()
        {
            // Arrange 
            var steps = new BuildingFileSteps
            {
                Task = Mock.Of<Task>()
            };

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(steps.CheckDataCompleteness);
        }

        [Test]
        public void IncompleteDataWithNullEmployeeTest()
        {
            // Arrange 
            var steps = new BuildingFileSteps
            {
                Task = Mock.Of<Task>(),
                TaskStatus = TaskStatusType.Accepted
            };

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(steps.CheckDataCompleteness);
        }

        [Test]
        public void IncompleteDataWithNullInventoryRegNumberTest()
        {
            // Arrange 
            var steps = new BuildingFileSteps
            {
                Task = Mock.Of<Task>(),
                ResponsibleEmployee = Mock.Of<Employee>(),
                TaskStatus = TaskStatusType.Accepted
            };

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(steps.CheckDataCompleteness);
        }

        [Test]
        public void IncompleteDataWithNullNoticeForRejectedStatusTest()
        {
            // Arrange 
            var steps = new BuildingFileSteps
            {
                Task = Mock.Of<Task>(),
                TaskStatus = TaskStatusType.Rejected
            };

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(steps.CheckDataCompleteness);
        }
        
        [Test]
        public void CompleteDataForAcceptedTest()
        {
            // Arrange 
            var steps = new BuildingFileSteps
            {
                Task = Mock.Of<Task>(),
                ResponsibleEmployee = Mock.Of<Employee>(),
                TaskStatus = TaskStatusType.Accepted,
                InventoryRegNumber = 1
            };

            // Assert
            Assert.DoesNotThrow(steps.CheckDataCompleteness);
        }

        [Test]
        public void CompleteDataForRejectedStatusTest()
        {
            // Arrange 
            var steps = new BuildingFileSteps
            {
                Task = Mock.Of<Task>(),
                TaskStatus = TaskStatusType.Rejected,
                StatusNotice = "Rejected"
            };

            // Assert
            Assert.DoesNotThrow(steps.CheckDataCompleteness);
        }

        [Test]
        public void SetupActivityTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.ServiceGroupId == 1);
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.ServiceGroupId == 1);
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> {licensedActivity});
           
            var steps = new BuildingFileSteps();

            // Act
            steps.SetupActivity(dossierFile,dictionaryManager);

            // Assert
            Assert.AreEqual(dossierFile.LicensedActivity, licensedActivity);
        }

        [Test]
        public void SetupEmployeeTest()
        {
            // Arrange
            var employee = Mock.Of<Employee>(t => t.Id == 1);
            var dossierFile = Mock.Of<DossierFile>();

            var steps = new BuildingFileSteps
            {
                ResponsibleEmployee = employee
            };

            // Act
            steps.SetupEmployee(dossierFile);

            // Assert
            Assert.AreEqual(dossierFile.Employee, employee);
            Assert.AreEqual(dossierFile.EmployeeId, 1);
        }

        [Test]
        public void SetupScenarioTest()
        {
            // Arrange
            var task = Mock.Of<Task>(t => t.ServiceId == 1);
            var dossierFile = Mock.Of<DossierFile>();
            var scenario = Mock.Of<Scenario>(t => t.ServiceId == 1 && t.Id == 2
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(t => t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var steps = new BuildingFileSteps
            {
                Task = task
            };

            // Act
            steps.SetupScenario(dossierFile, dictionaryManager);

            // Assert
            Assert.AreEqual(dossierFile.Scenario, scenario);
            Assert.AreEqual(dossierFile.ScenarioId, 2);
            Assert.AreEqual(dossierFile.CurrentScenarioStepId, 10);
        }

        [Test]
        public void BuildFileAddFisrtStepsTest()
        {
            // Arrange
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1 && s.Id == 20)
                                                  });
            var dossierFile = Mock.Of<DossierFile>(t => t.Scenario == scenario);
            var steps = new BuildingFileSteps();


            // Act
            steps.SetupFirstSteps(dossierFile);

            // Assert
            Assert.AreEqual(dossierFile.DossierFileStepList.Count, 2);

            var firstStep = dossierFile.DossierFileStepList.First();
            Assert.AreEqual(firstStep.StartDate.Date, DateTime.Today);
            Assert.AreEqual(firstStep.ScenarioStepId, 20);
            Assert.AreEqual(firstStep.EmployeeId, dossierFile.EmployeeId);

            var secondStep = dossierFile.DossierFileStepList.Last();
            Assert.AreEqual(secondStep.StartDate.Date, DateTime.Today);
            Assert.AreEqual(secondStep.ScenarioStepId, 10);
            Assert.AreEqual(secondStep.EmployeeId, dossierFile.EmployeeId);
        }

        [Test]
        public void BuildFileSetupInventoryDataTest()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var employee = Mock.Of<Employee>(t => t.Name == "EmployeeName" && t.Position == "EmployeePosition");
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.Name == "LicensedActivity" && t.ServiceGroupId == 1);
            var dossierFile = Mock.Of<DossierFile>(t => t.LicensedActivity == licensedActivity 
                                                     && t.Task == task);
           
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task).FullName == "LicenseHolder");
            var steps = new BuildingFileSteps
            {
                InventoryRegNumber = 100500,
                ResponsibleEmployee = employee
            };

            // Act
            steps.SetupInventory(dossierFile, taskParser);

            // Assert
            Assert.AreEqual(dossierFile.DocumentInventory.Stamp, DateTime.Today);
            Assert.AreEqual(dossierFile.DocumentInventory.RegNumber, 100500);
            Assert.AreEqual(dossierFile.DocumentInventory.LicenseHolder, "LicenseHolder");
            Assert.AreEqual(dossierFile.DocumentInventory.EmployeeName, "EmployeeName");
            Assert.AreEqual(dossierFile.DocumentInventory.EmployeePosition, "EmployeePosition");
            Assert.AreEqual(dossierFile.DocumentInventory.LicensedActivity, "LicensedActivity");
        }
    }
}