using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace Tested.Tests.AggregatorTests
{
    [TestFixture]
    public class VariablesWithInitializersTests
    {
        [Test]
        public void VariablesWithInitializersTest()
        {
            // Arrange
            var service = Mock.Of<IService>(t => t.Details == new List<Detail>());
            var aggregator = new Aggregator(Mock.Of<IDepend>(), Mock.Of<IAnother>())
            {
                Service = service,
                Serial = 1
            };

            // Act
            var details = aggregator.Aggregate();

            // Assert
            Assert.NotNull(details);

        }
    }
}