using System;
using GU.MZ.BL.DomainLogic.Notify;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Notifying;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Notify
{
    /// <summary>
    /// Тесты на методы класса DossierFileNotifier
    /// </summary>
    [TestFixture]
    public class DossierFileNotifierTests
    {
        [Test]
        public void AddNoticeTest()
        {
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.StepNotice(scenarioStep) == fileStep.Notice
                                              && t.HolderAddress == "HolderAddress"
                                              && t.TaskCustomerEmail == "TaskCustomerEmail");

            var noticeType = NoticeType.AcceptDocuments;
            var notifier = new DossierFileNotifier();

            // Act
            var notice = notifier.AddNotice(file, noticeType, scenarioStep);

            // Assert
            Assert.AreEqual(notice.NoticeType, noticeType);
            Assert.AreEqual(notice.Id, 1);
            Assert.AreEqual(notice.Address, "HolderAddress");
            Assert.AreEqual(notice.Email, "TaskCustomerEmail");
            Assert.AreEqual(notice.Stamp.Date, DateTime.Today);
            Assert.AreEqual(fileStep.Notice, notice);
        }
    }
}
