using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PostGrad.BL.DiActionContext.Before;
using PostGrad.Core.BL;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.Tests.DiActionContext.Before
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
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager, 
                                                                taskStatusPolicy, 
                                                                fileMapper,
                                                                licenseProvider, 
                                                                licenseRenewaller);

            // Act
            var result = serviceResultGranter.GrantServiseResult(dossierFile);

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
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);
            
            // Assert
            Assert.Throws<NotSingleResulstOnLightScenarioException>(() => serviceResultGranter.GrantServiseResult(dossierFile));
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
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Act
            var result = serviceResultGranter.GrantServiseResult(dossierFile);

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
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Act
            var result = serviceResultGranter.GrantServiseResult(dossierFile);

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
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Assert
            Assert.Throws<NotSingleResulstOnLightScenarioException>(() => serviceResultGranter.GrantServiseResult(dossierFile));
        }
        
        [Test]
        public void SavePositiveServiceResultTest()
        {
            // Arrange
            var savedFile = Mock.Of<DossierFile>();
            var task = Mock.Of<Task>();
            var tmp = Mock.Of<DossierFile>(t => t.IsGrantingPositve && t.Task == task);
            var dossierFile = Mock.Of<DossierFile>(t => t.Clone() == tmp);
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = new Mock<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Save(tmp, false) == savedFile);
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy.Object,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Act
            var result = serviceResultGranter.SaveServiceResult(dossierFile);

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
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = new Mock<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Save(tmp, false) == savedFile);
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy.Object,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Act
            var result = serviceResultGranter.SaveServiceResult(dossierFile);

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
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>(t => t.GetNewLicense(dossierFile) == license);
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Act
            var newLicense = serviceResultGranter.GrantNewLicense(dossierFile);

            // Assert
            Assert.AreEqual(license, newLicense);
            Assert.AreEqual(license, dossierFile.License);
        }

        [Test]
        public void GrantNewLicenseWrongStatusForGrantingTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve == false);
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Assert
            Assert.Throws<WrongStatusForGrantingException>(() => serviceResultGranter.GrantNewLicense(dossierFile));
        }

        [Test]
        public void GrantStopLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>();
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve);
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>(t => t.GetStopLicense(dossierFile) == license);
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Act
            var stoppedLicnse = serviceResultGranter.GrantStopLicense(dossierFile);

            // Assert
            Assert.AreEqual(license, stoppedLicnse);
        }

        [Test]
        public void GrantStopLicenseWrongStatusForGrantingTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve == false);
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Assert
            Assert.Throws<WrongStatusForGrantingException>(() => serviceResultGranter.GrantStopLicense(dossierFile));
        }

        [Test]
        public void GrantRenewalLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>();
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve);
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>(t => t.RenewalLicense(dossierFile) == license);
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Act
            var renewalLicense = serviceResultGranter.GrantRenewalLicense(dossierFile);

            // Assert
            Assert.AreEqual(license, renewalLicense);
        }

        [Test]
        public void GrantRenewalLicenseWrongStatusForGrantingTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsGrantingPositve == false);
            var dictionaryManager = Mock.Of<ICacheRepository>();
            var taskStatusPolicy = Mock.Of<ITaskStatusPolicy>();
            var fileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            var licenseProvider = Mock.Of<ILicenseProvider>();
            var licenseRenewaller = Mock.Of<ILicenseRenewaller>();
            var serviceResultGranter = new ServiceResultGranter(dictionaryManager,
                                                                taskStatusPolicy,
                                                                fileMapper,
                                                                licenseProvider,
                                                                licenseRenewaller);

            // Assert
            Assert.Throws<WrongStatusForGrantingException>(() => serviceResultGranter.GrantRenewalLicense(dossierFile));
        }
    }
}
