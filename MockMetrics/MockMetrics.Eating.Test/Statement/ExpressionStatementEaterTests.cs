using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Test.Statement
{
    [TestFixture]
    public class ExpressionStatementEaterTests
    {
        [Test]
        public void EatAndAddToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var statement = Mock.Of<IExpressionStatement>(t => t.Expression == expression);
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == ExpressionKind.Assert);
            var expressionStatementEater = new ExpressionStatementEater(eater);

            // Act
            expressionStatementEater.Eat(snapshot.Object, statement);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Assert, statement));
        }
    }
}
