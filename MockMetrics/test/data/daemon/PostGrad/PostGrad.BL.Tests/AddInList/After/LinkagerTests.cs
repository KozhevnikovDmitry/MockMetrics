using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.Dossier;
using ILinkagerAddIn = PostGrad.BL.AddInList.After.ILinkagerAddIn;
using Linkager = PostGrad.BL.AddInList.After.Linkager;

namespace PostGrad.BL.Tests.AddInList.After
{
    [TestFixture]
    public class LinkagerTests
    {
        [Test]
        public void LinkageTest()
        {
            // Arrange
            var addIn = new Mock<ILinkagerAddIn>();
            var db = Mock.Of<IDomainDbManager>();
            var file = Mock.Of<DossierFile>();
            var linkager = new Linkager(() => db, new List<ILinkagerAddIn>
            {
                addIn.Object,
                addIn.Object,
                addIn.Object
            });

            // Act
            var fileWrapper = linkager.Linkage(file);

            // Assert
            Assert.AreEqual(fileWrapper.DossierFile, file);
            addIn.Verify(t => t.Linkage(It.Is<IDossierFileLinkWrapper>(w => w.Equals(fileWrapper)), db), Times.Exactly(3));
        }

        [Test]
        public void LinkageBySortedAddinsTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var file = Mock.Of<DossierFile>();
            var addinJournal = new List<int>();
            var addIn1 = new Mock<ILinkagerAddIn>();
            addIn1.SetupGet(t => t.SortOrder).Returns(1);
            addIn1.Setup(t => t.Linkage(It.IsAny<IDossierFileLinkWrapper>(), db)).Callback(() => addinJournal.Add(1));
            var addIn0 = new Mock<ILinkagerAddIn>();
            addIn0.SetupGet(t => t.SortOrder).Returns(0);
            addIn0.Setup(t => t.Linkage(It.IsAny<IDossierFileLinkWrapper>(), db)).Callback(() => addinJournal.Add(0));

            var linkager = new Linkager(() => db, new List<ILinkagerAddIn>
            {
                addIn1.Object,
                addIn0.Object
            });

            // Act
            linkager.Linkage(file);

            // Assert
            Assert.AreEqual(addinJournal.First(), 0);
            Assert.AreEqual(addinJournal.Last(), 1);
        }
    }
}