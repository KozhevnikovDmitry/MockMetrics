using NUnit.Framework;

namespace Tested.Tests.SimpleTests
{
    enum MyEnum
    {
        Some, 
        Atother, 
        AndMore
    }

    public class EnumOccurencesTests
    {
        [Test]
        public void EnumOccurencesTest()
        {
            // Arrange
            var obj = MyEnum.Some;

            // Assert
            Assert.AreEqual(obj, MyEnum.Some);
        }
    }
}
