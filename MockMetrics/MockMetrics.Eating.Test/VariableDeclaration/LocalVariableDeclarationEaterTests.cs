using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Test.VariableDeclaration
{
    [TestFixture]
    public class LocalVariableDeclarationEaterTests
    {
        [Test]
        public void EatVariableInitializerTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var initializer = Mock.Of<IVariableInitializer>();
            var localConstantDeclaration = Mock.Of<ILocalVariableDeclaration>(t => t.Initial == initializer);
            var eater = Mock.Of<IEater>();
            var initializerEater = new Mock<IVariableInitializerEater>();
            var localConstantDeclarationEater = new LocalVariableDeclarationEater(eater, initializerEater.Object);

            // Act
            localConstantDeclarationEater.Eat(snapshot, localConstantDeclaration);

            // Assert
            initializerEater.Verify(t => t.Eat(snapshot, initializer), Times.Once);
        }

        [Test]
        public void AddLocalVariableToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var initializer = Mock.Of<IVariableInitializer>();
            var localConstantDeclaration = Mock.Of<ILocalVariableDeclaration>(t => t.Initial == initializer);
            var eater = Mock.Of<IEater>();
            var initializerEater = Mock.Of<IVariableInitializerEater>(t => t.Eat(snapshot.Object, initializer) == ExpressionKind.Stub);
            var localConstantDeclarationEater = new LocalVariableDeclarationEater(eater, initializerEater);

            // Act
            localConstantDeclarationEater.Eat(snapshot.Object, localConstantDeclaration);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Stub, localConstantDeclaration), Times.Once);
        }
    }
}