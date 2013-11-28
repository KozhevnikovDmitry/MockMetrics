using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class GotoCaseStatementEaterTests
    {
        [Test]
        public void EatValueExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var value = Mock.Of<ICSharpExpression>();
            var gotoCaseStatement = Mock.Of<IGotoCaseStatement>(t => t.ValueExpression == value);
            var eater = new Mock<IEater>();
            var gotoCaseStatementEater = new GotoCaseStatementEater(eater.Object);

            // Act
            gotoCaseStatementEater.Eat(snapshot, gotoCaseStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, value), Times.Once);
        }
    }
}