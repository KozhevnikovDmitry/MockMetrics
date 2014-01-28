using System;
using System.Collections.Generic;
using System.Linq;
using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.GuParse
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void ParseHolderLoadHolderDataTest()
        {
            // Arrange
            var holder = Mock.Of<LicenseHolder>();
            var requisites = Mock.Of<HolderRequisites>();
            var address = Mock.Of<Address>();
            var holderNode = Mock.Of<ContentNode>();
            var addressNode = Mock.Of<ContentNode>();
            var chiefNode = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>();
            var importer = Mock.Of<IContentImporter>(t => t.GetChildContentNodeEndsWith(holderNode, "Address") == addressNode
                                                       && t.GetContentNode(holderNode, "Director") == chiefNode);
            var mapper = Mock.Of<IContentMapper>(t => t.MapAddress(addressNode) == address
                                                   && t.MapHolder(holderNode) == holder
                                                   && t.MapRequisites(holderNode) == requisites);

            var parser = new Mock<Parser>(importer, mapper) { CallBase = true };
            parser.Setup(t => t.GetHolderNode(task)).Returns(holderNode);

            // Act
           var result = parser.Object.ParseHolder(task);

            // Assert
            Assert.AreEqual(holder.RequisitesList.Single(), requisites);
            Assert.AreEqual(holder.RequisitesList.Single().Address, address);
            Assert.AreEqual(result, holder);
        }

        [Test]
        public void ParseHolderInfoTest()
        {
            // Arrange
            var holderNode = Mock.Of<ContentNode>();
            var holderInfo = Mock.Of<HolderInfo>();
            var task = Mock.Of<Task>();
            var contentMapper = Mock.Of<IContentMapper>(t => t.MapHolderInfo(holderNode) == holderInfo);
            var parser = new Mock<Parser>(Mock.Of<IContentImporter>(), contentMapper) { CallBase = true };
            parser.Setup(t => t.GetHolderNode(task)).Returns(holderNode);

            // Act
            var result = parser.Object.ParseHolderInfo(task);

            // Assert
            Assert.AreEqual(result, holderInfo);
        }

        [Test]
        public void ParseLicenseInfoTest()
        {
            // Arrange
            var licInfo = Mock.Of<ContentNode>();
            var licenseInfo = Mock.Of<LicenseInfo>();
            var task = Mock.Of<Task>();
            var contentMapper = Mock.Of<IContentMapper>(t => t.MapLicenseInfo(licInfo) == licenseInfo);
            var parser = new Mock<Parser>(Mock.Of<IContentImporter>(), contentMapper) { CallBase = true };
            parser.Setup(t => t.GetLicenseNode(task)).Returns(licInfo);

            // Act
            var result = parser.Object.ParseLicenseInfo(task);

            // Assert
            Assert.AreEqual(result, licenseInfo);
        }

        [Test]
        public void ParseLicensedObjectsTest()
        {
            // Arrange
            var licObjNode = Mock.Of<ContentNode>();
            var licObj = Mock.Of<LicenseObject>();
            var task = Mock.Of<Task>();
            var importer = Mock.Of<IContentImporter>();
            var loader = Mock.Of<IContentMapper>();
            var parser = new Mock<Parser>(importer, loader) {CallBase = true};
            parser.Setup(t => t.GetLincenseObjectNodes(task)).Returns(new List<ContentNode> { licObjNode });
            parser.Setup(t => t.ParseLicenseObject(licObjNode)).Returns(licObj);
            
            // Act
            var result = parser.Object.ParseLicenseObjects(task);

            // Assert
            Assert.AreEqual(result.Single(), licObj);
        }

        [Test]
        public void ParseLicenseObjectTest()
        {
            // Arrange
            var licObjNode = Mock.Of<ContentNode>();
            var addressNode = Mock.Of<ContentNode>();
            var address = Mock.Of<Address>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNode(licObjNode, "ActivityAddress") == addressNode);
            var loader = Mock.Of<IContentMapper>(t => t.MapAddress(addressNode) == address);
            var parser = new Mock<Parser>(importer, loader) { CallBase = true };

            // Act
            var result = parser.Object.ParseLicenseObject(licObjNode);

            // Assert
            Assert.AreEqual(result.Address, address);
            parser.Verify(t => t.CompleteLicenseObject(licObjNode, result), Times.Once);
        }
    }
}