using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class UnsafeCodeFixedPointerDeclarationEaterTests
    {
        [Test]
        public void AddUnsafeCodeFixedPointerToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var initial = Mock.Of<IVariableInitializer>();
            var codeFixedPointerDeclaration = Mock.Of<IUnsafeCodeFixedPointerDeclaration>(t => t.Initial == initial);
            var eater = Mock.Of<IEater>();
            var variableInitializerEater = Mock.Of<IVariableInitializerEater>();
            var metrics = Metrics.Create();
            Mock.Get(variableInitializerEater).Setup(t => t.Eat(snapshot.Object, initial)).Returns(metrics);
            var codeFixedPointerDeclarationEater = new UnsafeCodeFixedPointerDeclarationEater(eater, variableInitializerEater);

            // Act
            var result = codeFixedPointerDeclarationEater.Eat(snapshot.Object, codeFixedPointerDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(codeFixedPointerDeclaration, metrics), Times.Once);
            Assert.AreEqual(result, metrics);
            Assert.AreEqual(result.Scope, Scope.Local);
        }
    }
}