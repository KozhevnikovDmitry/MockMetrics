using NUnit.Framework;

namespace Tested.Tests.AggregatorTests
{
    [TestFixture]
    public class InternalFieldVariableTests
    {
        private Aggregator Aggregator;

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