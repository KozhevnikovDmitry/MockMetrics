using Moq;
using NUnit.Framework;

namespace Tested.Tests.AggregatorTests
{
    /// <summary>
    /// Test#4
    /// </summary>
    [TestFixture]
    public class MoqStubsTests
    {
        [Test]
        public void MoqStubsTest()
        {
            // Arrange
            var depend = Mock.Of<IDepend>();
            var another = Mock.Of<IAnother>(t => t.Calabanga(It.IsAny<string>()) == 100500);
            var aggr = new Aggregator(depend, another);

            // Act
            var result1 = aggr.Aggregate("WAKAWAKA");

            // Assert
            Assert.AreEqual(result1, 100500);
        }
    }
}