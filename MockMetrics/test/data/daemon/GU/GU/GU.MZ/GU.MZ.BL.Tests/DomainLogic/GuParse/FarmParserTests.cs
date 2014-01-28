using System.Collections.Generic;
using BLToolkit.EditableObjects;
using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.GuParse
{
    [TestFixture]
    public class FarmParserTests
    {
        [Test]
        public void LicenseServiceTypeTest()
        {
            // Act
            var parser = new FarmParser(Mock.Of<IContentImporter>(), Mock.Of<IContentMapper>());

            // Assert
            Assert.AreEqual(parser.LicenseServiceType, LicenseServiceType.Farm);
        }

        [Test]
        public void CompleteLicenseObjectParseSubactivitiesTest()
        {
            // Arrange
            var licObject = Mock.Of<LicenseObject>();
            var licObjNode = Mock.Of<ContentNode>();
            var work1 = Mock.Of<ContentNode>(t => t.BoolValue == true && t.SpecNode.Name == "Work1");
            var work2 = Mock.Of<ContentNode>(t => t.BoolValue == true && t.SpecNode.Name == "Work2");
            var work3 = Mock.Of<ContentNode>(t => t.BoolValue == false && t.SpecNode.Name == "Work3");
            var work4 = Mock.Of<ContentNode>(t => t.BoolValue == null && t.SpecNode.Name == "Work4");
            var worksNode = Mock.Of<ContentNode>(t => t.ChildContentNodes == new EditableList<ContentNode> { work1, work2, work3, work4 });
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetContentNode(licObjNode, "Works") == worksNode);
            var parser = new FarmParser(contentImporter, Mock.Of<IContentMapper>());

            // Act
            parser.CompleteLicenseObject(licObjNode, licObject);

            // Assert
            Assert.AreEqual(licObject.Note, "Work1, Work2");
        }

        [Test]
        public void CompleteLicenseObjectParseSubjectTest()
        {
            var licObject = Mock.Of<LicenseObject>();
            var licObjNode = Mock.Of<ContentNode>();
            var worksNode = Mock.Of<ContentNode>(t => t.ChildContentNodes == new EditableList<ContentNode>());
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetNodeStrValue(licObjNode, "Subject") == "Subject"
                                                              && t.GetContentNode(licObjNode, "Works") == worksNode);
            var parser = new FarmParser(contentImporter, Mock.Of<IContentMapper>());

            // Act
            parser.CompleteLicenseObject(licObjNode, licObject);

            // Assert
            Assert.AreEqual(licObject.Name, "Subject");
        }
    }
}