using System;
using GU.MZ.BL.DomainLogic.ViolationResolve;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.ViolationResolve
{
    /// <summary>
    /// Тесты на методы класса ViolationResolver
    /// </summary>
    [TestFixture]
    public class ViolationResolverTests
    {
        /// <summary>
        /// Тест на заведение информации об устранении нарушений
        /// </summary>
        [Test]
        public void PrepareViolationResolveInfoTest()
        {
            // Arrange
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.StepViolationResolveInfo(scenarioStep) == fileStep.ViolationResolveInfo);

            // Act
            var violationResolveInfo = new ViolationResolver().PrepareViolationResolveInfo(file, scenarioStep);

            // Assert
            Assert.NotNull(violationResolveInfo);
            Assert.AreEqual(violationResolveInfo, fileStep.ViolationResolveInfo);
        }

        [Test]
        public void PrepareViolationNoticeTest()
        {
            // Arrange
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.StepViolationResolveInfo(scenarioStep) == fileStep.ViolationResolveInfo
                                              && t.TaskStamp == DateTime.Today);

            // Act
            var notice = new ViolationResolver().PrepareViolationNotice(file, scenarioStep);

            // Assert
            Assert.NotNull(notice);
            Assert.AreEqual(notice, fileStep.ViolationNotice);
        }

        [Test]
        public void PrepareViolationNoticeTaskDataTest()
        {
            // Arrange
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.StepViolationResolveInfo(scenarioStep) == fileStep.ViolationResolveInfo
                                              && t.TaskId == 100500
                                              && t.TaskStamp == DateTime.Today);

            // Act
            var notice = new ViolationResolver().PrepareViolationNotice(file, scenarioStep);

            // Assert
            Assert.AreEqual(notice.TaskRegNumber, 100500);
            Assert.AreEqual(notice.TaskStamp, DateTime.Today);
        }

        [Test]
        public void NullTaskCreateDateTest()
        {
            // Arrange
            var scenarioStep = Mock.Of<ScenarioStep>();
            var dossierFile = Mock.Of<DossierFile>();

            // Assert
            Assert.Throws<NullTaskCreateDateException>(() => new ViolationResolver().PrepareViolationNotice(dossierFile, scenarioStep));
        }

        [Test]
        public void PrepareViolationNoticeFileDataTest()
        {
            // Arrange
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.StepViolationResolveInfo(scenarioStep) == fileStep.ViolationResolveInfo
                                              && t.TaskStamp == DateTime.Today
                                              && t.EmployeeName == "EmployeeName"
                                              && t.EmployeePosition == "EmployeePosition"
                                              && t.LicensedActivityName == "LicensedActivityName"
                                              && t.HolderFullName == "HolderName"
                                              && t.HolderAddress == "HolderAddress");

            // Act
            var notice = new ViolationResolver().PrepareViolationNotice(file, scenarioStep);

            // Assert
            Assert.AreEqual(notice.EmployeeName, "EmployeeName");
            Assert.AreEqual(notice.EmployeePosition, "EmployeePosition");
            Assert.AreEqual(notice.LicensedActivity, "LicensedActivityName");
            Assert.AreEqual(notice.LicenseHolder, "HolderName");
            Assert.AreEqual(notice.Address, "HolderAddress");
        }
    }
}
