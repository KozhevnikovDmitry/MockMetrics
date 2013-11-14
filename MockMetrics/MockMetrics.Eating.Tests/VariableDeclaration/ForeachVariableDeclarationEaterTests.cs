using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class ForeachVariableDeclarationEaterTests
    {
        [Test]
        public void AddForeachVariableToSnapshotAsVariableTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var foreachVariableDeclaration = Mock.Of<IForeachVariableDeclaration>();
            var eater = Mock.Of<IEater>();
            var foreachVariableDeclarationEater = new ForeachVariableDeclarationEater(eater);

            // Act
            foreachVariableDeclarationEater.Eat(snapshot.Object, foreachVariableDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(foreachVariableDeclaration), Times.Once);
        }
    }
}