﻿using System;
using NUnit.Framework;

namespace Tested.Tests.FooTests
{
    public class MyClass
    {
        public static string StringProperty { get; set; }
    }

    [TestFixture]
    public class ReferenceTypesTests
    {
        public string StringProperty { get; set; }

        public event Action Action;

        public const string Const = "const";

        [Test]
        public void ReferenceTypesTest()
        {
            // internal
            var s1 = StringProperty;

            // external 
            var s2 = MyClass.StringProperty;

            // variable
            var s3 = s1;

            // event method
            Action += ReferenceTypesTest;

            // const
            var s4 = Const;
        }
    }
}