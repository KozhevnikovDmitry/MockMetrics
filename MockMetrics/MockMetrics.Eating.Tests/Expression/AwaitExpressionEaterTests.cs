using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AwaitExpressionEaterTests
    {
        [Test]
        public void EatContainingExpressionAndReturnItKindTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var task = Mock.Of<IUnaryExpression>();
            var taskMetrics = Metrics.Create();
            var awaitExpression = Mock.Of<IAwaitExpression>(t => t.Task == task);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, task) == taskMetrics);
            var awaitExpressionEater = new AwaitExpressionEater(eater);

            // Act
            var result = awaitExpressionEater.Eat(snapshot, awaitExpression);

            // Assert
            Assert.AreEqual(result, taskMetrics);
        }
    }
}