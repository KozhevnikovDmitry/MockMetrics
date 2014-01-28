using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using GU.DataModel;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Person;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    /// <summary>
    /// Тесты на класс DossierFileDataMapper
    /// </summary>
    [TestFixture]
    public class DossierFileDataMapperTests : BaseTestFixture
    {
        #region TestData

        private IDictionaryManager _stubDictionaryManager;

        private IDomainDataMapper<Task> _stubTaskMapper;

        private IDomainDataMapper<LicenseDossier> _stubDossierMapper;

        private IDomainDataMapper<Employee> _stubEmployeeMapper;

        private IDomainDataMapper<DossierFileScenarioStep> _stubFileStepMapper;

        private IDomainDataMapper<HolderRequisites> _stubRequisitesMapper;

        private IDomainDataMapper<DocumentInventory> _stubInventoryMapper;

        private IDomainDataMapper<License> _stubLicenseMapper;

        #endregion

        [SetUp]
        public void Setup()
        {
            // толстый стаб для затыкания сложной логики с данными из dictionaryManager
            _stubDictionaryManager =
                Mock.Of<IDictionaryManager>(t => t.GetDictionary<LicensedActivity>() == new List<LicensedActivity> { Mock.Of<LicensedActivity>() }
                                              && t.GetDictionaryItem<Scenario>(It.IsAny<int>()) == Mock.Of<Scenario>());

            _stubRequisitesMapper = Mock.Of<IDomainDataMapper<HolderRequisites>>();
            _stubTaskMapper = Mock.Of<IDomainDataMapper<Task>>();
            _stubDossierMapper = Mock.Of<IDomainDataMapper<LicenseDossier>>();
            _stubEmployeeMapper = Mock.Of<IDomainDataMapper<Employee>>();
            _stubFileStepMapper = Mock.Of<IDomainDataMapper<DossierFileScenarioStep>>();
            _stubLicenseMapper = Mock.Of<IDomainDataMapper<License>>();
            _stubInventoryMapper = Mock.Of<IDomainDataMapper<DocumentInventory>>(t => t.Retrieve(It.IsAny<object>(), It.IsAny<IDomainDbManager>()) == Mock.Of<DocumentInventory>());
        }

        #region Retrieve Tests

        [Test]
        public void RetrieveDossierFileTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file);

            var mapper = new DossierFileDataMapper(_stubTaskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result, file);
        }

        [Test]
        public void RetrieveTaskTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.TaskId == 2);
            var task = Mock.Of<Task>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file);

            var mapper = new DossierFileDataMapper(Mock.Of<IDomainDataMapper<Task>>(t => t.Retrieve(2, db) == task),
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.Task, task);
        }

        [Test]
        public void RetrieveDossierFileServiceResultTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.DossierFileServiceResultId == 2);
            var serviceResult = Mock.Of<DossierFileServiceResult>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file
                                                 && t.RetrieveDomainObject<DossierFileServiceResult>(2) == serviceResult);

            var mapper = new DossierFileDataMapper(_stubTaskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.DossierFileServiceResult, serviceResult);
        }

        [Test]
        public void RetrieveHolderReqisitesTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.HolderRequisitesId == 2 && t.LicenseDossierId == 3);
            var holderRequisites = Mock.Of<HolderRequisites>();
            var holder = Mock.Of<LicenseHolder>();
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseHolder == holder);
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file
                                                 && t.RetrieveDomainObject<LicenseDossier>(3) == dossier);

            var requisitesMapper =
                Mock.Of<IDomainDataMapper<HolderRequisites>>(t => t.Retrieve(2, db) == holderRequisites);

            var mapper = new DossierFileDataMapper(_stubTaskMapper,
                                                   _stubFileStepMapper,
                                                   requisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.HolderRequisites, holderRequisites);
            Assert.AreEqual(result.HolderRequisites.LicenseHolder, holder);
        }

        [Test]
        public void RetrieveLicenseDossierTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.LicenseDossierId == 2);
            var dossier = Mock.Of<LicenseDossier>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file
                                                 && t.RetrieveDomainObject<LicenseDossier>(2) == dossier);
            var mockDossierMapper = new Mock<IDomainDataMapper<LicenseDossier>>();

            var mapper = new DossierFileDataMapper(_stubTaskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   mockDossierMapper.Object,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.LicenseDossier, dossier);
            mockDossierMapper.Verify(t => t.FillAssociations(dossier, db), Times.Once());
        }

        [Test]
        public void RetrieveEmployeeTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.EmployeeId == 2);
            var employee = Mock.Of<Employee>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file
                                                 && t.RetrieveDomainObject<Employee>(2) == employee);
            var mockEmployeeMapper = new Mock<IDomainDataMapper<Employee>>();

            var mapper = new DossierFileDataMapper(_stubTaskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   mockEmployeeMapper.Object,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.Employee, employee);
            mockEmployeeMapper.Verify(t => t.FillAssociations(employee, db), Times.Once());
        }

        [Test]
        public void RetrieveFileStepListTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.Id == 1);
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 2 && t.DossierFileId == 1);
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file
                                                 && t.GetDomainTable<DossierFileScenarioStep>() ==
                                                        new List<DossierFileScenarioStep> { fileStep, Mock.Of<DossierFileScenarioStep>() }.AsQueryable());

            var fileStepMapper = Mock.Of<IDomainDataMapper<DossierFileScenarioStep>>(t => t.Retrieve(2, db) == fileStep);

            var mapper = new DossierFileDataMapper(_stubTaskMapper,
                                                   fileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.DossierFileStepList.Single(), fileStep);
        }

        [Test]
        public void RetrieveLicensedActivityTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.ServiceGroupId == 2);
            var licensedActivity = Mock.Of<LicensedActivity>(l => l.ServiceGroupId == 2);
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file);
            var dictionaryManager =
                Mock.Of<IDictionaryManager>(t => t.GetDictionary<LicensedActivity>() == new List<LicensedActivity> { licensedActivity }
                                              && t.GetDictionaryItem<Scenario>(It.IsAny<int>()) == Mock.Of<Scenario>());

            var mapper = new DossierFileDataMapper(_stubTaskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   dictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.LicensedActivity, licensedActivity);
        }

        [Test]
        public void RetrieveScenarioTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.ScenarioId == 2);
            var scenario = Mock.Of<Scenario>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file);
            var dictionaryManager =
                Mock.Of<IDictionaryManager>(t => t.GetDictionaryItem<Scenario>(2) == scenario
                                              && t.GetDictionary<LicensedActivity>() == new List<LicensedActivity> { Mock.Of<LicensedActivity>() });

            var mapper = new DossierFileDataMapper(_stubTaskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   dictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.Scenario, scenario);
        }

        [Test]
        public void RetrieveInventoryTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.Id == 1);
            var inventory = Mock.Of<DocumentInventory>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DossierFile>(1) == file);

            var mapper = new DossierFileDataMapper(_stubTaskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   Mock.Of<IDomainDataMapper<DocumentInventory>>(t => t.Retrieve(1, db) == inventory),
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.DocumentInventory, inventory);
        }

        #endregion

        #region Save Tests

        [Test]
        public void SaveDossierFileTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.DossierFileStepList == new EditableList<DossierFileScenarioStep>());
            var db = new Mock<IDomainDbManager>();
            var taskMapper =
                Mock.Of<IDomainDataMapper<Task>>(t => t.Save(It.IsAny<Task>(), db.Object, false) == Mock.Of<Task>());
            var mapper = new DossierFileDataMapper(taskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<DossierFile>(t => t.Clone() == file));

            // Assert
            db.Verify(t => t.SaveDomainObject(file));
            Assert.AreEqual(file, result);
        }

        [Test]
        public void SaveTaskTest()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var savedTask = Mock.Of<Task>(t => t.Id == 100500);
            var file = Mock.Of<DossierFile>(t => t.DossierFileStepList == new EditableList<DossierFileScenarioStep>()
                                              && t.Task == task);
            var db = new Mock<IDomainDbManager>();
            var taskMapper =
                Mock.Of<IDomainDataMapper<Task>>(d => d.Save(task, db.Object, false) == savedTask);
            var mapper = new DossierFileDataMapper(taskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<DossierFile>(t => t.Clone() == file));

            // Assert
            Assert.AreEqual(result.TaskId, 100500);
            Assert.AreEqual(result.Task, savedTask);
        }

        [Test]
        public void SaveDossierFileServiceResult()
        {
            // Arrange
            var serviceResult = Mock.Of<DossierFileServiceResult>(t => t.Id == 100500);
            var file = Mock.Of<DossierFile>(t => t.DossierFileStepList == new EditableList<DossierFileScenarioStep>()
                                              && t.DossierFileServiceResult == serviceResult);

            var db = new Mock<IDomainDbManager>();

            var taskMapper =
                Mock.Of<IDomainDataMapper<Task>>(t => t.Save(It.IsAny<Task>(), db.Object, false) == Mock.Of<Task>());
            var mapper = new DossierFileDataMapper(taskMapper,
                                                   _stubFileStepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<DossierFile>(t => t.Clone() == file));

            // Assert
            db.Verify(t => t.SaveDomainObject(serviceResult), Times.Once());
            Assert.AreEqual(file.DossierFileServiceResultId, 100500);
        }

        [Test]
        public void SaveHolderRequisitesResult()
        {
            // Arrange
            var holderRequisites = Mock.Of<HolderRequisites>();
            var savedRequisites = Mock.Of<HolderRequisites>(t => t.Id == 100500);
            var file = Mock.Of<DossierFile>(t => t.DossierFileStepList == new EditableList<DossierFileScenarioStep>()
                                              && t.HolderRequisites == holderRequisites
                                              && t.LicenseDossier == Mock.Of<LicenseDossier>(l => l.LicenseHolderId == 500100));

            var db = Mock.Of<IDomainDbManager>();

            var taskMapper =
                Mock.Of<IDomainDataMapper<Task>>(t => t.Save(It.IsAny<Task>(), db, false) == Mock.Of<Task>());

            var requisitesMapper =
                Mock.Of<IDomainDataMapper<HolderRequisites>>(t => 
                    t.Save(It.Is<HolderRequisites>(h => h.LicenseHolderId == 500100 && h.Equals(holderRequisites)), db, false) == savedRequisites);

            var mapper = new DossierFileDataMapper(taskMapper,
                                                   _stubFileStepMapper,
                                                   requisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db));

            // Act
            var result = mapper.Save(Mock.Of<DossierFile>(t => t.Clone() == file));

            // Assert
            Assert.AreEqual(file.HolderRequisites, savedRequisites);
            Assert.AreEqual(file.HolderRequisitesId, 100500);
        }

        [Test]
        public void SaveFileScenarioStepListTest()
        {
            // Arrange
            var step = Mock.Of<DossierFileScenarioStep>();
            var savedStep = Mock.Of<DossierFileScenarioStep>();
            var file = Mock.Of<DossierFile>(t => t.Id == 100500 && t.DossierFileStepList == new EditableList<DossierFileScenarioStep> { step });

            var db = new Mock<IDomainDbManager>();
            var taskMapper =
                Mock.Of<IDomainDataMapper<Task>>(t => t.Save(It.IsAny<Task>(), db.Object, false) == Mock.Of<Task>());

            var stepMapper =
                Mock.Of<IDomainDataMapper<DossierFileScenarioStep>>(d => d.Save(It.Is<DossierFileScenarioStep>(t => t.DossierFileId == 100500 && t.Equals(step)), db.Object, false) == savedStep);

            var mapper = new DossierFileDataMapper(taskMapper,
                                                   stepMapper,
                                                   _stubRequisitesMapper,
                                                   _stubDossierMapper,
                                                   _stubEmployeeMapper,
                                                   _stubInventoryMapper,
                                                   _stubLicenseMapper,
                                                   _stubDictionaryManager,
                                                   new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<DossierFile>(t => t.Clone() == file));

            // Assert
            Assert.AreEqual(result.DossierFileStepList.Single(), savedStep);
        }

        #endregion
    }
}
