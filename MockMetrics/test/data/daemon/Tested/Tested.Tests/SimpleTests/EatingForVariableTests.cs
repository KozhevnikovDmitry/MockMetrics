using System.Collections.Generic;
using NUnit.Framework;

namespace Tested.Tests.SimpleTests
{
    [TestFixture]
    public class EatingForVariableTests
    {
        [Test]
        public void EatingForVariableTest()
        {
            var list = new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i], null);
            }

            int j = 0;
            for (j = 0; j < list.Count; j++)
            {
                Assert.AreEqual(list[j], null);
            }
        }
    }
}
