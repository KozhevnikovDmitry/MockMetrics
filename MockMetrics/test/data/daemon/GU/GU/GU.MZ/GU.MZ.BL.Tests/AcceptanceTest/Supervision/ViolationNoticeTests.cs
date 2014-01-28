using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Notifying;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Приёмочные тесты на логику послыки уведомления о необходимости устранить нарушения
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class ViolationNoticeTests : SupervisionFixture
    {
        /// <summary>
        /// Тест на переход к этапу посылки уведомления о необходимости устранения нарушений
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void StepToViolationNoticeTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertStep(dossierFile, 3);
            AssertPersistence(dossierFile);
        }

        /// <summary>
        /// Тест на посылку уведомления о необходимости устранения нарушений
        /// C занесением данных об устранении нарушений
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void SendRejectNoticeWithResolveTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActNoticeOfRejectDossierFile();
            ActPrepareNotResolvedViolationNotice();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.RejectDocuments);
            AssertViolationNotice(dossierFile, "Ужасные нарушения");
        }
        
        /// <summary>
        /// Тест на посылку уведомления о необходимости устранения нарушений
        /// С занесением данных о том, что нарушения не были устранены
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void SendRejectNoticeWithoutResolveTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActNoticeOfRejectDossierFile();
            ActPrepareNotResolvedViolationNotice();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.RejectDocuments);
            AssertViolationNotice(dossierFile, "Ужасные нарушения");
        }
    }
}
