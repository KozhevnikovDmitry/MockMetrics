using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PostGrad.BL.DiActionContext.After;
using PostGrad.Core.BL;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.Tests.DiActionContext.After
{
    [TestFixture]
    public class ServiceResultGranterTests
    {
        [Test]
        public void GrandLightScenarioServiceResultTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.ScenarioId == 1 && t.ScenarioType == ScenarioType.Light);
            var serviceResult = Mock.Of<ServiceResult>(t => t.ScenarioId == 1 && t.Id == 2);
            var dictionaryManager = Mock.Of<ICacheRepository>(t => t.GetCache<ServiceResult>() == new List<ServiceResult> { serviceResult });
            var serviceResultGranter = new ServiceResultGranter();

            // Act
            var result = serviceResultGranter.GrantServiseResult(dossierFile, dictionaryManager);

            // Assert
            Assert.AreEqual(result, dossierFile.DossierFileServiceResult);
            Assert.AreEqual(result.ServiceResult, serviceResult);
            Assert.AreEqual(result.ServiceResultId, 2);
        }

        [Test]
        public void GrantLigntScenarioResultNotSingleResulstOnLightScenarioTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.ScenarioId == 1 && t.ScenarioType == ScenarioType.Light);
            var serviceResult = Mock.Of<ServiceResult>(t => t.ScenarioId == 1 && t.Id == 2);
            var dictionaryManager = Mock.Of<ICacheRepository>(t => t.GetCache<ServiceResult>() == new List<ServiceResult> { serviceResult, serviceResult });
            var serviceResultGranter = new ServiceResultGranter();
            
            // Assert
            Assert.Throws<NotSingleResulstOnLightScenarioException>(() => serviceResultGranter.GrantServiseResult(dossierFile, dictionaryManager));
        }

        [Test]
        public void GrandFullScenarioPositiveServiceResultTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.ScenarioId == 1 
                                                     && t.ScenarioType == ScenarioType.Full
                                                     && t.IsGrantingPositve);
            var positiveResult = Mock.Of<ServiceResult>(t => t.ScenarioId == 1 && t.Id == 2 && t.IsPositive);
            var negativeResult = Mock.Of<ServiceResult>(t => t.ScenarioId == 1 && t.Id == 3 && t.IsPositive == false);
            var dictionaryManager = Mock.Of<ICacheRepository>(t => t.GetCache<ServiceResult>()
                == new List<ServiceResult> { positiveResult, negativeResult });
            var serviceResultGranter = new ServiceResultGranter();

            // Act
            var result = serviceResultGranter.GrantServiseResult(dossierFile, dictionaryManager);

            // Assert
            Assert.AreEqual(result, dossierFile.DossierFileServiceResult);
            Assert.AreEqual(result.ServiceResult, positiveResult);
            Assert.AreEqual(result.ServiceResultId, 2);
        }

        [Test]
        public void GrandFullScenarioNegativeServiceResultTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.ScenarioId == 1
                                                     && t.ScenarioType == ScenarioType.Full
                                                     && t.IsGrantingPositve == false);
            var positiveResult = Mock.Of<ServiceResult>(t => t.ScenarioId == 1 && t.Id == 2 && t.IsPositive);
            var negativeResult = Mock.Of<ServiceResult>(t => t.ScenarioId == 1 && t.Id == 3 && t.IsPositive == false);
            var dictionaryManager = Mock.Of<ICacheRepository>(t => t.GetCache<ServiceResult>()
                == new List<ServiceResult> { positiveResult, negativeResult });
            var serviceResultGranter = new ServiceResultGranter();

            // Act
            var result = serviceResultGranter.GrantServiseResult(dossierFile, dictionaryManager);

            // Assert
            Assert.AreEqual(result, dossierFile.DossierFileServiceResult);
            Assert.AreEqual(result.ServiceResult, negativeResult);
            Assert.AreEqual(result.ServiceResultId, 3);
        }

        [Test]
        public void GrantFullScenarioResultNotSingleResulstOnLightScenarioTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.ScenarioId == 1
                                                     && t.ScenarioType == ScenarioType.Full
                                                     && t.IsGrantingPositve);
            var positiveResult = Mock.Of<ServiceResult>(t => t.ScenarioId == 1 && t.IsPositive);
            var dictionaryManager = Mock.Of<ICacheRepository>(t => t.GetCache<ServiceResult>()
                == new List<ServiceResult> { positiveResult, positiveResult });
            var serviceResultGranter = new ServiceResultGranter();

            // Assert
            Assert.Throws<NotSingleResulstOnLightScenarioException>(() => serviceResultGranter.GrantServiseResult(dossierFile, dictionaryManager));
        }
        
        [Test]
        public void SavePositiveServiceResultTest()
        {
            // Arrange
            var savedFile = Mock.Of<DossierFile>();
            var task = Mock.Of<Task>();
            var tmp = Mock.Of<DossierFile>(t => t.IsGrantingPositve && t.Task == task);
            var dossierFile = Mock.Of<DossierFile>(t => t.Clone() == tmp);
            var taskStatusPolicy = new Mock<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Save(tmp, false) == savedFile);
            var serviceResultGranter = new ServiceResultGranter();

            // Act
            var result = serviceResultGranter.SaveServiceResult(dossierFile, taskStatusPolicy.Object ,fileMapper);

            // Assert
            Assert.AreEqual(savedFile, result);
            taskStatusPolicy.Verify(t => t.SetStatus(TaskStatusType.Done, string.Empty, task), Times.Once);
        }

        [Test]
        public void SaveNegativeServiceResultTest()
        {
            // Arrange
            var savedFile = Mock.Of<DossierFile>();
            var tmp = Mock.Of<DossierFile>(t => t.IsGrantingPositve == false);
            var dossierFile = Mock.Of<DossierFile>(t => t.Clone() == tmp);

            var taskStatusPolicy = new Mock<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Save(tmp, false) == savedFile);
            var serviceResultGranter = new ServiceResultGranter();

            // Act
            var result = serviceResultGranter.SaveServiceResult(dossierFile, taskStatusPolicy.Object, fileMapper);

            // Assert
            Assert.AreEqual(savedFile, result);
            taskStatusPolicy.Verify(t => t.SetStatus(It.IsAny<TaskStatusType>(), It.IsAny<string>(), It.IsAny<Task>()), Times.Never);
        }
        
        [Test]
        public void GrantNewLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>();
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve);
            var licenseProvider = Mock.Of<ILicenseProvider>(t => t.GetNewLicense(dossierFile) == license);
            var serviceResultGranter = new ServiceResultGranter();

            // Act
            var newLicense = serviceResultGranter.GrantNewLicense(dossierFile, licenseProvider);

            // Assert
            Assert.AreEqual(license, newLicense);
            Assert.AreEqual(license, dossierFile.License);
        }

        [Test]
        public void GrantNewLicenseWrongStatusForGrantingTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve == false);
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var serviceResultGranter = new ServiceResultGranter();

            // Assert
            Assert.Throws<WrongStatusForGrantingException>(() => serviceResultGranter.GrantNewLicense(dossierFile, licenseProvider));
        }

        [Test]
        public void GrantStopLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>();
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve);
            var licenseProvider = Mock.Of<ILicenseProvider>(t => t.GetStopLicense(dossierFile) == license);
            var serviceResultGranter = new ServiceResultGranter();

            // Act
            var stoppedLicnse = serviceResultGranter.GrantStopLicense(dossierFile, licenseProvider);

            // Assert
            Assert.AreEqual(license, stoppedLicnse);
        }

        [Test]
        public void GrantStopLicenseWrongStatusForGrantingTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve == false);
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var serviceResultGranter = new ServiceResultGranter();

            // Assert
            Assert.Throws<WrongStatusForGrantingException>(() => serviceResultGranter.GrantStopLicense(dossierFile, licenseProvider));
        }

        [Test]
        public void GrantRenewalLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>();
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve);
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>(t => t.RenewalLicense(dossierFile) == license);
            var serviceResultGranter = new ServiceResultGranter();

            // Act
            var renewalLicense = serviceResultGranter.GrantRenewalLicense(dossierFile, licenseRenewaller);

            // Assert
            Assert.AreEqual(license, renewalLicense);
        }

        [Test]
        public void GrantRenewalLicenseWrongStatusForGrantingTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve == false);
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter();

            // Assert
            Assert.Throws<WrongStatusForGrantingException>(() => serviceResultGranter.GrantRenewalLicense(dossierFile, licenseRenewaller));
        }
    }
}
