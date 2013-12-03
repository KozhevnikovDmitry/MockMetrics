
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class RegularParameterDeclarationEaterTests
    {
        [Test]
        public void ReturnMetricsForParametersOfParametrizedTestTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var paramDeclaration = Mock.Of<IRegularParameterDeclaration>();
            var regularParameterEater = new RegularParameterDeclarationEater(eater);

            // Act
            var metrics = regularParameterEater.Eat(snapshot.Object, paramDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(paramDeclaration, It.Is<Metrics>(m => m.Scope == Scope.Local && m.VarType == VarType.Library && m.Aim == Aim.Data && m.Equals(metrics))));
        }
    }
}