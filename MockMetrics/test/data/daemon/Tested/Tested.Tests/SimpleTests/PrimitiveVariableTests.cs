﻿using NUnit.Framework;

namespace Tested.Tests.SimpleTests
{
    [TestFixture]
    public class PrimitiveVariableTests
    {
        [Test]
        public void PrimitiveVariableTest()
        {
            var str = "string";
            var i = str.Length;
            var summ = i + i;
            var tos = summ.ToString();
        }
    }
}