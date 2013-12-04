using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class TypeofExpressionEaterTests
    {
        [Test]
        public void AddDataCallToSnapshotAndReturnVarTypeLibraryTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var typeofExpression = Mock.Of<ITypeofExpression>();
            var eater = Mock.Of<IEater>();
            var typeofExpressionEater = new TypeofExpressionEater(eater);

            // Act
            var metric = typeofExpressionEater.Eat(snapshot.Object, typeofExpression);

            // Assert
            snapshot.Verify(t => t.AddCall(typeofExpression, It.Is<Metrics>(m => m.Call == Call.Library)));
            Assert.AreEqual(metric.Variable, Variable.Data);
        }
    }
}