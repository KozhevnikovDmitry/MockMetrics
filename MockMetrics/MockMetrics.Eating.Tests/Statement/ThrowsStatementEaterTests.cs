using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class ThrowsStatementEaterTests
    {
        [Test]
        public void EatExceptionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var exception = Mock.Of<ICSharpExpression>();
            var throwStatement = Mock.Of<IThrowStatement>(t => t.Exception == exception);
            var eater = new Mock<IEater>();
            var throwStatementEater = new ThrowStatementEater(eater.Object);

            // Act
            throwStatementEater.Eat(snapshot, throwStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, exception, false), Times.Once);
        }

        [Test]
        public void AddExceptionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var exception = Mock.Of<ICSharpExpression>();
            var throwStatement = Mock.Of<IThrowStatement>(t => t.Exception == exception);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, exception, false) == ExpressionKind.None);
            var throwStatementEater = new ThrowStatementEater(eater);

            // Act
            throwStatementEater.Eat(snapshot.Object, throwStatement);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, exception), Times.Once);
        } 
    }
}
