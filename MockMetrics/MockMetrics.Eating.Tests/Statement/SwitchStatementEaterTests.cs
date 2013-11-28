using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
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
            eater.Verify(t => t.Eat(snapshot, condition), Times.Once);
        }
    }
}