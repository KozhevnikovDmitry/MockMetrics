using System.Linq;
using Moq;
using NUnit.Framework;
using PostGrad.BL.StepByStep.After;
using PostGrad.Core.BL;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Person;

namespace PostGrad.BL.Tests.StepByStep.After
{
    [TestFixture]
    public class DossierFileBuildingFacadeTests
    {
        [Test]
        public void FromTaskTest()
        {
            // Arrange
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var taskParser = Mock.Of<ITaskParser>();
            var steps = Mock.Of<BuildingFileSteps>();
            var builder = new DossierFileBuildingFacade(steps, dictionaryManager, taskStatusPolicy, taskParser);
            var task = Mock.Of<Task>(t => t.Id == 2);

            // Act
            builder.FromTask(task);

            // Assert
            Assert.AreEqual(steps.Task, task);
        }

        [Test]
        public void WithInventoryRegNumberTest()
        {
            // Arrange
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var taskParser = Mock.Of<ITaskParser>();
            var steps = Mock.Of<BuildingFileSteps>();
            var builder = new DossierFileBuildingFacade(steps, dictionaryManager, taskStatusPolicy, taskParser);

            // Act
            builder.WithInventoryRegNumber(1);

            // Assert
            Assert.AreEqual(steps.InventoryRegNumber, 1);
        }

        [Test]
        public void ToEmployeeTest()
        {
            // Arrange
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var taskParser = Mock.Of<ITaskParser>();
            var steps = Mock.Of<BuildingFileSteps>();
            var builder = new DossierFileBuildingFacade(steps, dictionaryManager, taskStatusPolicy, taskParser);
            var employee = Mock.Of<Employee>();

            // Act
            builder.ToEmployee(employee);

            // Assert
            Assert.AreEqual(steps.ResponsibleEmployee, employee);
        }

        [Test]
        public void WithAcceptedStatusTest()
        {
            // Arrange

            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var taskParser = Mock.Of<ITaskParser>();
            var steps = Mock.Of<BuildingFileSteps>();
            var builder = new DossierFileBuildingFacade(steps, dictionaryManager, taskStatusPolicy, taskParser);

            // Act
            builder.WithAcceptedStatus("Accepted");

            // Assert
            Assert.AreEqual(steps.StatusNotice, "Accepted");
            Assert.AreEqual(steps.TaskStatus, TaskStatusType.Accepted);

        }

        [Test]
        public void WithRejectedStatusTest()
        {
            // Arrange
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var taskParser = Mock.Of<ITaskParser>();
            var steps = Mock.Of<BuildingFileSteps>();
            var builder = new DossierFileBuildingFacade(steps, dictionaryManager, taskStatusPolicy, taskParser);

            // Act
            builder.WithRejectedStatus("Rejected");

            // Assert
            Assert.AreEqual(steps.StatusNotice, "Rejected");
            Assert.AreEqual(steps.TaskStatus, TaskStatusType.Rejected);
        }

        [Test]
        public void AddProvidedDocumentTest()
        {
            // Arrange
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var taskParser = Mock.Of<ITaskParser>();
            var steps = Mock.Of<BuildingFileSteps>();
            var builder = new DossierFileBuildingFacade(steps, dictionaryManager, taskStatusPolicy, taskParser);

            // Act
            builder.AddProvidedDocument("      Document        ", 3);
            var provDoc = steps.ProvidedDocumentList.Single();

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
            var steps = Mock.Of<BuildingFileSteps>();
            var builder = new DossierFileBuildingFacade(steps, dictionaryManager, taskStatusPolicy, taskParser);

            // Assert
            Assert.Throws<CantAddProvidedDocumentWithEmptyNameException>(() => builder.AddProvidedDocument("  ", 3));
            Assert.Throws<CantAddProvidedDocumentWithNegativeQuantityException>(() => builder.AddProvidedDocument("Doc", 0));
            Assert.Throws<CantAddProvidedDocumentWithNegativeQuantityException>(() => builder.AddProvidedDocument("Doc", -1));
        }
    }
}
