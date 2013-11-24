using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
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
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var expressionStatementEater = new ExpressionStatementEater(eater.Object);

            // Act
            expressionStatementEater.Eat(snapshot, statement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression, false), Times.Once);
        }
    }
}
