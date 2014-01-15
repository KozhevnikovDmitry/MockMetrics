using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
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
        [TestCase("WOWOWOWOWO", ExpectedException = typeof(UnexpectedMoqItIsMethodNameException))]
        public void EatItIsReturnStubAnyWayTest(string methodName)
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var eater = Mock.Of<IEater>();
            var argumentsEater = Mock.Of<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == methodName);
            var snapshot = Mock.Of<ISnapshot>();
            var moqFakeOptionEater = Mock.Of<IMoqFakeOptionEater>();
            var itIsInvocationEater = new ItIsInvocationEater(eater, eatExpressionHelper, argumentsEater, moqFakeOptionEater);

            // Act
            var result = itIsInvocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(result, Variable.Stub);
        }

        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argumentsEater = new Mock<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.It.IsAny");
            var moqFakeOptionEater = Mock.Of<IMoqFakeOptionEater>();
            var itIsInvocationEater = new ItIsInvocationEater(eater, eatExpressionHelper, argumentsEater.Object, moqFakeOptionEater);

            // Act
            itIsInvocationEater.Eat(snapshot, invocationExpression);

            // Assert
            argumentsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }
    }
}
