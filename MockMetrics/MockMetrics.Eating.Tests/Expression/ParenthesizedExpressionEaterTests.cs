using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ParenthesizedExpressionEaterTests
    {
        [Test]
        public void EatContainingExpressionAndReturnItKindTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var expression = Mock.Of<ICSharpExpression>();
            var parenthesizedExpression = Mock.Of<IParenthesizedExpression>(t => t.Expression == expression);
            var metrics = Metrics.Create();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression) == metrics);
            var parenthesizedExpressionEater = new ParenthesizedExpressionEater(eater);

            // Act
            var result = parenthesizedExpressionEater.Eat(snapshot, parenthesizedExpression);

            // Assert
            Assert.AreEqual(result, metrics);
        }
    }
}