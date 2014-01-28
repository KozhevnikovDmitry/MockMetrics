using System.Collections.Generic;
using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.GuParse
{
    [TestFixture]
    public class DrugParserTests
    {
        [Test]
        public void LicenseServiceTypeTest()
        {
            // Act
            var parser = new DrugParser(Mock.Of<IContentImporter>(), Mock.Of<IContentMapper>());

            // Assert
            Assert.AreEqual(parser.LicenseServiceType, LicenseServiceType.Drug);
        }

        [Test]
        public void GetHolderNodeTest()
        {
            // Arrange
            var root = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>(t => t.Service.ServiceGroup.IsOnlyForJuridical
                                       && t.RootContentNode == root);
            var holderNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNode(root, "OrgInfo") == holderNode);
            var parser = new DrugParser(importer, Mock.Of<IContentMapper>());

            // Act
            var result = parser.GetHolderNode(task);

            // Assert
            Assert.AreEqual(result, holderNode);
        }

        [Test]
        public void GetLicenseNodeTest()
        {
            // Arrange
            var root = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>(t => t.RootContentNode == root);
            var licenseNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNode(root, "LicenseInfo") == licenseNode);
            var parser = new DrugParser(importer, Mock.Of<IContentMapper>());

            // Act
            var result = parser.GetLicenseNode(task);

            // Assert
            Assert.AreEqual(result, licenseNode);
        }

        [Test]
        public void GetLicenseObjectsNodesTest()
        {
            // Arrange
            var root = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>(t => t.RootContentNode == root);
            var licenseObjectNodes = Mock.Of<IList<ContentNode>>();
            var holderNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNode(root, "OrgInfo") == holderNode
                                                       && t.GetContentNodes(holderNode, "WorksInfo") == licenseObjectNodes);
            var parser = new DrugParser(importer, Mock.Of<IContentMapper>());

            // Act
            var result = parser.GetLincenseObjectNodes(task);

            // Assert
            Assert.AreEqual(result, licenseObjectNodes);
        }

        [Test]
        public void CompleteLicenseObjectParseSubactivitiesTest()
        {
            // Arrange
            var licObject = Mock.Of<LicenseObject>();
            var licObjNode = Mock.Of<ContentNode>();
            var work1 = Mock.Of<ContentNode>(t => t.StrValue == "Work1");
            var work2 = Mock.Of<ContentNode>(t => t.StrValue == "Work2");
            var worksNodes = new List<ContentNode> {work1, work2};
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetContentNodes(licObjNode, "Works") == worksNodes);
            var parser = new DrugParser(contentImporter, Mock.Of<IContentMapper>());

            // Act
            parser.CompleteLicenseObject(licObjNode, licObject);

            // Assert
            Assert.AreEqual(licObject.Note, "Work1, Work2");
        }

        [Test]
        public void CompleteLicenseObjectParseApartmentDescriptionTest()
        {
            // Arrange
            var address = Mock.Of<Address>();
            var licObject = Mock.Of<LicenseObject>(t => t.Address == address);
            var licObjNode = Mock.Of<ContentNode>();
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetContentNodes(licObjNode, "Works") == new List<ContentNode>()
                                                              && t.GetNodeStrValue(licObjNode, "ApartmentDescription") == "ApartmentDescription");
            var parser = new DrugParser(contentImporter, Mock.Of<IContentMapper>());

            // Act
            parser.CompleteLicenseObject(licObjNode, licObject);

            // Assert
            Assert.AreEqual(licObject.Address.Note, "ApartmentDescription");
        }

        [Test]
        public void ParseLicenseObjectTest()
        {
            // Arrange
            var licObjNode = Mock.Of<ContentNode>();
            var addressNode = Mock.Of<ContentNode>();
            var address = Mock.Of<Address>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNode(licObjNode, "Address") == addressNode);
            var loader = Mock.Of<IContentMapper>(t => t.MapAddress(addressNode) == address);
            var parser = new Mock<DrugParser>(importer, loader) { CallBase = true };
            parser.Setup(t => t.CompleteLicenseObject(licObjNode, It.IsAny<LicenseObject>()));

            // Act
            var result = parser.Object.ParseLicenseObject(licObjNode);

            // Assert
            Assert.AreEqual(result.Address, address);
            parser.Verify(t => t.CompleteLicenseObject(licObjNode, result), Times.Once);
        }
    }
}