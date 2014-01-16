using System;
using System.Text.RegularExpressions;
using Moq;
using NUnit.Framework;

namespace Tested.Tests.AggregatorTests
{
    [TestFixture]
    public class MoqSyntaxTests
    {
        [Test]
        public void MoqSyntaxTest()
        {
            IAnother obj = null;
            var mock = Mock.Get(obj);
            mock.VerifyGet(t => t.Mamba, Times.Once);

            Mock.Of<IAnother>();
            Mock.Of<IAnother>(t => t.Mamba == "1");
            mock.Setup(t => t.Calabanga());
            mock.Setup(t => t.Mamba);
            mock.SetupAllProperties();
            mock.SetupGet(t => t.Mamba);
            mock.SetupSet(t => t.Mamba);
            mock.SetupProperty(t => t.Mamba);
            mock.SetupProperty(t => t.Mamba, "dsf");

            mock.VerifyGet(t => t.Mamba);
            mock.VerifyGet(t => t.Mamba, "sdfsdf");
            mock.VerifyGet(t => t.Mamba, new Times(), "sdfsdf");
            mock.VerifyGet(t => t.Mamba, Times.Once);

            mock.Verify(t => t.Calabanga());
            mock.Verify(t => t.Calabanga(), "sdfsdf");
            mock.Verify(t => t.Calabanga(), new Times(), "sdfsdf");
            mock.Verify(t => t.Calabanga(), Times.Once);

            mock.Setup(t => t.Calabanga("ads")).Returns(1).Callback(() => obj.Calabanga());
            mock.Setup(t => t.Calabanga("ads")).Callback(() => obj.Calabanga()).Returns(1);
            mock.Setup(t => t.Calabanga("ads")).Throws(new Exception());
            mock.Setup(t => t.Calabanga("ads")).Throws<Exception>();

            It.IsAny<IAnother>();
            It.IsNotNull<IAnother>();
            It.IsIn<IAnother>();
            It.IsIn<IAnother>(new IAnother[] { });
            It.IsNotIn<IAnother>();
            It.IsNotIn<IAnother>(new IAnother[] { });
            It.IsInRange(0,10, Range.Inclusive);
            It.IsRegex("sdsdf");
            It.IsRegex("sdsdf", RegexOptions.Compiled);
            It.Is<IAnother>(t => t.Mamba == "");
        }
    }
}