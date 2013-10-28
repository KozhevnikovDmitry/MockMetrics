using MockMetrics.Fake.Tested;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    [TestFixture]
    public class FooTests
    {
        [Test]
        public void GetBoolTest()
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