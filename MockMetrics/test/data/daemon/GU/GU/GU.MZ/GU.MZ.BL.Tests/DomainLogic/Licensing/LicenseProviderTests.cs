using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.Licensing;
using GU.MZ.BL.DomainLogic.Licensing.Renewal;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Licensing
{
    [TestFixture]
    public class LicenseProviderTests
    {
        [Test]
        public void GetNewLicenseOnlyFromTaskForNewLicenseTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsNewLicense == false);
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(), Mock.Of<ILicenseRenewaller>());
            
            // Assert
            Assert.Throws<WrongTaskServiceException>(() => licenseProvider.GetNewLicense(dossierFile));
        }

        [Test]
        public void GetNewLicenseFileNotLinkagedToDossierExceptionTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t =>
                t.IsNewLicense &&
                t.IsDone &&
                t.LicenseDossierId == null);

            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(), Mock.Of<ILicenseRenewaller>());

            // Assert
            Assert.Throws<FileNotLinkagedToDossierException>(() => licenseProvider.GetNewLicense(dossierFile));
        }

        [Test]
        public void GetNewLicenseNotNullTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => 
                t.IsNewLicense &&
                t.IsDone &&
                t.LicenseDossierId == 1 &&
                t.GetStandartOrderOfType(StandartOrderType.GrantLicense) == Mock.Of<StandartOrder>() &&
                t.LicensedActivity == Mock.Of<LicensedActivity>() &&
                t.HolderRequisites == Mock.Of<HolderRequisites>());
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(t => t.GetNewLicenseObjects(It.IsAny<Task>()) == new EditableList<LicenseObject>()),
                                                      Mock.Of<ILicenseRenewaller>());

            // Act
            var license = licenseProvider.GetNewLicense(dossierFile);

            // Assert
            Assert.NotNull(license);
        }

        [Test]
        public void GetNewLicenseDataFromDossierFileTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.Id == 1);
            var dossierFile = Mock.Of<DossierFile>(t => 
                t.IsNewLicense &&
                t.IsDone &&
                t.LicenseDossierId == 1 &&
                t.GetStandartOrderOfType(StandartOrderType.GrantLicense) == Mock.Of<StandartOrder>() &&
                t.LicensedActivity == licensedActivity &&
                t.HolderRequisites == Mock.Of<HolderRequisites>());
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(t => t.GetNewLicenseObjects(It.IsAny<Task>()) == new EditableList<LicenseObject>()),
                                                      Mock.Of<ILicenseRenewaller>());

            // Act
            var license = licenseProvider.GetNewLicense(dossierFile);

            // Assert
            Assert.AreEqual(license.Id, 0);
            Assert.AreEqual(license.LicensedActivity, licensedActivity);
            Assert.AreEqual(license.LicensedActivityId, licensedActivity.Id);
            Assert.AreEqual(license.LicenseDossierId, 1);
        }

        [Test]
        public void GetNewLicenseRequisitesFromHolderRequisitesTest()
        {
            // Arrange
            var licenseRequisites = Mock.Of<LicenseRequisites>();
            var holderRequisites = Mock.Of<HolderRequisites>(t => t.ToLicenseRequisites() == licenseRequisites);
            var dossierFile = Mock.Of<DossierFile>(t =>
                t.IsNewLicense &&
                t.IsDone &&
                t.LicenseDossierId == 1 &&
                t.GetStandartOrderOfType(StandartOrderType.GrantLicense) == Mock.Of<StandartOrder>() &&
                t.LicensedActivity == Mock.Of<LicensedActivity>() &&
                t.HolderRequisites == holderRequisites);
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(t => t.GetNewLicenseObjects(It.IsAny<Task>()) == new EditableList<LicenseObject>()),
                                                      Mock.Of<ILicenseRenewaller>());

            // Act
            var license = licenseProvider.GetNewLicense(dossierFile);

            // Assert
            Assert.AreEqual(license.LicenseRequisitesList.Single(), licenseRequisites);
            Assert.AreEqual(license.ActualRequisites, licenseRequisites);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false, ExpectedException = typeof(FileNotReadyToGrantLicenseException))]
        public void GetNewLicenseConsideringTaskStatusTest(bool isReady, bool isDone)
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => 
                t.IsNewLicense &&
                t.IsDone == isDone &&
                t.LicenseDossierId == 1 &&
                t.IsReady == isReady &&
                t.GetStandartOrderOfType(StandartOrderType.GrantLicense) == Mock.Of<StandartOrder>() &&
                t.LicensedActivity == Mock.Of<LicensedActivity>() &&
                t.HolderRequisites == Mock.Of<HolderRequisites>());
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(t => t.GetNewLicenseObjects(It.IsAny<Task>()) == new EditableList<LicenseObject>()),
                                                      Mock.Of<ILicenseRenewaller>());

            // Act
            var license = licenseProvider.GetNewLicense(dossierFile);

            // Assert
            Assert.NotNull(license);
        }

        [Test]
        public void GetNewLicenseSetLicensiarPositionAndNameTest()
        {
            // Arrange
            var licensedActivity = Mock.Of<LicensedActivity>(t => t.Id == 1);
            var dossierFile = Mock.Of<DossierFile>(t => 
                t.IsNewLicense &&
                t.IsDone &&
                t.LicenseDossierId == 1 &&
                t.GetStandartOrderOfType(StandartOrderType.GrantLicense) == Mock.Of<StandartOrder>() &&
                t.LicensedActivity == licensedActivity &&
                t.HolderRequisites == Mock.Of<HolderRequisites>());
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(t => t.GetNewLicenseObjects(It.IsAny<Task>()) == new EditableList<LicenseObject>()),
                                                     Mock.Of<ILicenseRenewaller>());

            // Act
            var license = licenseProvider.GetNewLicense(dossierFile);

            // Assert
            Assert.AreEqual(license.LicensiarHeadName, "В.Н. Янин");
            Assert.AreEqual(license.LicensiarHeadPosition, "Министр здравоохранения Красноярского края");
        }

        [Test]
        public void GetNewLicenseSetLicenseGrantOrderInfoTest()
        {
            // Arrange
            var order = Mock.Of<StandartOrder>(t => t.Stamp == DateTime.Today && t.RegNumber == "100500");
            var dossierFile = Mock.Of<DossierFile>(t => 
                t.IsNewLicense &&
                t.IsDone &&
                t.LicenseDossierId == 1 &&
                t.LicensedActivity == Mock.Of<LicensedActivity>() &&
                t.GetStandartOrderOfType(StandartOrderType.GrantLicense) == order &&
                t.HolderRequisites == Mock.Of<HolderRequisites>());
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(t => t.GetNewLicenseObjects(It.IsAny<Task>()) == new EditableList<LicenseObject>()),
                                                      Mock.Of<ILicenseRenewaller>());

            // Act
            var license = licenseProvider.GetNewLicense(dossierFile);

            // Assert
            Assert.AreEqual(license.GrantOrderRegNumber, "100500");
            Assert.AreEqual(license.GrantOrderStamp, DateTime.Today);
        }

        [Test]
        public void GetNewLicenseSetLicenseObjectsGrantOrderInfoTest()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var order = Mock.Of<StandartOrder>(t => t.Stamp == DateTime.Today && t.RegNumber == "100500");
            var dossierFile = Mock.Of<DossierFile>(t =>
                t.Task == task &&
                t.IsNewLicense &&
                t.IsDone &&
                t.LicenseDossierId == 1 &&
                t.LicensedActivity == Mock.Of<LicensedActivity>() &&
                t.GetStandartOrderOfType(StandartOrderType.GrantLicense) == order &&
                t.HolderRequisites == Mock.Of<HolderRequisites>());
            var licenseObject = Mock.Of<LicenseObject>();
            var licenseObjectProvider = Mock.Of<ILicenseObjectProvider>(t => t.GetNewLicenseObjects(task) == new EditableList<LicenseObject> { licenseObject });
            var licenseProvider = new LicenseProvider(licenseObjectProvider,
                                                      Mock.Of<ILicenseRenewaller>());

            // Act
            var license = licenseProvider.GetNewLicense(dossierFile);

            // Assert
            Assert.AreEqual(license.LicenseObjectList.Single().GrantOrderRegNumber, "100500");
            Assert.AreEqual(license.LicenseObjectList.Single().GrantOrderStamp, DateTime.Today);
        }

        [Test]
        public void GetNewLicenseNoGrantOrderTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t =>
                t.IsNewLicense &&
                t.IsDone &&
                t.LicenseDossierId == 1 &&
                t.LicensedActivity == Mock.Of<LicensedActivity>() &&
                t.HolderRequisites == Mock.Of<HolderRequisites>());
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(),
                                                       Mock.Of<ILicenseRenewaller>());

            // Assert
            Assert.Throws<NoGrantOrderException>(() => licenseProvider.GetNewLicense(dossierFile));
        }

        [Test]
        public void GetNewLicenseImportLicenseObjectsTest()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var dossierFile = Mock.Of<DossierFile>(t =>
                t.Task == task &&
                t.IsNewLicense &&
                t.IsDone &&
                t.LicenseDossierId == 1 &&
                t.LicensedActivity == Mock.Of<LicensedActivity>() &&
                t.GetStandartOrderOfType(StandartOrderType.GrantLicense) == Mock.Of<StandartOrder>() &&
                t.HolderRequisites == Mock.Of<HolderRequisites>());
            var licenseObject = Mock.Of<LicenseObject>();
            var licenseObjectProvider = Mock.Of<ILicenseObjectProvider>(t => t.GetNewLicenseObjects(task) == new EditableList<LicenseObject> { licenseObject });
            var licenseProvider = new LicenseProvider(licenseObjectProvider,
                                                      Mock.Of<ILicenseRenewaller>());

            // Act
            var license = licenseProvider.GetNewLicense(dossierFile);

            // Assert
            Assert.AreEqual(license.LicenseObjectList.Single(), licenseObject);
        }

        [Test]
        public void StopLicenseOnlyForStopLicenseTaskTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsStopLicense == false);
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(),
                                                      Mock.Of<ILicenseRenewaller>());

            // Assert
            Assert.Throws<WrongTaskServiceException>(() => licenseProvider.GetStopLicense(dossierFile));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false, ExpectedException = typeof(FileNotReadyToGrantLicenseException))]
        public void GetStopLicenseConsideringTaskStatusTest(bool isReady, bool isDone)
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t =>
                t.IsStopLicense &&
                t.IsDone == isDone &&
                t.IsReady == isReady &&
                t.License == Mock.Of<License>());
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(),
                                                       Mock.Of<ILicenseRenewaller>());

            // Act
            var license = licenseProvider.GetStopLicense(dossierFile);

            // Assert
            Assert.AreEqual(dossierFile.License, license);
        }

        [Test]
        public void GetStopLicenseAddStopStatus()
        {
            // Arrange
            var license = new Mock<License>();
            var dossierFile = Mock.Of<DossierFile>(t =>
                t.IsStopLicense &&
                t.IsDone == true &&
                t.License == license.Object);
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(),
                                                      Mock.Of<ILicenseRenewaller>());

            // Act
            licenseProvider.GetStopLicense(dossierFile);

            // Assert
            license.Verify(t => t.AddStatus(LicenseStatusType.Stop, It.Is<DateTime>(d => d.Date == DateTime.Now.Date), string.Empty), Times.Once);
        }

        [Test]
        public void GetStopLicenseFileNotLinkagedToLicenseTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t =>
                t.IsStopLicense &&
                t.IsDone &&
                t.IsReady);
            var licenseProvider = new LicenseProvider(Mock.Of<ILicenseObjectProvider>(),
                                                       Mock.Of<ILicenseRenewaller>());

            // Assert
            Assert.Throws<FileNotLinkagedToLicenseException>(() => licenseProvider.GetStopLicense(dossierFile));
        }
    }
}