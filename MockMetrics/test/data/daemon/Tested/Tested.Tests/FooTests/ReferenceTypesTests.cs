using System;
using NUnit.Framework;

namespace Tested.Tests.FooTests
{
    public struct MyStruct
    {
         public static string StringProperty { get; set; }
    }

    public enum MyEnum
    {
         Item,
         MoreItem
    }

    public class MyClass
    {
        public static string StringProperty { get; set; }
    }

    [TestFixture]
    public class ReferenceTypesTests
    {
        public string StringProperty { get; set; }

        public event Action<string> Action;

        public const string Const = "const";

        public string StringField;

        [TestCase("sdfdsf")]
        public void ReferenceTypesTest(string prmt)
        {
            // internal
            var s1 = StringProperty;

            // external 
            var s2 = MyClass.StringProperty;
            
            // external struct
            var s3 = MyStruct.StringProperty;

            // parameter
            var s4 = prmt;

            // external enum
            var s5 = MyEnum.Item;

            //field
            var s6 = StringField;

            // variable
            var s7 = s1;

            // event method
            Action += ReferenceTypesTest;

            // const
            var s8 = Const;

            // local const
            const string s = "dsfsdf";
            var s9 = s;

            //lambda
            Action<int> act1 = i => { var t = i; };

            //anon param
            Action<int> act2 = delegate(int i) { var t = i; };
        }


    }
}