using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ReferenceExpressionEaterTests
    {
        [Test]
        public void EatReferenceByParentTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var expressionQualifier = Mock.Of<ICSharpExpression>();
            var referenceExpression = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == expressionQualifier);
            var parentMetrics = Metrics.Create(Scope.Local, Variable.Data);
            var currentMetrics = Metrics.Create();
            var resultMetrics = Metrics.Create();
            var eatHelper = Mock.Of<IRefereceEatHelper>(t => t.Eat(snapshot, referenceExpression) == currentMetrics);
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsMerge(currentMetrics, parentMetrics) == resultMetrics);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expressionQualifier) == parentMetrics);
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, resultMetrics);
        }

        [Test]
        public void EatReferenceWithoutParentTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var referenceExpression = Mock.Of<IReferenceExpression>(); 
            var currentMetrics = Metrics.Create();
            var resultMetrics = Metrics.Create();
            var eatHelper = Mock.Of<IRefereceEatHelper>(t => t.Eat(snapshot, referenceExpression) == currentMetrics);
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsMerge(currentMetrics, It.Is<Metrics>(m => m.Call == Call.None && m.Scope == Scope.None && m.Variable == Variable.None)) == resultMetrics);
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, resultMetrics);
        }
    }
}
