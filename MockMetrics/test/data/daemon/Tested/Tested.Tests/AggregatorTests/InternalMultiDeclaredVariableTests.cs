using NUnit.Framework;

namespace Tested.Tests.AggregatorTests
{
    public interface IParent
    {
        Aggregator Aggregator { get; set; }
    }

    public class Parent : IParent
    {
        public virtual Aggregator Aggregator { get; set; }
    }

    [TestFixture]
    public class InternalMultiDeclaredVariableTests : Parent
    {
        public override Aggregator Aggregator { get; set; }

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