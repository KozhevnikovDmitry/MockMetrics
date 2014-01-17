using Moq;
using NUnit.Framework;
using PostGrad.BL.DiActionContext.Before;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.Tests.DiActionContext.Before
{
    [TestFixture]
    public class SupervisionFacadeTests
    {
        [Test]
        public void GrantServiseResultTest()
        {
            // Arrange
            var serviceResult = Mock.Of<DossierFileServiceResult>();
            var dossierFile = Mock.Of<DossierFile>();
            var resultGranter = Mock.Of<IServiceResultGranter>(t => t.GrantServiseResult(dossierFile) == serviceResult);
            var supervisionFacade = new SupervisionFacade(dossierFile, resultGranter);

            // Act
            var result = supervisionFacade.GrantServiseResult();

            // Assert
            Assert.AreEqual(result, serviceResult);
        }

        [Test]
        public void SaveServiceResultTest()
        {
            // Arrange
            var savedFile = Mock.Of<DossierFile>();
            var dossierFile = Mock.Of<DossierFile>();
            var resultGranter = Mock.Of<IServiceResultGranter>(t => t.SaveServiceResult(dossierFile) == savedFile);
            var supervisionFacade = new SupervisionFacade(dossierFile, resultGranter);

            // Act
            supervisionFacade.SaveServiceResult();

            // Assert
            Assert.AreEqual(supervisionFacade.DossierFile, savedFile);
        }

        [Test]
        public void GrantNewLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>();
            var dossierFile = Mock.Of<DossierFile>();
            var resultGranter = Mock.Of<IServiceResultGranter>(t => t.GrantNewLicense(dossierFile) == license);
            var supervisionFacade = new SupervisionFacade(dossierFile, resultGranter);

            // Act
            var newLicense = supervisionFacade.GrantNewLicense();

            // Assert
            Assert.AreEqual(newLicense, license);
        }

        [Test]
        public void GrantRenewalLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>();
            var dossierFile = Mock.Of<DossierFile>();
            var resultGranter = Mock.Of<IServiceResultGranter>(t => t.GrantRenewalLicense(dossierFile) == license);
            var supervisionFacade = new SupervisionFacade(dossierFile, resultGranter);

            // Act
            var renewalLicense = supervisionFacade.GrantRenewalLicense();

            // Assert
            Assert.AreEqual(renewalLicense, license);
        }

        [Test]
        public void GrantStopLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>();
            var dossierFile = Mock.Of<DossierFile>();
            var resultGranter = Mock.Of<IServiceResultGranter>(t => t.GrantStopLicense(dossierFile) == license);
            var supervisionFacade = new SupervisionFacade(dossierFile, resultGranter);

            // Act
            var stopLicense = supervisionFacade.GrantStopLicense();

            // Assert
            Assert.AreEqual(stopLicense, license);
        }
    }
}
