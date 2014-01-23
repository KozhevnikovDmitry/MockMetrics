using System.Collections.Generic;
using System.Runtime.InteropServices;
using Moq;
using NUnit.Framework;

namespace Tested.Tests.SimpleTests
{
    [TestFixture]
    public class DynamicArgumentForOverloadMethodTests
    {
        [Test]
        public void DynamicArgumentForOverloadMethodTest()
        {
            dynamic obj = new object();

            // Assert
            Assert.AreEqual(obj, 1);
        }
    }
}