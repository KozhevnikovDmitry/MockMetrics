using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
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
            eater.Verify(t => t.Eat(snapshot, condition), Times.Once);
        }
    }
}