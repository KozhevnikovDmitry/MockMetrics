using System;
using Moq;
using NUnit.Framework;

namespace Tested.Tests.FooTests
{
    [TestFixture]
    public class MoqStubPositioningTests
    {
        [Test]
        public void MoqStubPostioningTest()
        {
            // create
            var stb1 = Mock.Of<Bar>();

            // assign
            Bar stb2; 
            stb2 = Mock.Of<Bar>();

            // expressiom
            var stb3 = Mock.Of<Bar>() as Object;

            // argument pass
            new Foo().GetBar(Mock.Of<Bar>());

            // initilizator
            new Foo
            {
                Bar = Mock.Of<Bar>()
            };

            // anon type member
            var stb4 = new {stb = Mock.Of<Bar>()};

            //standalone
            Mock.Of<Bar>();

            //array
            var arr = new Bar[] { Mock.Of<Bar>() };
        }
    }
}