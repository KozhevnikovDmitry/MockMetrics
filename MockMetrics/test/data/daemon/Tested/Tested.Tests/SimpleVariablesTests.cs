using System;
using System.Linq;
using NUnit.Framework;

namespace Tested.Tests
{
    public class a
    {
        protected string S1 { get; set; }

        protected string S2 { get; set; }

        protected const string S3 = "";
    }

    [TestFixture]
    public class SimpleVariablesTests : a
    {
        public const int foo2 = 1;
        public Foo foo1 { get; set; }
        public Foo foo3;
        public event Func<int> aa; 

        [Test]
        public void SimpleVariablesTest()
        {
            var d = from t1 in new String[10]
                    let l = t1.Length
                    group t1 by l
                    into len
                    group len by len.ToString().First()
                    into f
                    let ff = f.Key
                    select new { f, ff };

            // Arrange
            var item = true;
            var foo = new Foo();

            // Act
            var result = foo.GetBool(item);

            // Assert
            Assert.False(result);
        }
    }
}