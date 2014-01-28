using GU.DataModel;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Licensing;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Приёмочные тесты на заведение результата предоставления услуги в томе
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class GrantResultTests : SupervisionFixture
    {
        #region Grant New
        
        /// <summary>
        /// Тест на переход к этапу предоставления лицензии
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void StepToGrantLicenseTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActExpertiseOrder();
            ActSupervisionStepNext();
            ActProvideExpertise();
            ActSupervisionStepNext();
            ActInspectionOrder();
            ActSupervisionStepNext();
            ActProvideInspection();
            ActSupervisionStepNext();
            ActGrantLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Ready);
            AssertStep(dossierFile, 10);
            AssertPersistence(dossierFile);
        }
        
        /// <summary>
        /// Тест на переход к этапу отказ в предоставлении лицении
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void StepToNotGrantLicenseTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActExpertiseOrder();
            ActSupervisionStepNext();
            ActProvideExpertise();
            ActSupervisionStepNext();
            ActInspectionOrder();
            ActSupervisionStepNext();
            ActProvideInspection();
            ActSupervisionStepNext();
            ActNotGrantLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Rejected);
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Rejected);
            AssertStep(dossierFile, 10);
            AssertPersistence(dossierFile);
        }
        
        /// <summary>
        /// Тест на предоставление лицензии
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void GrantLicenseTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActExpertiseOrder();
            ActSupervisionStepNext();
            ActProvideExpertise();
            ActSupervisionStepNext();
            ActInspectionOrder();
            ActSupervisionStepNext();
            ActProvideInspection();
            ActSupervisionStepNext();
            ActGrantLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            ActGrantServiceResult();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Done);
            Assert.NotNull(dossierFile.DossierFileServiceResult,
                "Assert that there is a file serviceresult");

            var fileResult = dossierFile.DossierFileServiceResult;
            Assert.True(DictionaryManager.GetDictionaryItem<ServiceResult>(fileResult.ServiceResultId).IsPositive,
                "Assert that result is positive");
        }

        /// <summary>
        /// Тест на отказ в предоставлении лицензии
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void NotGrantLicenseTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActExpertiseOrder();
            ActSupervisionStepNext();
            ActProvideExpertise();
            ActSupervisionStepNext();
            ActInspectionOrder();
            ActSupervisionStepNext();
            ActProvideInspection();
            ActSupervisionStepNext();
            ActNotGrantLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Rejected);
            ActGrantServiceResult();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Rejected);
            Assert.NotNull(dossierFile.DossierFileServiceResult,
                "Assert that there is a file serviceresult");

            var fileResult = dossierFile.DossierFileServiceResult;
            Assert.False(DictionaryManager.GetDictionaryItem<ServiceResult>(fileResult.ServiceResultId).IsPositive,
                "Assert that result is positive");
        }
        
        #endregion


        #region Renewal

        /// <summary>
        /// Тест на переход к этапу переоформление лицензии
        /// </summary>
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(12)]
        public void StepToRenewalLicenseTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActExpertiseOrder();
            ActSupervisionStepNext();
            ActProvideExpertise();
            ActSupervisionStepNext();
            ActInspectionOrder();
            ActSupervisionStepNext();
            ActProvideInspection();
            ActSupervisionStepNext();
            ActGrantLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Ready);
            AssertStep(dossierFile, 10);
            AssertPersistence(dossierFile);
        }

        /// <summary>
        /// Тест на переход к этапу отказ в переоформлении лицении
        /// </summary>
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(12)]
        public void StepToNotRenewalLicenseTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActExpertiseOrder();
            ActSupervisionStepNext();
            ActProvideExpertise();
            ActSupervisionStepNext();
            ActInspectionOrder();
            ActSupervisionStepNext();
            ActProvideInspection();
            ActSupervisionStepNext();
            ActNotGrantLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Rejected);
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Rejected);
            AssertStep(dossierFile, 10);
            AssertPersistence(dossierFile);
        }

        /// <summary>
        /// Тест на переоформление лицензии
        /// </summary>
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(12)]
        public void RenewalLicenseTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActExpertiseOrder();
            ActSupervisionStepNext();
            ActProvideExpertise();
            ActSupervisionStepNext();
            ActInspectionOrder();
            ActSupervisionStepNext();
            ActProvideInspection();
            ActSupervisionStepNext();
            ActRenewalLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            ActGrantServiceResult();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Done);
            Assert.NotNull(dossierFile.DossierFileServiceResult,
                "Assert that there is a file serviceresult");

            //assert file service result
            var fileResult = dossierFile.DossierFileServiceResult;
            Assert.True(DictionaryManager.GetDictionaryItem<ServiceResult>(fileResult.ServiceResultId).IsPositive,
                "Assert that result is positive");
        }

        /// <summary>
        /// Тест на отказ в переоформлении лицензии
        /// </summary>
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(12)]
        public void NotRenewalLicenseTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActExpertiseOrder();
            ActSupervisionStepNext();
            ActProvideExpertise();
            ActSupervisionStepNext();
            ActInspectionOrder();
            ActSupervisionStepNext();
            ActProvideInspection();
            ActSupervisionStepNext();
            ActGrantLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Rejected);
            ActGrantServiceResult();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Rejected);
            Assert.NotNull(dossierFile.DossierFileServiceResult,
                "Assert that there is a file serviceresult");

            var fileResult = dossierFile.DossierFileServiceResult;
            Assert.That(!DictionaryManager.GetDictionaryItem<ServiceResult>(fileResult.ServiceResultId).IsPositive,
                "Assert that result is negative");
        }

        //[Test]
        //public void MedReorganizationRenewalLicenseTest()
        //{
        //    // Arrange
        //    var task = ArrangeTask("Renewal/01_med_renewal_reorganization");

        //    // Act
        //    ActAccepting(task);
        //    ActLinkaging(RequisitesOrigin.FromTask);
        //    ActSupervisionStepNext();
        //    ActSupervisionStepNext();
        //    ActSupervisionStepNextWithStatus(TaskStatusType.Working);
        //    ActExpertiseOrder();
        //    ActSupervisionStepNext();
        //    ActProvideExpertise();
        //    ActSupervisionStepNext();
        //    ActInspectionOrder();
        //    ActSupervisionStepNext();
        //    ActProvideInspection();
        //    ActSupervisionStepNext();
        //    ActRenewalLicenseOrder();
        //    ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
        //    ActRenewalLicense();
            
        //    // Assert
        //    var holderReqs = Superviser.DossierFile.LicenseDossier.LicenseHolder.ActualRequisites;
        //    var licenseReqs = Superviser.DossierFile.License.ActualRequisites;

        //    Assert.AreEqual(holderReqs.FullName, "");
        //    Assert.AreEqual(holderReqs.ShortName, "");
        //    Assert.AreEqual(holderReqs.FirmName, "");
        //}

        #endregion


        #region Stop
        
        /// <summary>
        /// Тест на переход к этапу прекращение действия лицензии
        /// </summary>
        [TestCase(3)]
        [TestCase(8)]
        [TestCase(13)]
        public void StepToStopLicenseTest(int serviceId)
        {
            // Arrange
            var inn = RandomProvider.RandomNumberString(11);
            var ogrn = RandomProvider.RandomNumberString(13);

            var existingtask = ArrangeTask(GetNewLicenseServiceId(serviceId), inn, ogrn);
            var existingHolder = ActCreateJuridicalLicenseHolder(existingtask);
            var existingDossier = ActCreateExistingLicenseDossier(existingHolder, existingtask);
            var existinglicense = ActCreateExistingLicense(existingDossier);

            var task = ArrangeTask(serviceId, inn, ogrn);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromRegistr);
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActStopLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Ready);
            AssertStep(dossierFile, 4);
            AssertPersistence(dossierFile);
        }

        /// <summary>
        /// Тест на прекращение действия лицензии
        /// </summary>
        [TestCase(3)]
        [TestCase(8)]
        [TestCase(13)]
        public void StopLicenseTest(int serviceId)
        {
            // Arrange
            var inn = RandomProvider.RandomNumberString(11);
            var ogrn = RandomProvider.RandomNumberString(13);

            var existingtask = ArrangeTask(GetNewLicenseServiceId(serviceId), inn, ogrn);
            var existingHolder = ActCreateJuridicalLicenseHolder(existingtask);
            var existingDossier = ActCreateExistingLicenseDossier(existingHolder, existingtask);
            var existinglicense = ActCreateExistingLicense(existingDossier);

            var task = ArrangeTask(serviceId, inn, ogrn);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromRegistr);
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActStopLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            ActStopLicense();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertLicenseCurrentStatus(dossierFile.License, LicenseStatusType.Stop);

            Assert.That(dossierFile.License.LicenseStatusList.Count, Is.EqualTo(2), "Assert that license has two statuses");

            AssertFileCurrentStatus(dossierFile, TaskStatusType.Done);
            Assert.NotNull(dossierFile.DossierFileServiceResult,
                "Assert that there is a file serviceresult");

            var fileResult = dossierFile.DossierFileServiceResult;
            Assert.True(DictionaryManager.GetDictionaryItem<ServiceResult>(fileResult.ServiceResultId).IsPositive,
                "Assert that result is positive");


        }
        
        #endregion


        #region Copy

        #endregion


        #region Dublicate

        #endregion

    }
}
