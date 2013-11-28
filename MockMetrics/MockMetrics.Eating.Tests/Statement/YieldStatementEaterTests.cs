using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class YieldStatementEaterTests
    {
        [Test]
        public void EatExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var expression = Mock.Of<ICSharpExpression>();
            var yieldStatement = Mock.Of<IYieldStatement>(t => t.Expression == expression);
            var eater = new Mock<IEater>();
            var yieldStatementEater = new YieldStatementEater(eater.Object);

            // Act
            yieldStatementEater.Eat(snapshot, yieldStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression), Times.Once);
        }
       
    }
}