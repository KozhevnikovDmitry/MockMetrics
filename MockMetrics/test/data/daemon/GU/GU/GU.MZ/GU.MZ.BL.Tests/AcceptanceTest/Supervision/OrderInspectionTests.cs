using System;
using System.Linq;
using Common.Types;
using GU.BL.Extensions;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Notifying;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Приёмочные тесты на проведение выездной проверки в рамках тома
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class OrderInspectionTests : SupervisionFixture
    {
        /// <summary>
        /// Тест на переход к этапу издания приказа о проведении выездной проверки
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void StepToPublishInspectionNoticeTest(int serviceId)
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
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertFileCurrentStatus(dossierFile, TaskStatusType.Working);
            AssertStep(dossierFile, 7);
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
        public void PublishExpertiseOrderTest(int serviceId)
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
            var dossierFile = Superviser.DossierFile;

            // Assert
            AssertNotice(dossierFile, NoticeType.PlaceInspection);

            var order = dossierFile.CurrentFileStep.InspectionOrder;
            Assert.AreEqual(order.Id, dossierFile.CurrentFileStep.Id,
                "Order id equals step id");
            Assert.AreEqual(order.Stamp.Date, DateTime.Today,
                "Order is published today");
            Assert.AreEqual(order.RegNumber, "№ 100500 лиц.",
                "Order regnumber is not assigned");
            Assert.AreEqual(order.EmployeeName, dossierFile.EmployeeName,
                "Order employee name from dossierFile employee");
            Assert.AreEqual(order.EmployeePosition, dossierFile.EmployeePosition,
                "Order employee position from dossierFile employee");
            Assert.AreEqual(order.EmployeeContacts, dossierFile.EmployeeContacts,
                "Order employee contacts from dossierFile employee");
            Assert.AreEqual(order.TaskId, dossierFile.TaskId,
                "Order task id is dossierFile task id");
            Assert.AreEqual(order.TaskStamp, dossierFile.TaskStamp,
                "Order task stamp is dossierFile task stamp");
            Assert.AreEqual(order.Address, dossierFile.HolderAddress,
                "Order holder address list as single address, that equals dossierFile address");
            Assert.AreEqual(order.FullName, dossierFile.HolderFullName,
                "Order full name equals doaaierFile holder full name");
            Assert.AreEqual(order.Inn, dossierFile.HolderInn,
                "Order inn equals doaaierFile holder inn");
            Assert.AreEqual(order.LicensedActivity, dossierFile.LicensedActivity.BlankName,
                "Order licensed activity is blank name of dossierfile licensed activity");
            Assert.AreEqual(order.ActivityAdditionalInfo, dossierFile.LicensedActivity.AdditionalInfo,
                "Order activity additional info is additiona info of dossierfile licensed activity");

            var scenarioStep = DictionaryManager.GetDictionaryItem<ScenarioStep>(dossierFile.CurrentFileStep.ScenarioStepId);
            Assert.AreEqual(order.DueDays, scenarioStep.Scenario.ScenarioStepList.Next(scenarioStep).DueDays,
                "Order due days from scenatio step");
            Assert.AreEqual(order.StartDate, order.Stamp.AddDays(1),
                "Order start date equals next day from order order stamp");
            Assert.AreEqual(order.DueDate, order.StartDate.AddWorkingDays(order.DueDays),
                "Order due date from start date add working due days");

            Assert.AreEqual(order.InspectionOrderAgreeList.Count, 2,
                "Order has two agrees");
            Assert.That(order.InspectionOrderAgreeList.All(t => !string.IsNullOrEmpty(t.EmployeeName) && !string.IsNullOrEmpty(t.EmployeePosition)),
                "Order agrees havу not null name and position");
        }

    }
}