using Autofac;
using Autofac.Core;
using GU.MZ.BL.DomainLogic;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Content
{
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class ParseTaskTests : MzAcceptanceTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        public void ParseTaskTest(int serviceid)
        {
            // Arrange
            var contentParser = MzLogicFactory.IocContainer.Resolve<ContentParser>();
            
            // Act
            var task = contentParser.ParseTask(serviceid);
            var holderInfo = MzLogicFactory.GetTaskParser().ParseHolderInfo(task);

            // Assert
            Assert.IsNotNull(task);
            Assert.IsNotEmpty(holderInfo.Inn);
            Assert.IsNotEmpty(holderInfo.Ogrn);

        }
    }
}
