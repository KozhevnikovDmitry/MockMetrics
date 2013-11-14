using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.VariableDeclaration;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class VariableInitializerEaterTests
    {
        [Test]
        public void EatArrayInitializerTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IExpressionInitializer>(t => t.Value == expression);
            var arrayInit = Mock.Of<IArrayInitializer>();
            Mock.Get(arrayInit).Setup(t => t.ElementInitializers)
                .Returns(new TreeNodeCollection<IVariableInitializer>(new[] {itemInitializer}));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var initializerEater = new VariableInitializerEater(eater.Object);

            // Act
            var kind = initializerEater.Eat(snapshot, arrayInit);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
            eater.Verify(t => t.Eat(snapshot, expression));
        }

        [Test]
        public void AddItemToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IExpressionInitializer>(t => t.Value == expression);
            var arrayInit = Mock.Of<IArrayInitializer>();
            Mock.Get(arrayInit).Setup(t => t.ElementInitializers)
                .Returns(new TreeNodeCollection<IVariableInitializer>(new[] { itemInitializer }));
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == ExpressionKind.Stub);
            var initializerEater = new VariableInitializerEater(eater);

            // Act
            initializerEater.Eat(snapshot.Object, arrayInit);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Stub, itemInitializer));
        }

        [Test]
        public void EatExpressionInitializerTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IExpressionInitializer>(t => t.Value == expression);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression) == ExpressionKind.Mock);
            var initializerEater = new VariableInitializerEater(eater);

            // Act
            var kind = initializerEater.Eat(snapshot, itemInitializer);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatUnsafeCodeFixedPointerInitializerTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IUnsafeCodeFixedPointerInitializer>(t => t.Value == expression);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression) == ExpressionKind.Mock);
            var initializerEater = new VariableInitializerEater(eater);

            // Act
            var kind = initializerEater.Eat(snapshot, itemInitializer);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatUnsafeCodeStackAllocInitializerTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IUnsafeCodeStackAllocInitializer>(t => t.DimExpr == expression);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression) == ExpressionKind.Mock);
            var initializerEater = new VariableInitializerEater(eater);

            // Act
            var kind = initializerEater.Eat(snapshot, itemInitializer);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatTargetCallInitializerTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IExpressionInitializer>(t => t.Value == expression);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression) == ExpressionKind.TargetCall);
            var initializerEater = new VariableInitializerEater(eater);

            // Act
            var kind = initializerEater.Eat(snapshot, itemInitializer);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Result);
        }

        [Test]
        public void EatStubCandidateTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IExpressionInitializer>(t => t.Value == expression);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression) == ExpressionKind.StubCandidate);
            var initializerEater = new VariableInitializerEater(eater);

            // Act
            var kind = initializerEater.Eat(snapshot, itemInitializer);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
        }
    }
}