using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
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
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, operand, false) == ExpressionKind.None);
            var postfixOperatorExpressionEater = new PostfixOperatorExpressionEater(eater);

            // Act
            var kind = postfixOperatorExpressionEater.Eat(snapshot, postfixOperatorExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.None);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EatOperandAndReturnItKind_TranslateInnerEatTest(bool innerEat)
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<IPrimaryExpression>();
            var postfixOperatorExpression = Mock.Of<IPostfixOperatorExpression>(t => t.Operand == operand);
            var eater = new Mock<IEater>();
            var postfixOperatorExpressionEater = new PostfixOperatorExpressionEater(eater.Object);

            // Act
            postfixOperatorExpressionEater.Eat(snapshot, postfixOperatorExpression, innerEat);

            // Assert
            eater.Verify(t => t.Eat(snapshot, operand, innerEat), Times.Once);
        }
    }
}
