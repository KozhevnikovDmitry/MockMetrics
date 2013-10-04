using System;
using JetBrains.Annotations;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    public class Foo
    {
        public bool GetBool(bool item)
        {
            return !item;
        }
    }

    [TestFixture]
    public class FooTests
    {
        [Obsolete]
        [NotNull]
        [Test]
        public void GetBoolTest()
        {
            // Arrange
            var foo = new Foo();

            // Act
            var result = foo.GetBool(true);

            // Assert
            Assert.False(result);
        }
    }
}