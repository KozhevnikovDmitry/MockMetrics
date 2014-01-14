using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class LiteralExpressionEaterTests
    {
        [Test]
        public void ReturnDataTest()
        {
            // Arrange
            var literalExpression = Mock.Of<ICSharpLiteralExpression>();
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var expressionHelper =
                Mock.Of<EatExpressionHelper>(t => t.IsStandaloneLiteralExpression(literalExpression) == false);
            var literalExpressionEater = new LiteralExpressionEater(eater, expressionHelper);

            // Act
            var metrics = literalExpressionEater.Eat(snapshot.Object, literalExpression);

            // Assert
            Assert.AreEqual(metrics, Variable.Library);
            snapshot.Verify(t => t.AddVariable(literalExpression, metrics), Times.Never);
        }

        [Test]
        public void AddStandaloneLiteralExpressionToSnapshotTest()
        {
            // Arrange
            var literalExpression = Mock.Of<ICSharpLiteralExpression>();
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var expressionHelper =
                Mock.Of<EatExpressionHelper>(t => t.IsStandaloneLiteralExpression(literalExpression) == true);
            var literalExpressionEater = new LiteralExpressionEater(eater, expressionHelper);

            // Act
            var metrics = literalExpressionEater.Eat(snapshot.Object, literalExpression);

            // Assert
            Assert.AreEqual(metrics, Variable.Library);
            snapshot.Verify(t => t.AddVariable(literalExpression, metrics), Times.Once);
        }
    }
}
