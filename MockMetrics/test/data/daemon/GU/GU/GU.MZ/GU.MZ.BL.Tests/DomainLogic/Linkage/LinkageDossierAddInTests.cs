using System;
using Common.DA;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Linkage
{
    [TestFixture]
    public class LinkageDossierAddInTests
    {
        [Test]
        public void SortOrderTest()
        {
            // Arrange
            var addin = new LinkageDossierAddIn( Mock.Of<LicenseDossierRepository>());

            // Assert
            Assert.AreEqual(addin.SortOrder, 2);
        }

        [Test]
        public void LinkageExistingDossierTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var file = Mock.Of<DossierFile>(t => t.LicensedActivity == Mock.Of<LicensedActivity>(l => l.Id == 1));
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2);
            var dossier = Mock.Of<LicenseDossier>();
            var dossierRegistr = Mock.Of<LicenseDossierRepository>(t => t.DossierExists(1, 2, db) && t.GetLicenseDossier(1, 2, db) == dossier);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseHolder == holder);
            var addin = new LinkageDossierAddIn(dossierRegistr);

            // Act
            addin.Linkage(fileLink, db);

            // Assert
            Assert.AreEqual(fileLink.LicenseDossier, dossier);
        }

        [Test]
        public void LinkageNewDossierTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var activity = Mock.Of<LicensedActivity>(l => l.Id == 1 && l.Code == "100500");
            var file = Mock.Of<DossierFile>(t => t.LicensedActivity == activity);
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2 && t.Inn == "500100");
            var dossierRegistr = Mock.Of<LicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) == false);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseHolder == holder);
            var addin = new LinkageDossierAddIn(dossierRegistr);

            // Act
            addin.Linkage(fileLink, db);

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
            var db = Mock.Of<IDomainDbManager>();
            var activity = Mock.Of<LicensedActivity>(l => l.Id == 1 && l.Code == "100500");
            var file = Mock.Of<DossierFile>(t => t.LicensedActivity == activity && t.ScenarioType == ScenarioType.Light);
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 2 && t.Inn == "500100");
            var dossierRegistr = Mock.Of<LicenseDossierRepository>(t => t.DossierExists(It.IsAny<int>(), It.IsAny<int>(), db) == false);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseHolder == holder);
            var addin = new LinkageDossierAddIn(dossierRegistr);

            // Assert
            Assert.Throws<NoExistingDossierForLightSceanrioException>(() => addin.Linkage(fileLink, db));
        }
    }
}