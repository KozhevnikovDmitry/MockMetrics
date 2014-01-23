using System;
using NUnit.Framework;

namespace Tested.Tests.SimpleTests
{
    [TestFixture]
    public class EatingCatchVaribaleBeforeBodyTests
    {
        [Test]
        public void EatingForeachVaribaleBeforeBodyTest()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex, null);
            }
        }
    }
}