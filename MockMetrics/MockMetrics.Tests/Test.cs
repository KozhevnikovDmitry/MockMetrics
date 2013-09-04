using Moq;
using NUnit.Framework;

namespace MM.Debug
{
    public class Inverter
    {
        public bool GetBool(bool item)
        {
            return !item;
        }
    }

    [TestFixture]
    public class InverterTests
    {
        [Test]
        public void GetBoolTest()
        {
            var item = true;
            var stub = Mock.Of<Inverter>();
            var inverter = new Inverter();

            // Act
            var result = inverter.GetBool(item);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void MockTest()
        {
            var item = true;
            var inverter = new Mock<Inverter>();

            // Act
            var result = inverter.Object.GetBool(item);

            // Assert
            inverter.Verify();
        }
    }
}
