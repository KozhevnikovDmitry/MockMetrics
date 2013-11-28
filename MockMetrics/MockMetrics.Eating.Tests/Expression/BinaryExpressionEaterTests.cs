using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class BinaryExpressionEaterTests
    {
        [Test]
        public void EatOperandsAndReturnMeregedMetricsTest()
        {
            // Arrange
            var rightMetrics = Metrics.Create();
            var leftMetrics = Metrics.Create();
            var mergeMetrics = Metrics.Create();
            var snapshot = Mock.Of<ISnapshot>();
            var right = Mock.Of<ICSharpExpression>();
            var left = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IBinaryExpression>(t => t.LeftOperand == left && t.RightOperand == right);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, right) == rightMetrics
                                          && t.Eat(snapshot, left) == leftMetrics);
           
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsMerge(leftMetrics, rightMetrics) == mergeMetrics);
            var binaryExpressionEater = new BinaryExpressionEater(eater, metricHelper);

            // Act
            var result = binaryExpressionEater.Eat(snapshot, assignmentExpression);

            // Assert
            Assert.That(result, Is.EqualTo(mergeMetrics));
        }
    }
}
