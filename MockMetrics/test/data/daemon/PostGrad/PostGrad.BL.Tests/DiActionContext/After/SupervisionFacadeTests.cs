using System;
using Moq;
using NUnit.Framework;
using PostGrad.BL.DiActionContext.After;
using PostGrad.Core.BL;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.Tests.DiActionContext.After
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
            var resultGranter = Mock.Of<IServiceResultGranter>();
            var diActionContext = Mock.Of<IDiActionContext>(t => t.Get(It.IsAny<Func<ICacheRepository, DossierFileServiceResult>>()) == serviceResult);
            var supervisionFacade = new SupervisionFacade(dossierFile, diActionContext, resultGranter);

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
            var resultGranter = Mock.Of<IServiceResultGranter>();
            var diActionContext = Mock.Of<IDiActionContext>(t => t.Get(It.IsAny<Func<ITaskStatusPolicy, IDomainDataMapper<DossierFile>, DossierFile>>()) == savedFile);
            var supervisionFacade = new SupervisionFacade(dossierFile, diActionContext, resultGranter);

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
            var resultGranter = Mock.Of<IServiceResultGranter>();
            var diActionContext =
                Mock.Of<IDiActionContext>(
                    t =>
                        t.Get(It.IsAny<Func<ILicenseProvider, License>>()) == license);
            var supervisionFacade = new SupervisionFacade(dossierFile, diActionContext, resultGranter);

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
            var resultGranter = Mock.Of<IServiceResultGranter>();
            var diActionContext =
                Mock.Of<IDiActionContext>(
                    t =>
                        t.Get(It.IsAny<Func<ILicenseRenewaller, License>>()) == license);
            var supervisionFacade = new SupervisionFacade(dossierFile, diActionContext, resultGranter);

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
            var resultGranter = Mock.Of<IServiceResultGranter>();
            var diActionContext =
                Mock.Of<IDiActionContext>(
                    t =>
                        t.Get(It.IsAny<Func<ILicenseProvider, License>>()) == license);
            var supervisionFacade = new SupervisionFacade(dossierFile, diActionContext, resultGranter);

            // Act
            var stopLicense = supervisionFacade.GrantStopLicense();

            // Assert
            Assert.AreEqual(stopLicense, license);
        }
    }
}
