using System;
using System.Linq;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.Linkage;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Приёмочные тесты на занесение результатов выездной проверки
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class InspectionTests : SupervisionFixture
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
        public void StepToInspectionTest(int serviceId)
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
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Working);
            AssertStep(dossierFile, 8);
            AssertPersistence(dossierFile);
        }


        /// <summary>
        /// Тест на издание приказа о проведении выездной проверки
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void ProvideInspectionOrderTest(int serviceId)
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
            ActProvideInspection();
            var dossierFile = Superviser.DossierFile;

            // Assert
            var fileStep =
                dossierFile.DossierFileStepList.Single(t => t.ScenarioStepId == dossierFile.CurrentScenarioStepId);

            Assert.NotNull(fileStep.Inspection,
                "Assert that current step contains inspection");

            var inspection = fileStep.Inspection;

            Assert.AreEqual(inspection.InspectionEmployeeList.Count, 2,
                "Assert that inspection involve 2 employees");

            Assert.AreEqual(inspection.InspectionExpertList.Count, 0,
                "Assert that inspection doesn't involve experts");

            Assert.AreEqual(inspection.ActStamp.Date, DateTime.Today,
                "Assert that inspection is today dated");

            Assert.That(inspection.IsPassed,
                "Assert that expertise is passed");

        }
    }
}
