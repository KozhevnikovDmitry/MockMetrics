using System;
using System.Collections.Generic;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GrantResult;
using GU.MZ.BL.DomainLogic.GrantResult.GrantResultException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.GrantResult
{
    /// <summary>
    /// Тесты на методы класса ServiceResultGranter
    /// </summary>
    [TestFixture]
    public class ServiceResultGranterTests
    {
        #region TestData

        /// <summary>
        /// Мок маппера томов
        /// </summary>
        private Mock<IDomainDataMapper<DossierFile>> _mockFileMapper;

        /// <summary>
        /// Мок политики заявок
        /// </summary>
        private Mock<ITaskStatusPolicy> _mockTaskStatusPolicy;

        /// <summary>
        /// Мок менеджера справочников
        /// </summary>
        private IDictionaryManager _mockDictionaryManager;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Мок менеджера справочников 
            _mockDictionaryManager =
                Mock.Of<IDictionaryManager>(
                    d =>
                    d.GetDictionaryItem<Scenario>(1)
                    == Mock.Of<Scenario>(t => t.Id == 1 && t.ScenarioType == ScenarioType.Full)
                    && d.GetDictionaryItem<Scenario>(2)
                       == Mock.Of<Scenario>(t => t.Id == 2 && t.ScenarioType == ScenarioType.Light)
                    && d.GetDictionary<ServiceResult>()
                    == new List<ServiceResult>
                           {
                               Mock.Of<ServiceResult>(t => t.Id == 1 && t.ScenarioId == 1 && t.IsPositive),
                               Mock.Of<ServiceResult>(t => t.Id == 2 && t.ScenarioId == 1 && t.IsPositive == false),
                               Mock.Of<ServiceResult>(t => t.Id == 3 && t.ScenarioId == 2 && t.IsPositive),
                           });

            // Мок политики заявок
            _mockTaskStatusPolicy = new Mock<ITaskStatusPolicy>();

            // мок маппера томов
            _mockFileMapper = new Mock<IDomainDataMapper<DossierFile>>();
            _mockFileMapper.Setup(t => t.Save(It.IsAny<DossierFile>(), false))
                .Returns(DossierFile.CreateInstance());
        }

        /// <summary>
        /// Подготавливает тестовый том
        /// </summary>
        /// <returns>Тестовый том</returns>
        private DossierFile ArrangeDossierFile()
        {
            var dossierFile = DossierFile.CreateInstance();
            dossierFile.Task = Task.CreateInstance();
            dossierFile.TaskState = TaskStatusType.Ready;
            dossierFile.ScenarioId = 1;

            return dossierFile;
        }


        /// <summary>
        /// Подготавливает тестовый том
        /// </summary>
        /// <returns>Тестовый том</returns>
        private DossierFile ArrangeDossierFileWithResult()
        {
            var dossierFile = DossierFile.CreateInstance();
            dossierFile.Task = Task.CreateInstance();
            dossierFile.TaskState = TaskStatusType.Ready;
            dossierFile.DossierFileServiceResult = DossierFileServiceResult.CreateInstance();
            dossierFile.DossierFileServiceResult.ServiceResult = ServiceResult.CreateInstance();
            dossierFile.DossierFileServiceResult.ServiceResult.IsPositive = true;

            return dossierFile;
        }

        /// <summary>
        /// Возвращает грантер результатов
        /// </summary>
        /// <param name="dossierFile">Том</param>
        /// <returns>Грантер результатов</returns>
        private ServiceResultGranter BuildServiceResultGranter()
        {
            return new ServiceResultGranter();
        }

        /// <summary>
        /// Тест на заведение результата услуги для тома
        /// </summary>
        [Test]
        public void GrantServiceResultTest()
        {
            // Arrange
            var dossierFile = ArrangeDossierFile();
            var granter = BuildServiceResultGranter();

            // Act
            var result = granter.GrantServiseResult(dossierFile, _mockDictionaryManager);

            // Assert
            Assert.AreEqual(result, dossierFile.DossierFileServiceResult);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Тест на заведение корректных данных результата тома
        /// </summary>
        [Test]
        public void CorrectGrantDossierFileServiceResultDataTest()
        {
            // Arrange
            var dossierFile = ArrangeDossierFile();
            var granter = BuildServiceResultGranter();

            // Act
            var result = granter.GrantServiseResult(dossierFile, _mockDictionaryManager);

            // Assert
            Assert.AreEqual(result.Stamp.Date, DateTime.Today);
            Assert.AreEqual(result.ServiceResultId, 1);
        }

        /// <summary>
        /// Тест на заведение положительного результата услуги для тома с full сценарием
        /// </summary>
        [Test]
        public void GrantPositiveResultFullTest()
        {
            // Arrange
            var dossierFile = ArrangeDossierFile();
            dossierFile.TaskState = TaskStatusType.Ready;
            var granter = BuildServiceResultGranter();

            // Act
            var result = granter.GrantServiseResult(dossierFile, _mockDictionaryManager);

            // Assert
            Assert.That(result.ServiceResult.IsPositive);
        }

        /// <summary>
        /// Тест на заведение отрицательного результата услуги для тома с full сценарием
        /// </summary>
        [Test]
        public void GrantNegativeResultFullTest()
        {
            // Arrange
            var dossierFile = ArrangeDossierFile();
            dossierFile.TaskState = TaskStatusType.Rejected;
            var granter = BuildServiceResultGranter();

            // Act
            var result = granter.GrantServiseResult(dossierFile, _mockDictionaryManager);

            // Assert
            Assert.False(result.ServiceResult.IsPositive);
        }

        /// <summary>
        /// Тест на заведение положительного результата услуги для тома с light сценарием
        /// </summary>
        [Test]
        public void GrantPositiveResultLightTest()
        {
            // Arrange
            var dossierFile = ArrangeDossierFile();
            dossierFile.ScenarioId = 2;
            dossierFile.TaskState = TaskStatusType.Ready;
            var granter = BuildServiceResultGranter();

            // Act
            var result = granter.GrantServiseResult(dossierFile, _mockDictionaryManager);

            // Assert
            Assert.That(result.ServiceResult.IsPositive);
        }

        /// <summary>
        /// Тест на попытку заведения результата на том с заявкой в неправильном статусе
        /// </summary>
        [TestCase(TaskStatusType.Ready)]
        [TestCase(TaskStatusType.Rejected)]
        [TestCase(TaskStatusType.Accepted, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.CheckupWaiting, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.None, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.NotFilled, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.Working, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.Done, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        public void GrantResultToTaskWithWrongStatusTest(TaskStatusType currentStstus)
        {
            // Arrange
            var dossierFile = ArrangeDossierFile();
            dossierFile.TaskState = currentStstus;
            var granter = BuildServiceResultGranter();

            // Assert
            granter.GrantServiseResult(dossierFile, _mockDictionaryManager);
        }

        /// <summary>
        /// Тест на заведение результата при некорректных(слишком много) данных в справочнике результатов
        /// </summary>
        [Test]
        public void GrantResultWithNotSingleServiceResultOnLightScenarioTest()
        {
            // Arrange
            _mockDictionaryManager =
                Mock.Of<IDictionaryManager>(
                    d =>
                    d.GetDictionaryItem<Scenario>(1)
                    == Mock.Of<Scenario>(t => t.Id == 1 && t.ScenarioType == ScenarioType.Full)
                    && d.GetDictionaryItem<Scenario>(2)
                       == Mock.Of<Scenario>(t => t.Id == 2 && t.ScenarioType == ScenarioType.Light)
                    && d.GetDictionary<ServiceResult>()
                    == new List<ServiceResult>
                           {
                               Mock.Of<ServiceResult>(t => t.ScenarioId == 2 && t.IsPositive),
                               Mock.Of<ServiceResult>(t => t.ScenarioId == 2 && t.IsPositive == false),
                               Mock.Of<ServiceResult>(t => t.ScenarioId == 2 && t.IsPositive),
                           });

            var dossierFile = ArrangeDossierFile();
            dossierFile.ScenarioId = 2;
            var granter = BuildServiceResultGranter();

            // Assert
            Assert.Throws<NotSingleResulstOnLightScenarioException>(() => granter.GrantServiseResult(dossierFile, _mockDictionaryManager));
        }

        /// <summary>
        /// Тест на заведение результата при отсутсвтии данных в справочнике результатов
        /// </summary>
        [Test]
        public void GrantResultWithNoServiceResultOnLightScenarioTest()
        {
            // Arrange
            _mockDictionaryManager =
                Mock.Of<IDictionaryManager>(
                    d =>
                    d.GetDictionaryItem<Scenario>(1)
                    == Mock.Of<Scenario>(t => t.Id == 1 && t.ScenarioType == ScenarioType.Full)
                    && d.GetDictionaryItem<Scenario>(2)
                       == Mock.Of<Scenario>(t => t.Id == 2 && t.ScenarioType == ScenarioType.Light)
                    && d.GetDictionary<ServiceResult>()
                    == new List<ServiceResult>
                           {
                               Mock.Of<ServiceResult>(t => t.ScenarioId == 2 && t.IsPositive),
                               Mock.Of<ServiceResult>(t => t.ScenarioId == 2 && t.IsPositive == false),
                               Mock.Of<ServiceResult>(t => t.ScenarioId == 2 && t.IsPositive),
                           });

            var dossierFile = ArrangeDossierFile();
            var granter = BuildServiceResultGranter();

            // Assert
            Assert.Throws<NotSingleResulstOnLightScenarioException>(() => granter.GrantServiseResult(dossierFile, _mockDictionaryManager));
        }


        /// <summary>
        /// Тест на заведение результата при отсутсвтии результата нужного типа в справочнике результатов
        /// </summary>
        [TestCase(true, ExpectedException = typeof(NotSingleResulstOnLightScenarioException))]
        [TestCase(false, ExpectedException = typeof(NotSingleResulstOnLightScenarioException))]
        public void GrantResultWithNoPositiveServiceResultOnLightScenarioTest(bool isPositive)
        {
            // Arrange
            _mockDictionaryManager =
                Mock.Of<IDictionaryManager>(
                    d =>
                    d.GetDictionaryItem<Scenario>(1)
                    == Mock.Of<Scenario>(t => t.Id == 1 && t.ScenarioType == ScenarioType.Full)
                    && d.GetDictionary<ServiceResult>()
                    == new List<ServiceResult>
                           {
                               Mock.Of<ServiceResult>(
                                   t => t.ScenarioId == 1 && t.IsPositive == isPositive),
                           });

            var dossierFile = ArrangeDossierFile();
            if (isPositive)
            {
                dossierFile.TaskState = TaskStatusType.Rejected;
            }
            var granter = BuildServiceResultGranter();
            
            //Act
            granter.GrantServiseResult(dossierFile, _mockDictionaryManager);
        }

        /// <summary>
        /// Тест на клонирование тома перед сохранением
        /// </summary>
        [Test]
        public void SaveCloneResultDossierFileTest()
        {
            // Arrange
            var dossierFile = ArrangeDossierFileWithResult();
            var mockFile = new Mock<DossierFile>();
            mockFile.Setup(t => t.Clone()).Returns(dossierFile);
            var granter = BuildServiceResultGranter();

            // Act
            var savedFile = granter.SaveServiceResult(mockFile.Object, _mockTaskStatusPolicy.Object, _mockFileMapper.Object);

            // Assert
            mockFile.Verify(t => t.Clone(), Times.Once());
        }

        /// <summary>
        /// Тест сохранение результата тома с помощью маппера томов 
        /// </summary>
        [Test]
        public void SaveResultDossierFileMapperTest()
        {
            // Arrange
            var dossierFile = ArrangeDossierFileWithResult();
            var mockFile = new Mock<DossierFile>();
            mockFile.Setup(t => t.Clone()).Returns(dossierFile);
            var granter = BuildServiceResultGranter();

            // Act
            var savedFile = granter.SaveServiceResult(mockFile.Object, _mockTaskStatusPolicy.Object, _mockFileMapper.Object);

            // Assert
            _mockFileMapper.Verify(t => t.Save(dossierFile, false), Times.Once());
            Assert.AreEqual(savedFile, _mockFileMapper.Object.Save(dossierFile));
        }

        /// <summary>
        /// Тест на сохранение результата предоставления услуги
        /// </summary>
        [TestCase(TaskStatusType.Ready)]
        [TestCase(TaskStatusType.Rejected)]
        [TestCase(TaskStatusType.Accepted, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.CheckupWaiting, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.None, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.NotFilled, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.Working, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        [TestCase(TaskStatusType.Done, ExpectedException = typeof(GranterWorksWithTaskWithWrongStatusException))]
        public void SaveServiceTest(TaskStatusType currentStatus)
        {
            // Arrange
            var dossierFile = ArrangeDossierFileWithResult();
            dossierFile.TaskState = currentStatus;
            var mockFile = Mock.Of<DossierFile>(t => t.Clone() == dossierFile);
            var granter = BuildServiceResultGranter();
            var grantedStatus = currentStatus == TaskStatusType.Ready ? TaskStatusType.Done : TaskStatusType.Rejected;

            // Act
            var savedFile = granter.SaveServiceResult(mockFile, _mockTaskStatusPolicy.Object, _mockFileMapper.Object);

            // Assert
            if (grantedStatus == TaskStatusType.Done)
            _mockTaskStatusPolicy.Verify(t => t.SetStatus(grantedStatus, It.IsAny<string>(), dossierFile.Task), Times.Once());
        }
    }
}
