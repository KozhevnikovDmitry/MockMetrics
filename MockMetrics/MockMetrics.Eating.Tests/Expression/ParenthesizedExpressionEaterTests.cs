using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
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
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression, false) == ExpressionKind.None);
            var parenthesizedExpressionEater = new ParenthesizedExpressionEater(eater);

            // Act
            var kind = parenthesizedExpressionEater.Eat(snapshot, parenthesizedExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.None);
        }
    }
}