using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class ReturnStatementEaterTests
    {
        [Test]
        public void EatValueTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var value = Mock.Of<ICSharpExpression>();
            var returnStatement = Mock.Of<IReturnStatement>(t => t.Value == value);
            var eater = new Mock<IEater>();
            var returnStatementEater = new ReturnStatementEater(eater.Object);

            // Act
            returnStatementEater.Eat(snapshot, returnStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, value), Times.Once);
        }

        [Test]
        public void AddValueToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var value = Mock.Of<ICSharpExpression>();
            var returnStatement = Mock.Of<IReturnStatement>(t => t.Value == value);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, value) == ExpressionKind.None);
            var returnStatementEater = new ReturnStatementEater(eater);

            // Act
            returnStatementEater.Eat(snapshot.Object, returnStatement);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, value), Times.Once);
        }
    }
}