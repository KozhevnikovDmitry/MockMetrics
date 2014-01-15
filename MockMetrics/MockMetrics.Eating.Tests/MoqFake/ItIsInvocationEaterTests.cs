using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqFake;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.MoqFake
{
    [TestFixture]
    public class ItIsInvocationEaterTests
    {
        [TestCase("Method:Moq.It.IsAny")]
        [TestCase("Method:Moq.It.IsIn")]
        [TestCase("Method:Moq.It.IsInRange")]
        [TestCase("Method:Moq.It.IsNotNull")]
        [TestCase("Method:Moq.It.IsNotIn")]
        [TestCase("Method:Moq.It.IsRegex")]
        [TestCase("Method:Moq.It.Is")]
        [TestCase("WOWOWOWOWO", ExpectedException = typeof(UnexpectedMoqItIsMethodNameException))]
        public void EatIsAnyTest(string methodName)
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var eater = Mock.Of<IEater>();
            var argumentsEater = Mock.Of<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == methodName);
            var snapshot = Mock.Of<ISnapshot>();
            var itIsInvocationEater = new ItIsInvocationEater(eater, eatExpressionHelper, argumentsEater);

            // Act
            var result = itIsInvocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(result, Variable.Stub);
            It.Is<string>(t => t.IsNormalized());
        }
    }
}
