using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
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
            var awaitExpression = Mock.Of<IAwaitExpression>(t => t.Task == task);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, task, false) == ExpressionKind.None);
            var awaitExpressionEater = new AwaitExpressionEater(eater);

            // Act
            var kind = awaitExpressionEater.Eat(snapshot, awaitExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.None);
        }
    }
}