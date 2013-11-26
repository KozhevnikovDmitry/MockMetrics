using Moq;
using NUnit.Framework;

namespace Tested.Tests.AggregatorTests
{
    /// <summary>
    /// Test#7
    /// </summary>
    [TestFixture]
    public class NestedMoqStubsTests
    {
        [Test]
        public void NestedMoqStubsTest()
        {
            // Arrange
            var detail = Mock.Of<Detail>();
            var master = Mock.Of<Master>();
            var aggr = Mock.Of<Aggregator>(t => t.Service == Mock.Of<IService>() 
                                             && t.GetMaster(detail) == master
                                             && t.Serial == 100500);

            // Act
            var result = aggr.GetMaster(detail);

            // Assert
            Assert.AreEqual(result, detail);
            Assert.AreEqual(aggr.Serial, "100500");
        }
    }
}