using GU.DataModel;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Приёмочные тесты на логику издания приказа о предоставлении или отказе в услуге
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class OrderResultTests : SupervisionFixture
    {
        /// <summary>
        /// Тест на переход к этапу издания приказа о возврате или приёме документов
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void StepToPublishGrantRejectNoticeTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging( RequisitesOrigin.FromTask);
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
            var dossierFile = Superviser.DossierFile;


            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Working);
            AssertStep(dossierFile, 9);
            AssertPersistence(dossierFile);
        }

        /// <summary>
        /// Тест на издание приказа о предсотавлении лицензии
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void GrantLicenseOrderTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging( RequisitesOrigin.FromTask);
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
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.ServiceGrant);
            AssertStandartOrder(dossierFile, StandartOrderType.GrantLicense);
            AssertOrderDetail(dossierFile);
            AssertOrderLicenseSubject(dossierFile);
            AssertOrderTaskSubjectComment(dossierFile);
        }

        /// <summary>
        /// Тест на издание приказа отказе в предоставлении лицензии
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void NotGrantLicenseOrderTest(int serviceId)
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
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.ServiseReject);
            AssertStandartOrder(dossierFile, StandartOrderType.NotGrantLicense);
            AssertOrderDetail(dossierFile);
            AssertOrderLicenseSubject(dossierFile);
        }

        /// <summary>
        /// Тест на издание приказа о переоформлении лицензии
        /// </summary>
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(12)]
        public void RenewalLicenseOrderTest(int serviceId)
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
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.ServiceGrant);
            AssertStandartOrder(dossierFile, StandartOrderType.GrantLicense);
            AssertOrderDetail(dossierFile);
            AssertOrderLicenseSubject(dossierFile);
            AssertOrderTaskSubjectComment(dossierFile);
        }

        /// <summary>
        /// Тест на издание приказа отказе в переоформлении лицензии
        /// </summary>
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(12)]
        public void NotRenewalLicenseOrderTest(int serviceId)
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
            ActNotRenewalLicenseOrder();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.ServiseReject);
            AssertStandartOrder(dossierFile, StandartOrderType.NotRenewalLicense);
            AssertOrderDetail(dossierFile);
            AssertOrderLicenseSubject(dossierFile);
        }


        /// <summary>
        /// Тест на издание приказа о прекращении действия лицензии
        /// </summary>
        [TestCase(3)]
        [TestCase(8)]
        [TestCase(13)]
        public void StopLicenseOrderTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);
            var holder = ActCreateJuridicalLicenseHolder(task);
            ActCreateExistingLicenseDossier(holder, task);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromRegistr);
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActStopLicenseOrder();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.ServiceGrant);
            AssertStandartOrder(dossierFile, StandartOrderType.StopLicense);
            AssertOrderDetail(dossierFile);
            AssertOrderLicenseSubject(dossierFile);
            AssertOrderTaskSubjectComment(dossierFile);
        }
    }
}
