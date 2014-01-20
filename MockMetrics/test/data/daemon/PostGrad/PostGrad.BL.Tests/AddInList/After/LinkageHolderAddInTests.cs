using BLToolkit.EditableObjects;
using Moq;
using NUnit.Framework;
using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Holder;
using LinkageHolderAddIn = PostGrad.BL.AddInList.After.LinkageHolderAddIn;

namespace PostGrad.BL.Tests.AddInList.After
{
    [TestFixture]
    public class LinkageHolderAddInTests
    {
        [Test]
        public void SortOrderTest()
        {
            // Arrange
            var addin = new LinkageHolderAddIn(Mock.Of<ITaskParser>(), Mock.Of<ILicenseHolderRepository>());

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
            var holderInfo =  Mock.Of<HolderInfo>(t => t.Inn == "1" && t.Ogrn == "2");
            var taskDataParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == holderInfo);
            var holderRegistr = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists("1", "2", db)
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
            var holderInfo = Mock.Of<HolderInfo>();
            var taskDataParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == holderInfo
                                                               && t.ParseHolder(task) == holder);
            var holderRegistr = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db) == false);

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
            var holderInfo = Mock.Of<HolderInfo>();
            var taskDataParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == holderInfo
                                                               && t.ParseHolder(task) == holder);
            var holderRegistr = Mock.Of<ILicenseHolderRepository>(t => t.HolderExists(It.IsAny<string>(), It.IsAny<string>(), db) == false);

            var addin = new LinkageHolderAddIn(taskDataParser, holderRegistr);

            // Assert
            Assert.Throws<NoExistingHolderForLightSceanrioException>(() => addin.Linkage(fileLink, db));
        }
    }
}