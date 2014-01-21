using NUnit.Framework;

namespace Tested.Tests.AggregatorTests
{
    [TestFixture]
    public class InternalVariableTests
    {
        private Aggregator Aggregator { get; set; }

        [SetUp]
        public void Setup()
        {
            Aggregator = new Aggregator(null, null);
        }

        [Test]
        public void InternalVariableTestTest()
        {
            var result = Aggregator.Aggregate("sdfsdf");

            Assert.AreEqual(result, 1);
        }
    }
}
