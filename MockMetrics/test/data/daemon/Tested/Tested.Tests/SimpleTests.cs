using NUnit.Framework;

namespace Tested.Tests
{
    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void SimpleVariablesTest()
        {
            // Arrange
            var item = true;
            var foo = new Foo();

            // Act
            var result = foo.GetBool(item);

            // Assert
            Assert.False(result);
        }

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