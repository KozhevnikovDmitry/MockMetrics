using NUnit.Framework;

namespace Tested.Tests
{
    [TestFixture]
    public class SimpleVariablesTests
    {
        public Foo foo1 { get; set; }

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
    }
}