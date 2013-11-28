using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class SwitchLabelStatementEaterTests
    {
        [Test]
        public void EatValueExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var valueExpression = Mock.Of<ICSharpExpression>();
            var switchStatement = Mock.Of<ISwitchLabelStatement>(t => t.ValueExpression == valueExpression);
            var eater = new Mock<IEater>();
            var switchStatementEater = new SwitchLabelStatementEater(eater.Object);

            // Act
            switchStatementEater.Eat(snapshot, switchStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, valueExpression), Times.Once);
        }
    }
}
