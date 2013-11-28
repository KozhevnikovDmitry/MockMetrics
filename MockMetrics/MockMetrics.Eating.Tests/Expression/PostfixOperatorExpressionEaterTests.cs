using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class PostfixOperatorExpressionEaterTests
    {
        [Test]
        public void EatOperandAndReturnItKindTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<IPrimaryExpression>();
            var postfixOperatorExpression = Mock.Of<IPostfixOperatorExpression>(t => t.Operand == operand);
            var metrics = Metrics.Create();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, operand) == metrics);
            var postfixOperatorExpressionEater = new PostfixOperatorExpressionEater(eater);

            // Act
            var result = postfixOperatorExpressionEater.Eat(snapshot, postfixOperatorExpression);

            // Assert
            Assert.AreEqual(result, metrics);
        }
    }
}
