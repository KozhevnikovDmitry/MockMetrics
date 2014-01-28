using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Dossier;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Linkage
{
    [TestFixture]
    public class SetupFileAddInTests
    {
        [Test]
        public void SortOrderTest()
        {
            // Arrange
            var addin = new SetupFileAddIn(Mock.Of<LicenseDossierRepository>());

            // Assert
            Assert.AreEqual(addin.SortOrder, 5);
        }

        [Test]
        public void NoLinkageForNewLicenseTaskTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseDossier == Mock.Of<LicenseDossier>());
            var addin = new SetupFileAddIn(Mock.Of<LicenseDossierRepository>());

            // Act
            addin.Linkage(fileLink, Mock.Of<IDomainDbManager>());

            // Assert
            Assert.IsNull(fileLink.License);
        }

        [Test]
        public void SetupDossierFileTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense);
            var dossier = Mock.Of<LicenseDossier>();
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseDossier == dossier);
            var dossierRegistr = Mock.Of<LicenseDossierRepository>(t => t.GetNextFileRegNumber(dossier, db) == 100500);
            var addin = new SetupFileAddIn(dossierRegistr);

            // Act
            addin.Linkage(fileLink, db);

            // Assert
            Assert.AreEqual(fileLink.DossierFile.RegNumber, 100500);
            Assert.AreEqual(fileLink.DossierFile.CurrentStatus, DossierFileStatus.Active);
        }
    }
}