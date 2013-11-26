using Moq;
using NUnit.Framework;

namespace Tested.Tests
{
    /// <summary>
    /// Test#2
    /// </summary>
    [TestFixture]
    public class StubTests
    {
        [Test]
        public void ExpressionVariablesTest()
        {
            // Arrange
            var stub = Mock.Of<Bar>();
            var foo = new Foo();

            // Act
            var result = foo.GetBool(stub.SomeBool);

            // Assert
            Assert.False(result);
        }
    }
}