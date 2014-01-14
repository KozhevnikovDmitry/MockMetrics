using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class UnaryOperatorExpressionEaterTests
    {
        [Test]
        public void EatOperandAndReturnItKindTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<IUnaryExpression>();
            var unaryOperatorExpression = Mock.Of<IUnaryOperatorExpression>(t => t.Operand == operand);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, operand) == Variable.None);
            var unaryOperatorExpressionEater = new UnaryOperatorExpressionEater(eater);

            // Act
            var result = unaryOperatorExpressionEater.Eat(snapshot, unaryOperatorExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }
    }
}