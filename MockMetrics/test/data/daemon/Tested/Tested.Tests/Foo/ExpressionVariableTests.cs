using NUnit.Framework;

namespace Tested.Tests
{
    [TestFixture]
    public class ExpressionVariableTests
    {
        [Test]
        public void ExpressionVariablesTest()
        {
            // Arrange
            var item = string.IsNullOrEmpty("WAKAWAKA");
            var foo = new Foo();

            // Act
            var result = foo.GetBool(item);

            // Assert
            Assert.False(result);
        }
    }
}