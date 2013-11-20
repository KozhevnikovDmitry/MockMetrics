using System.Windows.Input;
using JetBrains.ReSharper.Psi;
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
        public void EatVariableWithoutInitializerTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var type = Mock.Of<IType>();
            var localConstantDeclaration = Mock.Of<ILocalVariableDeclaration>(t => t.Type == type);
            var typeEater = new Mock<ITypeEater>();
            var eater = Mock.Of<IEater>();
            var initializerEater = Mock.Of<IVariableInitializerEater>();
            var localConstantDeclarationEater = new LocalVariableDeclarationEater(eater, initializerEater, typeEater.Object);

            // Act
            localConstantDeclarationEater.Eat(snapshot, localConstantDeclaration);

            // Assert
            typeEater.Verify(t => t.EatVariableType(snapshot, type), Times.Once);
        }

        [Test]
        public void AddVariableWithoutInitializerToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var type = Mock.Of<IType>();
            var localConstantDeclaration = Mock.Of<ILocalVariableDeclaration>(t => t.Type == type);
            var typeEater = Mock.Of<ITypeEater>(t => t.EatVariableType(snapshot.Object, type) == ExpressionKind.Stub);
            var eater = Mock.Of<IEater>();
            var initializerEater = Mock.Of<IVariableInitializerEater>();
            var localConstantDeclarationEater = new LocalVariableDeclarationEater(eater, initializerEater, typeEater);

            // Act
            localConstantDeclarationEater.Eat(snapshot.Object, localConstantDeclaration);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.Stub, localConstantDeclaration), Times.Once);
        }

        [Test]
        public void EatVariableInitializerTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var initializer = Mock.Of<IVariableInitializer>();
            var localConstantDeclaration = Mock.Of<ILocalVariableDeclaration>(t => t.Initial == initializer);
            var typeEater = Mock.Of<ITypeEater>();
            var eater = Mock.Of<IEater>();
            var initializerEater = new Mock<IVariableInitializerEater>();
            var localConstantDeclarationEater = new LocalVariableDeclarationEater(eater, initializerEater.Object, typeEater);

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
            var typeEater = Mock.Of<ITypeEater>();
            var eater = Mock.Of<IEater>();
            var initializerEater = Mock.Of<IVariableInitializerEater>(t => t.Eat(snapshot.Object, initializer) == initKind);
            var localConstantDeclarationEater = new LocalVariableDeclarationEater(eater, initializerEater, typeEater);

            // Act
            localConstantDeclarationEater.Eat(snapshot.Object, localConstantDeclaration);

            // Assert
            snapshot.Verify(t => t.Add(variableKindMustBe, localConstantDeclaration), Times.Once);
        }


    }
}