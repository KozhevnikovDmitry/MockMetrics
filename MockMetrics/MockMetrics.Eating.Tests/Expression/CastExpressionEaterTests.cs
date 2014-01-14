using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class CastExpressionEaterTests
    {
        [Test]
        public void AddToSnapshotTypeUsageTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var operand = Mock.Of<IUnaryExpression>();
            var targetType = Mock.Of<ITypeUsage>();
            var castExpression = Mock.Of<ICastExpression>(t => t.Op == operand && t.TargetType == targetType);
            var eater = Mock.Of<IEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var castExpressionEater = new CastExpressionEater(eater, metricHelper);

            // Act
            castExpressionEater.Eat(snapshot.Object, castExpression);

            // Assert
            snapshot.Verify(t =>
                t.AddVariable(targetType, Variable.Library),
                             Times.Once);
        }

        [Test]
        public void EatExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<IUnaryExpression>();
            var targetType = Mock.Of<ITypeUsage>();
            var castExpression = Mock.Of<ICastExpression>(t => t.Op == operand && t.TargetType == targetType);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, operand) == Variable.None);
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsForCasted(snapshot, Variable.None, targetType) == Variable.Library);
            var castExpressionEater = new CastExpressionEater(eater, metricHelper);

            // Act
            var result = castExpressionEater.Eat(snapshot, castExpression);

            // Assert
            Assert.AreEqual(result, Variable.Library);
        }
    }
}