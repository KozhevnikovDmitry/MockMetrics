using System;
using System.Linq;
using BLToolkit.EditableObjects;
using Moq;
using NUnit.Framework;
using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Holder;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.Tests.AddInList.Before
{
    [TestFixture]
    public class LinkagerTests
    {
        #region Linkage Holder Tests
        
        [Test]
        public void LinkageExistingHolderTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) == false);
            var holder = Mock.Of<LicenseHolder>(t => t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.LicensedActivity.Id == 3);
            var holderInfo = new HolderInfo { Inn = "1", Ogrn = "2" };
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == holderInfo);
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists("1", "2", db)
                                                                    && t.GetLicenseHolder("1", "2", db) == holder);

            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileLink.LicenseHolder, holder);
        }

        [Test]
        public void LinkageNewHolderTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) == false);
            var holder = Mock.Of<LicenseHolder>(t => t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.LicensedActivity.Id == 3);
            var holderInfo = new HolderInfo();
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == holderInfo
                                                               && t.ParseHolder(task) == holder);
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db) == false);

            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileLink.LicenseHolder, holder);
        }

        [Test]
        public void NoExistingHolderForLightSceanrio()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db));
            var holder = Mock.Of<LicenseHolder>(t => t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.ScenarioType == ScenarioType.Light && t.LicensedActivity.Id == 3);
            var holderInfo = new HolderInfo();
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == holderInfo
                                                               && t.ParseHolder(task) == holder);
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db) == false);

            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Assert
            Assert.Throws<NoExistingHolderForLightSceanrioException>(() => linkager.Linkage(file));
        }

        #endregion

        #region Linkage Requisites Tests
        
        [Test]
        public void SetupAvailableRequisitesForNewHolder()
        {
            // Arrange
            var taskReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today);
            var holder =
                Mock.Of<LicenseHolder>(
                    h => h.RequisitesList == new EditableList<HolderRequisites> { taskReqs });

            var db = Mock.Of<IDomainDbManager>();
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db));
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var task = Mock.Of<Task>();
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == Mock.Of<HolderInfo>());
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.LicensedActivity.Id == 3 && t.IsNewLicense);
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Value, fileLink.SelectedRequisites);
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Value, taskReqs);
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Key, RequisitesOrigin.FromTask);
        }

        [Test]
        public void SetupAvailableRequisitesForFullScenario()
        {
            // Arrange
            var taskReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today);
            var holderReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today.AddDays(1));
            var holder =
                Mock.Of<LicenseHolder>(
                    h => h.Id == 1 && h.RequisitesList == new EditableList<HolderRequisites> { taskReqs, holderReqs });

            var db = Mock.Of<IDomainDbManager>();
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db));
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.LicensedActivity.Id == 3 && t.IsNewLicense);
            var taskParser =
                Mock.Of<ITaskParser>(
                    t => t.ParseHolderInfo(task) == Mock.Of<HolderInfo>() &&
                    t.ParseHolder(task) ==
                    Mock.Of<LicenseHolder>(h => h.RequisitesList == new EditableList<HolderRequisites> { taskReqs }));

            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileLink.AvailableRequisites.First().Value, fileLink.SelectedRequisites);
            Assert.AreEqual(fileLink.AvailableRequisites[RequisitesOrigin.FromRegistr], holderReqs);
            Assert.AreEqual(fileLink.AvailableRequisites[RequisitesOrigin.FromTask], taskReqs);
        }

        [Test]
        public void SetupAvailableRequisitesForLightScenario()
        {
            // Arrange
            var taskReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today);
            var holderReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today.AddDays(1));
            var holder =
                Mock.Of<LicenseHolder>(
                    h => h.Id == 1 && h.RequisitesList == new EditableList<HolderRequisites> { taskReqs, holderReqs });
            var db = Mock.Of<IDomainDbManager>();
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db));
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var task = Mock.Of<Task>();
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == Mock.Of<HolderInfo>());
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.ScenarioType == ScenarioType.Light && t.LicensedActivity.Id == 3 && t.IsNewLicense);

            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Value, fileLink.SelectedRequisites);
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Value, holderReqs);
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Key, RequisitesOrigin.FromRegistr);
        }

        [Test]
        public void AlwaysGetTheSameAvailableRequisistes()
        {
            // Arrange
            var taskReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today);
            var holder =
                Mock.Of<LicenseHolder>(
                    h => h.Id == 1 && h.RequisitesList == new EditableList<HolderRequisites> { taskReqs });
            var db = Mock.Of<IDomainDbManager>();
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db));
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.LicensedActivity.Id == 3 && t.IsNewLicense);

            var taskParser = new Mock<ITaskParser>();
            taskParser.Setup(t => t.ParseHolder(task)).Returns(holder);
            taskParser.Setup(t => t.ParseHolderInfo(task)).Returns(Mock.Of<HolderInfo>());

            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser.Object);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            taskParser.Verify(t => t.ParseHolder(task), Times.Once());
        }

        #endregion

        #region Linkage Dossier Tests

        [Test]
        public void LinkageExistingDossierTest()
        {
            // Arrange
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2 && t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var db = Mock.Of<IDomainDbManager>();
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder); 
            var task = Mock.Of<Task>();
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == Mock.Of<HolderInfo>() 
                                                    && t.ParseHolder(task) == Mock.Of<LicenseHolder>(h => h.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() }));
            var file = Mock.Of<DossierFile>(t => t.LicensedActivity == Mock.Of<LicensedActivity>(l => l.Id == 1) && t.Task == task);
            var dossier = Mock.Of<LicenseDossier>();
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(1, 2, db) && t.GetLicenseDossier(1, 2, db) == dossier);
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileLink.LicenseDossier, dossier);
        }

        [Test]
        public void LinkageNewDossierTest()
        {
            // Arrange
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2 && t.Inn == "500100" && t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var db = Mock.Of<IDomainDbManager>();
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var task = Mock.Of<Task>();
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == Mock.Of<HolderInfo>() &&
                                                       t.ParseHolder(task) == Mock.Of<LicenseHolder>(h => h.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() }));
            var activity = Mock.Of<LicensedActivity>(l => l.Id == 1 && l.Code == "100500");
            var file = Mock.Of<DossierFile>(t => t.LicensedActivity == activity && t.Task == task);
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) == false);
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileLink.LicenseDossier.PersistentState, PersistentState.New);
            Assert.AreEqual(fileLink.LicenseDossier.Id, 0);
            Assert.AreEqual(fileLink.LicenseDossier.LicensedActivity, activity);
            Assert.AreEqual(fileLink.LicenseDossier.LicensedActivityId, 1);
            Assert.AreEqual(fileLink.LicenseDossier.LicenseHolder, holder);
            Assert.AreEqual(fileLink.LicenseDossier.LicenseHolderId, 2);
            Assert.That(fileLink.LicenseDossier.IsActive);
            Assert.AreEqual(fileLink.LicenseDossier.CreateDate, DateTime.Today);
            Assert.IsEmpty(fileLink.LicenseDossier.LicenseList);
            Assert.AreEqual(fileLink.LicenseDossier.RegNumber, "ЛО-24-100500-500100");
        }

        [Test]
        public void NoExistingDossierForLightSceanrio()
        {
            // Arrange
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2 && t.Inn == "500100" && t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var db = Mock.Of<IDomainDbManager>();
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var task = Mock.Of<Task>();
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == Mock.Of<HolderInfo>() &&
                                                       t.ParseHolder(task) == Mock.Of<LicenseHolder>(h => h.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() }));
            var activity = Mock.Of<LicensedActivity>(l => l.Id == 1 && l.Code == "100500");
            var file = Mock.Of<DossierFile>(t => t.LicensedActivity == activity && t.ScenarioType == ScenarioType.Light && t.Task == task);
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) == false);
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Assert
            Assert.Throws<NoExistingDossierForLightSceanrioException>(() => linkager.Linkage(file));
        }

        #endregion

        #region Linkage License Tests
        
        [Test]
        public void NoLinkageForNewLicenseTaskTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>(); 
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2 && t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var dossierRepository = Mock.Of<ILicenseDossierRepository>();
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(It.IsAny<Task>()) == Mock.Of<HolderInfo>()
                                                    && t.ParseHolder(It.IsAny<Task>()) == Mock.Of<LicenseHolder>(h => h.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() }));
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense && t.LicensedActivity.Id == 3);
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.IsNull(fileLink.License);
        }

        [Test]
        public void LinkageLicenseTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2 && t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var license = Mock.Of<License>(t => t.RegNumber == "цщ-100500" && t.GrantDate == DateTime.Today);
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseList == new EditableList<License>
            {
                Mock.Of<License>(l => l.RegNumber == "100501" && l.GrantDate == DateTime.Today),
                Mock.Of<License>(l => l.RegNumber == "100500" && l.GrantDate == DateTime.Today.AddDays(1)),
                license
            });
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) && t.GetLicenseDossier(It.IsAny<int>(), It.IsAny<int>(), db) == dossier);
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense == false && t.Task == task && t.LicensedActivity.Id == 3);
            var licenseInfo = Mock.Of<LicenseInfo>(t => t.RegNumber == "ЦЩ-100500" && t.GrantDate == DateTime.Today);
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseLicenseInfo(task) == licenseInfo && t.ParseHolderInfo(task) == Mock.Of<HolderInfo>()
                                                    && t.ParseHolder(task) == Mock.Of<LicenseHolder>(h => h.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() }));
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileLink.License, license);
        }

        [Test]
        public void LinkageNoLicenseTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2 && t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseList == new EditableList<License>
            {
                Mock.Of<License>(l => l.RegNumber == "100501" && l.GrantDate == DateTime.Today),
                Mock.Of<License>(l => l.RegNumber == "100500" && l.GrantDate == DateTime.Today.AddDays(1))
            });
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) && t.GetLicenseDossier(It.IsAny<int>(), It.IsAny<int>(), db) == dossier);
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense == false && t.Task == task && t.LicensedActivity.Id == 3);
            var licenseInfo = Mock.Of<LicenseInfo>(t => t.RegNumber == "ЦЩ-100500" && t.GrantDate == DateTime.Today);
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseLicenseInfo(task) == licenseInfo && t.ParseHolderInfo(task) == Mock.Of<HolderInfo>()
                                                    && t.ParseHolder(task) == Mock.Of<LicenseHolder>(h => h.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() }));
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.IsNull(fileLink.License);
        }

        [Test]
        public void LinkageLicenseTooMoreLicensesTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2 && t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseList == new EditableList<License>
            {
                Mock.Of<License>(l => l.RegNumber == "ЦЩ-100500" && l.GrantDate == DateTime.Today),
                Mock.Of<License>(l => l.RegNumber == "ЦЩ-100500" && l.GrantDate == DateTime.Today)
            });
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) && t.GetLicenseDossier(It.IsAny<int>(), It.IsAny<int>(), db) == dossier);
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense == false && t.Task == task && t.LicensedActivity.Id == 3 && t.ScenarioType == ScenarioType.Light);
            var licenseInfo = Mock.Of<LicenseInfo>(t => t.RegNumber == "ЦЩ-100500" && t.GrantDate == DateTime.Today);
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseLicenseInfo(task) == licenseInfo && t.ParseHolderInfo(task) == Mock.Of<HolderInfo>());
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Assert
            Assert.Throws<TooMoreLinkagingLicesensesException>(() => linkager.Linkage(file));
        }

        #endregion

        #region Check Holder Data Tests

        [TestCase("1", "2", "1", "2", Result = false)]
        [TestCase("1", "2", "100500", "2", Result = true)]
        [TestCase("1", "2", "1", "100500", Result = true)]
        public bool GetIsHolderDataDoubtfullTest(string taskInn, string taskOgrn, string regInn, string regOgrn)
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var holder = Mock.Of<LicenseHolder>(t => t.Ogrn == regOgrn && t.Inn == regInn && t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);
            var dossierRepository = Mock.Of<ILicenseDossierRepository>();
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.IsNewLicense && t.LicensedActivity.Id == 3);
            var holderInfo = new HolderInfo { Inn = taskInn, Ogrn = taskOgrn };
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == holderInfo);
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            return fileLink.IsHolderDataDoubtfull;
        }

        #endregion

        #region Setup Holder Data Tests

        [Test]
        public void SetupDossierFileTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var holder = Mock.Of<LicenseHolder>(t => t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var holderRepository = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db)
                                                                       && t.GetLicenseHolder(It.IsAny<string>(), It.IsAny<string>(), db) == holder);

            var dossier = Mock.Of<LicenseDossier>();
            var dossierRepository = Mock.Of<ILicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) 
                                                                        && t.GetLicenseDossier(It.IsAny<int>(), It.IsAny<int>(), db) == dossier 
                                                                        && t.GetNextFileRegNumber(dossier, db) == 100500);
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.IsNewLicense && t.LicensedActivity.Id == 3);
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == Mock.Of<HolderInfo>());
            var linkager = new BL.AddInList.Before.Linkager(() => db, holderRepository, dossierRepository, taskParser);

            // Act
            var fileLink = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileLink.DossierFile.RegNumber, 100500);
            Assert.AreEqual(fileLink.DossierFile.CurrentStatus, DossierFileStatus.Active);
        }

        #endregion
    }
}