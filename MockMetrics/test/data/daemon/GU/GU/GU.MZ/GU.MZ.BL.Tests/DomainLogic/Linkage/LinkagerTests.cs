using System.Collections.Generic;
using System.Linq;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Dossier;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Linkage
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