using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
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
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, operand, false) == ExpressionKind.None);
            var prefixOperatorExpressionEater = new PrefixOperatorExpressionEater(eater);

            // Act
            var kind = prefixOperatorExpressionEater.Eat(snapshot, prefixOperatorExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.None);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EatOperandAndReturnItKind_TranslateInnerEatTest(bool innerEat)
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<IUnaryExpression>();
            var prefixOperatorExpression = Mock.Of<IPrefixOperatorExpression>(t => t.Operand == operand);
            var eater = new Mock<IEater>();
            var prefixOperatorExpressionEater = new PrefixOperatorExpressionEater(eater.Object);

            // Act
            prefixOperatorExpressionEater.Eat(snapshot, prefixOperatorExpression, innerEat);

            // Assert
            eater.Verify(t => t.Eat(snapshot, operand, innerEat), Times.Once);
        }
    }
}