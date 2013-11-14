using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class LockStatementEaterTests 
    {
        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<ICSharpStatement>();
            var lockStatement = Mock.Of<ILockStatement>(t => t.Body == body);
            var eater = new Mock<IEater>();
            var doStatementEater = new LockStatementEater(eater.Object);

            // Act
            doStatementEater.Eat(snapshot, lockStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }

        [Test]
        public void EatConditionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var monitor = Mock.Of<ICSharpExpression>();
            var lockStatement = Mock.Of<ILockStatement>(t => t.Monitor == monitor);
            var eater = new Mock<IEater>();
            var lockStatementEater = new LockStatementEater(eater.Object);

            // Act
            lockStatementEater.Eat(snapshot, lockStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, monitor), Times.Once);
        }

        [Test]
        public void AddConditionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var monitor = Mock.Of<ICSharpExpression>();
            var lockStatement = Mock.Of<ILockStatement>(t => t.Monitor == monitor);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, monitor) == ExpressionKind.None);
            var lockStatementEater = new LockStatementEater(eater);

            // Act
            lockStatementEater.Eat(snapshot.Object, lockStatement);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.None, monitor), Times.Once);
        } 
    }
}