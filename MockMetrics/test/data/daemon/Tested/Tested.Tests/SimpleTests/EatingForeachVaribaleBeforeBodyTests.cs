using System.Collections.Generic;
using NUnit.Framework;

namespace Tested.Tests.SimpleTests
{
    [TestFixture]
    public class EatingForeachVaribaleBeforeBodyTests
    {
        [Test]
        public void EatingForeachVaribaleBeforeBodyTest()
        {
            var list = new List<string>();

            foreach (var item in list)
            {
                Assert.AreEqual(item, null);
            }
        }
    }
}