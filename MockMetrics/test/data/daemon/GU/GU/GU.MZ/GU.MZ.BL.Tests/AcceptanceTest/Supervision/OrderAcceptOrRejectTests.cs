using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Приёмочные тесты на логику издания приказа о приёме или возврате документов
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class OrderAcceptOrRejectTests : SupervisionFixture
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
        public void StepToAcceptOrReturnTaskTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActNoticeOfRejectDossierFile();
            ActSupervisionStepNext();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertStep(dossierFile, 4);
            AssertPersistence(dossierFile);
        }

        /// <summary>
        /// Тест на издание приказа о возврате документов
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void ReturnTaskOrderTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActNoticeOfRejectDossierFile();
            ActPrepareNotResolvedViolationNotice();
            ActSupervisionStepNext();
            ActReturnTaskOrder();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.RejectDocuments);
            AssertStandartOrder(dossierFile, StandartOrderType.ReturnTask);
            AssertOrderTaskSubject(dossierFile);
            AssertOrderViolationComment(dossierFile);
            AssertOrderDetail(dossierFile);
        }

        /// <summary>
        /// Тест на издание приказа о возврате документов
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void AcceptTaskOrderTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActNoticeOfRejectDossierFile();
            ActPrepareResolvedViolationNotice();
            ActSupervisionStepNext();
            ActAcceptTaskOrder();
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.AcceptDocuments);
            AssertStandartOrder(dossierFile, StandartOrderType.AcceptTask);
        }

    }
}
