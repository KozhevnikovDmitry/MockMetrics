using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class LambdaParameterDeclarationEaterTests
    {
        [Test]
        public void AddLamdaParameterToSnapshotAsVariableTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var type = Mock.Of<IType>();
            var lambdaParameterDeclaration = Mock.Of<ILambdaParameterDeclaration>(t => t.Type == type);
            var eater = Mock.Of<IEater>();
            var helper = Mock.Of<IMetricHelper>();
            var metrics = Metrics.Create();
            Mock.Get(helper).Setup(t => t.MetricsForType(snapshot.Object, type)).Returns(metrics);
            var lambdaParameterDeclarationEater = new LambdaParameterDeclarationEater(eater, helper);

            // Act
            var result = lambdaParameterDeclarationEater.Eat(snapshot.Object, lambdaParameterDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(lambdaParameterDeclaration, metrics), Times.Once);
            Assert.AreEqual(result, metrics);
            Assert.AreEqual(result.Scope, Scope.Local);
        }
    }
}