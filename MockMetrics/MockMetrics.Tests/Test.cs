using System;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    public class PsiExamples
    {
        public PsiExamples()
        {
            var o1 = 0; // ICSharpLiteralExpression
            var o2 = new Object(); // IObjectCreationExpression
            var o3 = DateTime.Now; // IReferenceExpression
            var o4 = DateTime.Now.Date.Day; // IReferenceExpression IReferenceExpression
            var o5 = DateTime.Now.AddDays(1); // IInvocationExpression
            var o6 = 0 == 1; // IEqualityExpression
            var o7 = o2 is bool; // IIsExpression
            var o8 = o2 as Object; // IAsExpression
            var o9 = new { }; // IAnonymousObjectInitializer
            var o10 = new[] { "" }; // IArrayInitializer
            var o11 = o6 ? 1 : 3; // IConditionalTernaryExpression
            var o12 = o2 ?? 0; // INullCoalescingExpression
            var o13 = typeof(Object); //ITypeofExpression
            var o14 = (int) o2; // ICastException
            var o15 = o1++; // IPostfixOperatorExpression
            var o16 = !o6; // IUnaryOperatorExpression
            var o17 = --o1; // IPrefixOperatorExpression
            var o18 = 1 + 2; // IAdditiveExpression
            var o19 = 1 != 2; //IEqualityExpression
            var o20 = new DateTime?(); // 
        }
    }

    public class Inverter
    {
        public bool GetBool(bool item)
        {
            return !item;
        }
    }

    [TestFixture]
    public class InverterTests
    {
        [Test]
        public void GetBoolTest()
        {
            var item = true;
            var stub = Mock.Of<Inverter>();
            var inverter = new Inverter();

            // Act
            var result = inverter.GetBool(item);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void MockTest()
        {
            var item = true;
            var inverter = new Mock<Inverter>();

            // Act
            var result = inverter.Object.GetBool(item);

            // Assert
            inverter.Verify();
        }
    }
}
