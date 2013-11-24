using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class DoStatementEaterTests 
    {

        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<ICSharpStatement>();
            var doStatement = Mock.Of<IDoStatement>(t => t.Body == body);
            var eater = new Mock<IEater>();
            var doStatementEater = new DoStatementEater(eater.Object);

            // Act
            doStatementEater.Eat(snapshot, doStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }

        [Test]
        public void EatConditionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var doStatement = Mock.Of<IDoStatement>(t => t.Condition == condition);
            var eater = new Mock<IEater>();
            var doStatementEater = new DoStatementEater(eater.Object);

            // Act
            doStatementEater.Eat(snapshot, doStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, condition, false), Times.Once);
        }

        [Test]
        public void AddConditionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var doStatement = Mock.Of<IDoStatement>(t => t.Condition == condition);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, condition, false) == ExpressionKind.None);
            var doStatementEater = new DoStatementEater(eater);

            // Act
            doStatementEater.Eat(snapshot.Object, doStatement);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, condition), Times.Once);
        } 
    }
}