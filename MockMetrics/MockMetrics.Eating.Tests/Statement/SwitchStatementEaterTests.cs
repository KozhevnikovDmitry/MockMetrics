using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class SwitchStatementEaterTests
    {
        [Test]
        public void EatBlockTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var block = Mock.Of<IBlock>();
            var switchStatement = Mock.Of<ISwitchStatement>(t => t.Block == block);
            var eater = new Mock<IEater>();
            var switchStatementEater = new SwitchStatementEater(eater.Object);

            // Act
            switchStatementEater.Eat(snapshot, switchStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, block), Times.Once);
        }

        [Test]
        public void EatConditionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var switchStatement = Mock.Of<ISwitchStatement>(t => t.Condition == condition);
            var eater = new Mock<IEater>();
            var switchStatementEater = new SwitchStatementEater(eater.Object);

            // Act
            switchStatementEater.Eat(snapshot, switchStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, condition, false), Times.Once);
        }

        [Test]
        public void AddConditionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var switchStatement = Mock.Of<ISwitchStatement>(t => t.Condition == condition);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, condition, false) == ExpressionKind.None);
            var switchStatementEater = new SwitchStatementEater(eater);

            // Act
            switchStatementEater.Eat(snapshot.Object, switchStatement);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, condition), Times.Once);
        } 
    }
}