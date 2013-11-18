using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
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

        [TestCase(ExpressionKind.StubCandidate, ExpressionKind.Stub)]
        [TestCase(ExpressionKind.Stub, ExpressionKind.Stub)]
        [TestCase(ExpressionKind.Target, ExpressionKind.Target)]
        [TestCase(ExpressionKind.TargetCall, ExpressionKind.Result)]
        [TestCase(ExpressionKind.Result, ExpressionKind.Result)]
        [TestCase(ExpressionKind.Mock, ExpressionKind.Mock)]
        [TestCase(ExpressionKind.Assert, ExpressionKind.Result)]
        [TestCase(ExpressionKind.None, ExpressionKind.None)]
        public void AddLocalVariableToSnapshotTest(ExpressionKind initKind, ExpressionKind variableKindMustBe)
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var initializer = Mock.Of<IVariableInitializer>();
            var localConstantDeclaration = Mock.Of<ILocalVariableDeclaration>(t => t.Initial == initializer);
            var eater = Mock.Of<IEater>();
            var initializerEater = Mock.Of<IVariableInitializerEater>(t => t.Eat(snapshot.Object, initializer) == initKind);
            var localConstantDeclarationEater = new LocalVariableDeclarationEater(eater, initializerEater);

            // Act
            localConstantDeclarationEater.Eat(snapshot.Object, localConstantDeclaration);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(variableKindMustBe, localConstantDeclaration), Times.Once);
        }


    }
}