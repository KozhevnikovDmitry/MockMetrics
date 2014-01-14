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
    public class ForeachVariableDeclarationEaterTests
    {
        [Test]
        public void AddForeachVariableToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var type = Mock.Of<IType>();
            var foreachVariableDeclaration = Mock.Of<IForeachVariableDeclaration>(t => t.Type == type);
            var eater = Mock.Of<IEater>();
            var helper = Mock.Of<IMetricHelper>();
            Mock.Get(helper).Setup(t => t.MetricsForType(snapshot.Object, type)).Returns(Variable.None);
            var foreachVariableDeclarationEater = new ForeachVariableDeclarationEater(eater, helper);

            // Act
            var result = foreachVariableDeclarationEater.Eat(snapshot.Object, foreachVariableDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(foreachVariableDeclaration, Variable.None), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }
    }
}