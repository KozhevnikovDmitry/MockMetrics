using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class LocalConstantDeclarationEaterTests
    {
        [Test]
        public void AddConstantToSnapshotAsStubTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var localConstantDeclaration = Mock.Of<ILocalConstantDeclaration>();
            var eater = Mock.Of<IEater>();
            var localConstantDeclarationEater = new LocalConstantDeclarationEater(eater);

            // Act
            localConstantDeclarationEater.Eat(snapshot.Object, localConstantDeclaration);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Stub, localConstantDeclaration), Times.Once);
        }
    }
}