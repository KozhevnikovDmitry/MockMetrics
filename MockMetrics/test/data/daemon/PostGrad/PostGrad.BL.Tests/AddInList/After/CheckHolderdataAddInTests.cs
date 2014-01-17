using Moq;
using NUnit.Framework;
using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Holder;
using CheckHolderDataAddIn = PostGrad.BL.AddInList.After.CheckHolderDataAddIn;

namespace PostGrad.BL.Tests.AddInList.After
{
    [TestFixture]
    public class CheckHolderdataAddInTests
    {
        [Test]
        public void SortOrderTest()
        {
            // Arrange
            var addin = new CheckHolderDataAddIn(Mock.Of<ITaskParser>());

            // Assert
            Assert.AreEqual(addin.SortOrder, 4);
        }

        [TestCase("1", "2", "1", "2", Result = false)]
        [TestCase("1", "2", "100500", "2", Result = true)]
        [TestCase("1", "2", "1", "100500", Result = true)]
        public bool GetIsHolderDataDoubtfullTest(string taskInn, string taskOgrn, string regInn, string regOgrn)
        {
            // Arrange
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.Task == task);
            var holderInfo = new HolderInfo { Inn = taskInn, Ogrn = taskOgrn };
            var taskDataParser = Mock.Of<ITaskParser>(t => t.ParseHolderInfo(task) == holderInfo);
            var holder = Mock.Of<LicenseHolder>(t => t.Ogrn == regOgrn && t.Inn == regInn);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseHolder == holder);
            var addin = new CheckHolderDataAddIn(taskDataParser);

            // Act
            addin.Linkage(fileLink, Mock.Of<IDomainDbManager>());

            // Assert
            return fileLink.IsHolderDataDoubtfull;
        }
    }
}