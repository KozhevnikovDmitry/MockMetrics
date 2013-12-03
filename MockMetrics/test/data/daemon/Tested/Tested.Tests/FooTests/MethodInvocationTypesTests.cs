using System;
using NUnit.Framework;

namespace Tested.Tests.FooTests
{
    [TestFixture]
    public class MethodInvocationTypesTests
    {
        private Action Act;

        private event Action Event;

        private void Method()
        {
            
        }

        [Test]
        public void MethodInvocationTypesTest()
        {
            // Delegate
            Act();

            // Event
            Event();

            // Method
            Method();

            // Anon Method
            Action anon = delegate {  };
            anon();
        }
    }
}
