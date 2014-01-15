using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqFake;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class InvocationExpressionEaterTests
    {
        [Test]
        public void EatMoqInvocationTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var invocationStuffEater = Mock.Of<IInvocationStuffEater>();
            var mockEater = Mock.Of<IMoqInvocationEater>(t => t.Eat(snapshot, invocationExpression) == Variable.None && t.ContainsFakeOptions(invocationExpression));
            var invocationEater = new InvocationExpressionEater(eater, invocationStuffEater, mockEater);

            // Act
            var result = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatNotMoqInvocationTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var invocationStuffEater = Mock.Of<IInvocationStuffEater>(t => t.Eat(snapshot, invocationExpression) == Variable.None);
            var mockEater = Mock.Of<IMoqInvocationEater>(t => t.ContainsFakeOptions(invocationExpression) == false);
            var invocationEater = new InvocationExpressionEater(eater, invocationStuffEater, mockEater);

            // Act
            var result = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

    }
}
