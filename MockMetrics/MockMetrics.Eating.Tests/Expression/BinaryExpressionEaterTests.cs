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
            var snapshot = Mock.Of<ISnapshot>();
            var right = Mock.Of<ICSharpExpression>();
            var left = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IBinaryExpression>(t => t.LeftOperand == left && t.RightOperand == right);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, right) == Variable.None
                                          && t.Eat(snapshot, left) == Variable.Stub);

            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsMerge(Variable.Stub, Variable.None) == Variable.Service);
            var binaryExpressionEater = new BinaryExpressionEater(eater, metricHelper);

            // Act
            var result = binaryExpressionEater.Eat(snapshot, assignmentExpression);

            // Assert
            Assert.That(result, Is.EqualTo(Variable.Service));
        }
    }
}
