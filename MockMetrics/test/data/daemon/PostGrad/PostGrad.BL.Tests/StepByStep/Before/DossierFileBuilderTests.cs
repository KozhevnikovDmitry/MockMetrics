using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PostGrad.Core.BL;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Licensing;
using PostGrad.Core.DomainModel.Person;
using DossierFileBuilder = PostGrad.BL.StepByStep.Before.DossierFileBuilder;

namespace PostGrad.BL.Tests.StepByStep.Before
{

    [TestFixture]
    public class DossierFileBuilderTests
    {
        [Test]
        public void FromTaskTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>();
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t =>t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Id == 2);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = builder.Build();

            // Assert
            Assert.AreEqual(dossierFile.Task, task);
            Assert.AreEqual(dossierFile.TaskId, 2);
        }

        [Test]
        public void WithInventoryRegNumberTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>();
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>();
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = builder.Build();

            // Assert
            Assert.AreEqual(dossierFile.DocumentInventory.RegNumber, 1);
        }

        [Test]
        public void CreateDateIsTodayTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>();
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>();
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = builder.Build();

            // Assert
            Assert.AreEqual(dossierFile.CreateDate, DateTime.Today);
        }

        [Test]
        public void ToEmployeeTest()
        { 
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>();
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>();
            var employee = Mock.Of<Employee>(t => t.Id == 2);
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus(string.Empty);

            // Act
            var dossierFile = builder.Build();

            // Assert
            Assert.AreEqual(dossierFile.Employee, employee);
            Assert.AreEqual(dossierFile.EmployeeId, 2);
        }

        [Test]
        public void WithAcceptedStatusTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>();
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = new Mock<ITaskStatusPolicy>();
            taskStatusPolicy.Setup(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>())).Returns(true);
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy.Object, taskParser);
            var task = Mock.Of<Task>();
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Act
            builder.Build();

            // Assert
            taskStatusPolicy.Verify(t => t.SetStatus(TaskStatusType.Accepted, "Accepted" ,task), Times.Once);

        }

        [Test]
        public void WithRejectedStatusTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>();
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = new Mock<ITaskStatusPolicy>();
            taskStatusPolicy.Setup(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>())).Returns(true);
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy.Object, taskParser); 
            var task = Mock.Of<Task>();
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithRejectedStatus("Rejected");

            // Act
            builder.Build();

            // Assert
            taskStatusPolicy.Verify(t => t.SetStatus(TaskStatusType.Rejected, "Rejected", task), Times.Once);
        }

        [Test]
        public void AddProvidedDocumentTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.ServiceGroupId == 1);
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.Id == 1);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");
            builder.AddProvidedDocument("      Document        ", 3);

            // Act
            var dossierFile = builder.Build();
            var provDoc = dossierFile.DocumentInventory.ProvidedDocumentList.Single();

            // Assert
            Assert.AreEqual(provDoc.Name, "Document");
            Assert.AreEqual(provDoc.Quantity, 3);
        }

        [Test]
        public void CantAddProvidedDocumentWithWrongDataTest()
        {
            // Arrange
           
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var taskParser = Mock.Of<ITaskParser>();
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Service.ServiceGroupId == 1);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Assert
            Assert.Throws<CantAddProvidedDocumentWithEmptyNameException>(() => builder.AddProvidedDocument("  ", 3));
            Assert.Throws<CantAddProvidedDocumentWithNegativeQuantityException>(() => builder.AddProvidedDocument("Doc", 0));
            Assert.Throws<CantAddProvidedDocumentWithNegativeQuantityException>(() => builder.AddProvidedDocument("Doc", -1));
        }

        [Test]
        public void CantSetStatusTest()
        {
            // Arrange
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()) == false);
            var taskParser = Mock.Of<ITaskParser>();
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>();
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Assert 
            Assert.Throws<CantSetStatusException>(() => builder.Build());
        }

        [Test]
        public void IncompleteDataWithNullTaskTest()
        {
            // Arrange 
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()) == false);
            var taskParser = Mock.Of<ITaskParser>();
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var employee = Mock.Of<Employee>();
            builder.FromTask(null);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(() => builder.Build());
        }

        [Test]
        public void IncompleteDataWithNullEmployeeTest()
        {
            // Arrange 
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()) == false);
            var taskParser = Mock.Of<ITaskParser>();
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>();
            builder.FromTask(task);
            builder.ToEmployee(null);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(() => builder.Build());
        }

        [Test]
        public void IncompleteDataWithNullInventoryRegNumberTest()
        {
            // Arrange 
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()) == false);
            var taskParser = Mock.Of<ITaskParser>();
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>();
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(null);
            builder.WithAcceptedStatus("Accepted");

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(() => builder.Build());
        }

        [Test]
        public void IncompleteDataWithNullNoticeForRejectedStatusTest()
        {
            // Arrange 
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()) == false);
            var taskParser = Mock.Of<ITaskParser>();
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>();
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithRejectedStatus(null);

            // Assert
            Assert.Throws<BuildingDataNotCompleteException>(() => builder.Build());
        }
        
        [Test]
        public void CompleteDataForAcceptedTest()
        {
            // Arrange 
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.ServiceGroupId == 1);
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.Id == 1);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Assert
            Assert.DoesNotThrow(() => builder.Build());
        }

        [Test]
        public void CompleteDataForRejectedStatusTest()
        {
            // Arrange 
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.ServiceGroupId == 1);
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                   && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.Id == 1);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithRejectedStatus("Rejected");

            // Assert
            Assert.DoesNotThrow(() => builder.Build());
        }

        [Test]
        public void BuildFileCreateDateIsTodayTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.ServiceGroupId == 1);
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.Id == 1);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = builder.Build();

            // Assert
            Assert.AreEqual(dossierFile.CreateDate, DateTime.Today);
        }

        [Test]
        public void BuildFileCurrentStatusIsUnboundTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.ServiceGroupId == 1);
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.Id == 1);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = builder.Build();

            // Assert
            Assert.AreEqual(dossierFile.CurrentStatus, DossierFileStatus.Unbounded);
        }

        [Test]
        public void BuildFileSetupLicensedActivityTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.ServiceGroupId == 1);
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.Id == 1);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = builder.Build();

            // Assert
            Assert.AreEqual(dossierFile.LicensedActivity, licensedActivity);
        }

        [Test]
        public void BuildFileSetupScenarioTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.ServiceGroupId == 1);
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.Id == 1);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = builder.Build();

            // Assert
            Assert.AreEqual(dossierFile.Scenario, scenario);
            Assert.AreEqual(dossierFile.ScenarioId, 1);
            Assert.AreEqual(dossierFile.CurrentScenarioStepId, 10);
        }
        
        [Test]
        public void BuildFileAddFisrtStepsTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.ServiceGroupId == 1);
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1 && s.Id == 20)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>());
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.Id == 1);
            var employee = Mock.Of<Employee>();
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = builder.Build();

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
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.Name == "LicensedActivity" && t.ServiceGroupId == 1);
            var scenario = Mock.Of<Scenario>(t => t.Id == 1
                                                  && t.ScenarioStepList == new List<ScenarioStep>
                                                  {
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 2 && s.Id == 10),
                                                      Mock.Of<ScenarioStep>(s => s.SortOrder == 1)
                                                  });
            var dictionaryManager =
                Mock.Of<ICacheRepository>(
                    t => t.GetCache<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                         && t.GetCache<Scenario>() == new List<Scenario> { scenario });
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>(t => t.CanSetStatus(It.IsAny<TaskStatusType>(), It.IsAny<Task>()));
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.Id == 1);
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task).FullName == "LicenseHolder");
            var builder = new DossierFileBuilder(dictionaryManager, taskStatusPolicy, taskParser);
            var employee = Mock.Of<Employee>(t => t.Name == "EmployeeName" && t.Position == "EmployeePosition");
            builder.FromTask(task);
            builder.ToEmployee(employee);
            builder.WithInventoryRegNumber(1);
            builder.WithAcceptedStatus("Accepted");

            // Act
            var dossierFile = builder.Build();

            // Assert
            Assert.AreEqual(dossierFile.DocumentInventory.Stamp,  DateTime.Today);
            Assert.AreEqual(dossierFile.DocumentInventory.LicenseHolder, "LicenseHolder");
            Assert.AreEqual(dossierFile.DocumentInventory.EmployeeName, "EmployeeName");
            Assert.AreEqual(dossierFile.DocumentInventory.EmployeePosition, "EmployeePosition");
            Assert.AreEqual(dossierFile.DocumentInventory.LicensedActivity, "LicensedActivity");
        }
    }
}
