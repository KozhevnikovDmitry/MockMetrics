using BLToolkit.EditableObjects;
using Common.DA.Interface;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Linkage
{
    [TestFixture]
    public class LinkageHolderAddInTests
    {
        [Test]
        public void SortOrderTest()
        {
            // Arrange
            var addin = new LinkageHolderAddIn(Mock.Of<IParser>(), Mock.Of<LicenseHolderRepository>());

            // Assert
            Assert.AreEqual(addin.SortOrder, 0);
        }

        [Test]
        public void LinkageExistingHolderTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var holder = Mock.Of<LicenseHolder>(t => t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file);
            var holderInfo = new HolderInfo { Inn = "1", Ogrn = "2" };
            var taskDataParser = Mock.Of<IParser>(t => t.ParseHolderInfo(task) == holderInfo);
            var holderRegistr = Mock.Of<LicenseHolderRepository>(t => t.HolderExists("1", "2", db)
                                                                    && t.GetLicenseHolder("1", "2", db) == holder);

            var addin = new LinkageHolderAddIn(taskDataParser, holderRegistr);

            // Act
            addin.Linkage(fileLink, db);

            // Assert
            Assert.AreEqual(fileLink.LicenseHolder, holder);
        }

        [Test]
        public void LinkageNewHolderTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var holder = Mock.Of<LicenseHolder>();
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file);
            var holderInfo = new HolderInfo();
            var taskDataParser = Mock.Of<IParser>(t => t.ParseHolderInfo(task) == holderInfo
                                                               && t.ParseHolder(task) == holder);
            var holderRegistr = Mock.Of<LicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db) == false);

            var addin = new LinkageHolderAddIn(taskDataParser, holderRegistr);

            // Act
            addin.Linkage(fileLink, db);

            // Assert
            Assert.AreEqual(fileLink.LicenseHolder, holder);
        }

        [Test]
        public void NoExistingHolderForLightSceanrio()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var holder = Mock.Of<LicenseHolder>();
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task && t.ScenarioType == ScenarioType.Light);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file);
            var holderInfo = new HolderInfo();
            var taskDataParser = Mock.Of<IParser>(t => t.ParseHolderInfo(task) == holderInfo
                                                               && t.ParseHolder(task) == holder);
            var holderRegistr = Mock.Of<LicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db) == false);

            var addin = new LinkageHolderAddIn(taskDataParser, holderRegistr);

            // Assert
            Assert.Throws<NoExistingHolderForLightSceanrioException>(() => addin.Linkage(fileLink, db));
        }
    }
}