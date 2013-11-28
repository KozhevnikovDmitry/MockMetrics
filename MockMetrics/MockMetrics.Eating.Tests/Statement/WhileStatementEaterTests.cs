using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class WhileStatementEaterTests
    {
        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<ICSharpStatement>();
            var whileStatement = Mock.Of<IWhileStatement>(t => t.Body == body);
            var eater = new Mock<IEater>();
            var whileStatementEater = new WhileStatementEater(eater.Object);

            // Act
            whileStatementEater.Eat(snapshot, whileStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }

        [Test]
        public void EatConditionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var whileStatement = Mock.Of<IWhileStatement>(t => t.Condition == condition);
            var eater = new Mock<IEater>();
            var whileStatementEater = new WhileStatementEater(eater.Object);

            // Act
            whileStatementEater.Eat(snapshot, whileStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, condition), Times.Once);
        }
    }
}