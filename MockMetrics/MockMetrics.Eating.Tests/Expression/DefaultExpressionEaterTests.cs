using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class DefaultExpressionEaterTests
    {
        [Test]
        public void EatDynamicAsStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var typeUsage = Mock.Of<IDynamicTypeUsage>();
            var defaultExpression = Mock.Of<IDefaultExpression>(t => t.TypeName == typeUsage);
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsForType(snapshot, typeUsage) == Variable.None);
            var eater = Mock.Of<IEater>();
            var defaultExpressionEater = new DefaultExpressionEater(eater, metricHelper);

            // Act
            var result = defaultExpressionEater.Eat(snapshot, defaultExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }
    }
}
