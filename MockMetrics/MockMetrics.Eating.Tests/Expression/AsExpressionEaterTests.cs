using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AsExpressionEaterTests
    {
        [Test]
        public void AddToSnapshotTypeUsageTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var operand = Mock.Of<ICSharpExpression>();
            var typeOperand = Mock.Of<ITypeUsage>();
            var asExpression = Mock.Of<IAsExpression>(t => t.Operand == operand && t.TypeOperand == typeOperand);
            var eater = Mock.Of<IEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var asExpressionEater = new AsExpressionEater(eater, metricHelper);

            // Act
            asExpressionEater.Eat(snapshot.Object, asExpression);

            // Assert
            snapshot.Verify(t => 
                t.AddVariable(typeOperand, Variable.Library),
                             Times.Once);
        }

        [Test]
        public void EatExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<ICSharpExpression>();
            var typeOperand = Mock.Of<ITypeUsage>();
            var asExpression = Mock.Of<IAsExpression>(t => t.Operand == operand && t.TypeOperand == typeOperand);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, operand) == Variable.None);
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsForCasted(snapshot, Variable.None, typeOperand) == Variable.Stub);
            var asExpressionEater = new AsExpressionEater(eater, metricHelper);

            // Act
            var result = asExpressionEater.Eat(snapshot, asExpression);
            
            // Assert
            Assert.AreEqual(result, Variable.Stub);
        }
    }
}