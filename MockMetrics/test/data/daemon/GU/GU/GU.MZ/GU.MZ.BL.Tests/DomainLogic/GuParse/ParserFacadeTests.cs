using System.Collections.Generic;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.GuParse
{
    [TestFixture]
    public class ParserFacadeTests
    {
        [Test]
        public void NoParserForLicenseTypeTest()
        {
            // Arrange
            var parserFacade = new ParserFacade(new List<IParserImpl>());
            var task = Mock.Of<Task>();
            var mzTask = Mock.Of<IMzTaskWrapper>();
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);

            // Assert
            Assert.Throws<NoParserForLicenseTypeException>(() => parserFacade.ParseHolder(task));
            Assert.Throws<NoParserForLicenseTypeException>(() => parserFacade.ParseHolderInfo(task));
            Assert.Throws<NoParserForLicenseTypeException>(() => parserFacade.ParseLicense(task));
            Assert.Throws<NoParserForLicenseTypeException>(() => parserFacade.ParseLicenseInfo(task));
        }
        
        [Test]
        public void MultipleParserForLicenseTypeTest()
        {
            // Arrange
            var parser1 = Mock.Of<IParserImpl>(t => t.LicenseServiceType == LicenseServiceType.Drug);
            var parser2 = Mock.Of<IParserImpl>(t => t.LicenseServiceType == LicenseServiceType.Drug);
            var parserFacade = new ParserFacade(new List<IParserImpl> { parser1, parser2 });
            var task = Mock.Of<Task>();
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseServiceType == LicenseServiceType.Drug);
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);

            // Assert
            Assert.Throws<MultipleParserForLicenseTypeException>(() => parserFacade.ParseHolder(task));
            Assert.Throws<MultipleParserForLicenseTypeException>(() => parserFacade.ParseHolderInfo(task));
            Assert.Throws<MultipleParserForLicenseTypeException>(() => parserFacade.ParseLicense(task));
            Assert.Throws<MultipleParserForLicenseTypeException>(() => parserFacade.ParseLicenseInfo(task));
        }

        [Test]
        public void ParseHolderTest()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseServiceType == LicenseServiceType.Drug);
            var holder = Mock.Of<LicenseHolder>();
            var parser = Mock.Of<IParserImpl>(t => t.LicenseServiceType == LicenseServiceType.Drug
                                                && t.ParseHolder(task) == holder);
            var parserFacade = new ParserFacade(new List<IParserImpl> { parser });
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);

            // Act
            var result = parser.ParseHolder(task);

            // Assert
            Assert.AreEqual(result, holder);
        }

        [Test]
        public void ParseHolderInfoTest()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseServiceType == LicenseServiceType.Drug);
            var holderInfo = Mock.Of<HolderInfo>();
            var parser = Mock.Of<IParserImpl>(t => t.LicenseServiceType == LicenseServiceType.Drug
                                                && t.ParseHolderInfo(task) == holderInfo);
            var parserFacade = new ParserFacade(new List<IParserImpl> { parser });
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);

            // Act
            var result = parser.ParseHolderInfo(task);

            // Assert
            Assert.AreEqual(result, holderInfo);
        }

        [Test]
        public void ParseLicenseTest()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseServiceType == LicenseServiceType.Drug);
            var license = Mock.Of<License>();
            var parser = Mock.Of<IParserImpl>(t => t.LicenseServiceType == LicenseServiceType.Drug
                                                && t.ParseLicense(task) == license);
            var parserFacade = new ParserFacade(new List<IParserImpl> { parser });
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);

            // Act
            var result = parser.ParseLicense(task);

            // Assert
            Assert.AreEqual(result, license);
        }

        [Test]
        public void ParseLicenseInfoTest()
        {
            // Arrange
            var task = Mock.Of<Task>();
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseServiceType == LicenseServiceType.Drug);
            var licenseInfo = Mock.Of<LicenseInfo>();
            var parser = Mock.Of<IParserImpl>(t => t.LicenseServiceType == LicenseServiceType.Drug
                                                && t.ParseLicenseInfo(task) == licenseInfo);
            var parserFacade = new ParserFacade(new List<IParserImpl> { parser });
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);

            // Act
            var result = parser.ParseLicenseInfo(task);

            // Assert
            Assert.AreEqual(result, licenseInfo);
        }
    }
}