using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class PrefixOperatorExpressionEaterTests
    {
        [Test]
        public void EatOperandAndReturnItKindTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<IUnaryExpression>();
            var prefixOperatorExpression = Mock.Of<IPrefixOperatorExpression>(t => t.Operand == operand);
            var metrics = Metrics.Create();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, operand) == metrics);
            var prefixOperatorExpressionEater = new PrefixOperatorExpressionEater(eater);

            // Act
            var result = prefixOperatorExpressionEater.Eat(snapshot, prefixOperatorExpression);

            // Assert
            Assert.AreEqual(result, metrics);
        }
    }
}