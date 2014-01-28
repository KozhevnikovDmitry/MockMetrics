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
    public class MedParserTests
    {
        [Test]
        public void LicenseServiceTypeTest()
        {
            // Act
            var parser = new MedParser(Mock.Of<IContentImporter>(), Mock.Of<IContentMapper>());

            // Assert
            Assert.AreEqual(parser.LicenseServiceType, LicenseServiceType.Med);
        }

        [Test]
        public void CompleteLicenseObjectTest()
        {
            var licObject = Mock.Of<LicenseObject>();
            var licObjNode = Mock.Of<ContentNode>();
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetNodeStrValue(licObjNode, "Works") == "Works");
            var parser = new MedParser(contentImporter, Mock.Of<IContentMapper>());

            // Act
            parser.CompleteLicenseObject(licObjNode, licObject);

            // Assert
            Assert.AreEqual(licObject.Note, "Works");
        }
    }
}