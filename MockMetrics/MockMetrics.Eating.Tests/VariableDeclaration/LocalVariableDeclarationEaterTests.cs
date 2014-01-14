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
    public class LocalVariableDeclarationEaterTests
    {
        [Test]
        public void AddLocalVariableWithInitializerToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var initial = Mock.Of<IVariableInitializer>();
            var localVariableDeclaration = Mock.Of<ILocalVariableDeclaration>(t => t.Initial == initial);
            var eater = Mock.Of<IEater>();
            var variableInitializerEater = Mock.Of<IVariableInitializerEater>();
            Mock.Get(variableInitializerEater).Setup(t => t.Eat(snapshot.Object, initial)).Returns(Variable.None);
            var localVariableDeclarationEater = new LocalVariableDeclarationEater(eater, variableInitializerEater, Mock.Of<IMetricHelper>());

            // Act
            var result = localVariableDeclarationEater.Eat(snapshot.Object, localVariableDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(localVariableDeclaration, Variable.None), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatVariableWithoutInitializerTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var type = Mock.Of<IType>();
            var localVariableDeclaration = Mock.Of<ILocalVariableDeclaration>(t => t.Type == type);
            var metricHelper = Mock.Of<IMetricHelper>();
            Mock.Get(metricHelper).Setup(t => t.MetricsForType(snapshot.Object, type)).Returns(Variable.None);
            var eater = Mock.Of<IEater>();
            var initializerEater = Mock.Of<IVariableInitializerEater>();
            var localConstantDeclarationEater = new LocalVariableDeclarationEater(eater, initializerEater, metricHelper);

            // Act
            localConstantDeclarationEater.Eat(snapshot.Object, localVariableDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(localVariableDeclaration, Variable.None), Times.Once);
        }
    }
}