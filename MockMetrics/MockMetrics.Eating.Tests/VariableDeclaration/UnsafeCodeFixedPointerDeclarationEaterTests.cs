using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class UnsafeCodeFixedPointerDeclarationEaterTests
    {
        [Test]
        public void EatPointerInitializerTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var initializer = Mock.Of<IVariableInitializer>();
            var codeFixedPointerDeclaration = Mock.Of<IUnsafeCodeFixedPointerDeclaration>(t => t.Initial == initializer);
            var eater = Mock.Of<IEater>();
            var initializerEater = new Mock<IVariableInitializerEater>();
            var codeFixedPointerDeclarationEater = new UnsafeCodeFixedPointerDeclarationEater(eater, initializerEater.Object);

            // Act
            codeFixedPointerDeclarationEater.Eat(snapshot, codeFixedPointerDeclaration);

            // Assert
            initializerEater.Verify(t => t.Eat(snapshot, initializer), Times.Once);
        }

        [Test]
        public void AddPointerVariableToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var initializer = Mock.Of<IVariableInitializer>();
            var codeFixedPointerDeclaration = Mock.Of<IUnsafeCodeFixedPointerDeclaration>(t => t.Initial == initializer);
            var eater = Mock.Of<IEater>();
            var initializerEater = Mock.Of<IVariableInitializerEater>(t => t.Eat(snapshot.Object, initializer) == ExpressionKind.Stub);
            var codeFixedPointerDeclarationEater = new UnsafeCodeFixedPointerDeclarationEater(eater, initializerEater);

            // Act
            codeFixedPointerDeclarationEater.Eat(snapshot.Object, codeFixedPointerDeclaration);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.Stub, codeFixedPointerDeclaration), Times.Once);
        }
    }
}