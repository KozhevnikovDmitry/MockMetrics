using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class LocalConstantDeclarationEaterTests
    {
        [Test]
        public void AddConstantToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var localConstantDeclaration = Mock.Of<ILocalConstantDeclaration>();
            var eater = Mock.Of<IEater>();
            var constantDeclarationEater = new LocalConstantDeclarationEater(eater);

            // Act
            var result = constantDeclarationEater.Eat(snapshot.Object, localConstantDeclaration);

            // Assert
            snapshot.Verify(t =>
                t.AddVariable(localConstantDeclaration,
                It.Is<Metrics>(m => m.Scope == Scope.Local && m.Variable == Variable.Data && m.Equals(result))),
                Times.Once);
        }
    }
}