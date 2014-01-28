using System;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.DA.Interface;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Linkage
{
    [TestFixture]
    public class LinkageRequisitesAddInTests
    {
        [Test]
        public void SortOrderTest()
        {
            // Arrange
            var addin = new LinkageRequisitesAddIn(Mock.Of<IParser>());

            // Assert
            Assert.AreEqual(addin.SortOrder, 1);
        }

        [Test]
        public void SetupAvailableRequisitesForNewHolder()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task);
            var taskReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today);
            var holder =
                Mock.Of<LicenseHolder>(
                    h => h.RequisitesList == new EditableList<HolderRequisites> { taskReqs });
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseHolder == holder);

            var addin = new LinkageRequisitesAddIn(Mock.Of<IParser>());

            // Act
            addin.Linkage(fileLink, Mock.Of<IDomainDbManager>());

            // Assert
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Value, fileLink.SelectedRequisites);
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Value, taskReqs);
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Key, RequisitesOrigin.FromTask);
        }

        [Test]
        public void SetupAvailableRequisitesForFullScenario()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task);
            var taskReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today);
            var holderReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today.AddDays(1));
            var holder =
                Mock.Of<LicenseHolder>(
                    h => h.Id == 1 && h.RequisitesList == new EditableList<HolderRequisites> { taskReqs, holderReqs });
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseHolder == holder);
            var taskDataParser =
                Mock.Of<IParser>(
                    t =>
                    t.ParseHolder(task) ==
                    Mock.Of<LicenseHolder>(h => h.RequisitesList == new EditableList<HolderRequisites> { taskReqs }));

            var addin = new LinkageRequisitesAddIn(taskDataParser);

            // Act
            addin.Linkage(fileLink, Mock.Of<IDomainDbManager>());

            // Assert
            Assert.AreEqual(fileLink.AvailableRequisites.First().Value, fileLink.SelectedRequisites);
            Assert.AreEqual(fileLink.AvailableRequisites[RequisitesOrigin.FromRegistr], holderReqs);
            Assert.AreEqual(fileLink.AvailableRequisites[RequisitesOrigin.FromTask], taskReqs);
        }

        [Test]
        public void SetupAvailableRequisitesForLightScenario()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.ScenarioType == ScenarioType.Light);
            var taskReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today);
            var holderReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today.AddDays(1));
            var holder =
                Mock.Of<LicenseHolder>(
                    h => h.Id == 1 && h.RequisitesList == new EditableList<HolderRequisites> { taskReqs, holderReqs });
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseHolder == holder);

            var addin = new LinkageRequisitesAddIn(Mock.Of<IParser>());

            // Act
            addin.Linkage(fileLink, Mock.Of<IDomainDbManager>());

            // Assert
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Value, fileLink.SelectedRequisites);
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Value, holderReqs);
            Assert.AreEqual(fileLink.AvailableRequisites.Single().Key, RequisitesOrigin.FromRegistr);
        }

        [Test]
        public void AlwaysGetTheSameAvailableRequisistes()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task);
            var taskReqs = Mock.Of<HolderRequisites>(t => t.CreateDate == DateTime.Today);
            var holder =
                Mock.Of<LicenseHolder>(
                    h => h.Id == 1 && h.RequisitesList == new EditableList<HolderRequisites> { taskReqs });
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseHolder == holder);

            var taskParser = new Mock<IParser>();
            taskParser.Setup(t => t.ParseHolder(task)).Returns(holder);

            var addin = new LinkageRequisitesAddIn(taskParser.Object);

            // Act
            addin.Linkage(fileLink, Mock.Of<IDomainDbManager>());

            // Assert
            taskParser.Verify(t => t.ParseHolder(task), Times.Once());
        }
    }
}