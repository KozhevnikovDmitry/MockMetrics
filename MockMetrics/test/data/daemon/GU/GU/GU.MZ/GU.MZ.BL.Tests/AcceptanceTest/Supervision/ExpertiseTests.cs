using GU.DataModel;
using GU.MZ.BL.DomainLogic.Linkage;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Приёмочные тесты на проведение документарной проверки в рамках тома
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class ExpertiseTests : SupervisionFixture
    {
        /// <summary>
        /// Тест на переход к этапу проведение документарной проверки
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void StepToDocumentExpertiseTest(int serviceId)
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
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Working);
            AssertStep(dossierFile, 6);
            AssertPersistence(dossierFile);
        }

        /// <summary>
        /// Тест на проведение документарной проверки
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void ProvideDocumentExpertiseTest(int serviceId)
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
            var dossierFile = Superviser.DossierFile;

            // Assert
            Assert.NotNull(dossierFile.CurrentFileStep.DocumentExpertise, 
                "Assert that current step contains document expertise");

            var expertise = dossierFile.CurrentFileStep.DocumentExpertise;

            Assert.AreEqual(expertise.ExperiseResultList.Count, 1,
                "Assert that expertise contains single result");
            
            Assert.False(expertise.IsPassed,
                "Assert that expertise is not passed");
        }
    }
}
