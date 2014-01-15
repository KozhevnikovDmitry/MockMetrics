using Moq;
using NUnit.Framework;

namespace Tested.Tests.FooTests
{
    [TestFixture]
    public class ItIsFakeOptionTests
    {
        [Test]
        public void ItIsFakeOptionTest()
        {
            var fake2 = It.Is<IAnother>(t => t.Calabanga("sad") == 1 && t.Mamba == "sdfdsf");
        }
    }
}