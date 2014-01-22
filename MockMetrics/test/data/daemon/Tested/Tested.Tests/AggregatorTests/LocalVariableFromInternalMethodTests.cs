using NUnit.Framework;

namespace Tested.Tests.AggregatorTests
{
    [TestFixture]
    public class LocalVariableFromInternalMethodTests
    {
        private Aggregator GetAggregator()
        {
            return null;
        }

        [Test]
        public void LocalVariableFromInternalMethodTest()
        {
            var result = GetAggregator();

            Assert.AreEqual(result, 1);
        }
    }
}