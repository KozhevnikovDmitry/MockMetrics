using Moq;
using NUnit.Framework;

namespace Tested.Tests.AggregatorTests
{
    /// <summary>
    /// Test#5
    /// </summary>
    [TestFixture]
    public class MoqAssignmentStubsTests
    {
        [Test]
        public void MoqAssignmentStubsTest()
        {
            // Arrange
            IDepend depend;
            depend = Mock.Of<IDepend>();

            IAnother another;
            another = Mock.Of<IAnother>(t => t.Calabanga("WAKAWAKA") == 100500);

            Aggregator aggr;
            aggr = new Aggregator(depend, another);

            // Act
            var result1 = aggr.Aggregate("WAKAWAKA");
            var result2 = aggr.Aggregate("WAKAWAKA");

            // Assert
            Assert.AreEqual(result1, result2);
            Assert.AreEqual(result1, 100500);
        }
    }
}