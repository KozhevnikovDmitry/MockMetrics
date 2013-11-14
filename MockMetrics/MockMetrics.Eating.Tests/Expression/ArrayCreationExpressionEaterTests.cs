using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.VariableDeclaration;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ArrayCreationExpressionEaterTests
    {
        [Test]
        public void EatSizesTest()
        {
            // Arrange
            var size = Mock.Of<ICSharpExpression>();
            var arrayCreationExpression = Mock.Of<IArrayCreationExpression>();
            Mock.Get(arrayCreationExpression).Setup(t => t.Sizes)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new[] { size }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var arrayCreationExpressionEater = new ArrayCreationExpressionEater(eater.Object, Mock.Of<IVariableInitializerEater>());

            // Act
            arrayCreationExpressionEater.Eat(snapshot, arrayCreationExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, size));
        }

        [Test]
        public void AddSizesToSnapshotTest()
        {
            // Arrange
            var size = Mock.Of<ICSharpExpression>();
            var arrayCreationExpression = Mock.Of<IArrayCreationExpression>();
            Mock.Get(arrayCreationExpression).Setup(t => t.Sizes)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new[] { size }));
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, size) == ExpressionKind.Stub);
            var arrayCreationExpressionEater = new ArrayCreationExpressionEater(eater, Mock.Of<IVariableInitializerEater>());

            // Act
            arrayCreationExpressionEater.Eat(snapshot.Object, arrayCreationExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Stub, size));
        }

        [Test]
        public void EatArrayInitializerTest()
        {
            // Arrange
            var initializer = Mock.Of<IArrayInitializer>();
            var arrayCreationExpression = Mock.Of<IArrayCreationExpression>(t => t.ArrayInitializer == initializer);
            Mock.Get(arrayCreationExpression).Setup(t => t.Sizes)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var initilizerEater = new Mock<IVariableInitializerEater>();
            var arrayCreationExpressionEater = new ArrayCreationExpressionEater(eater, initilizerEater.Object);

            // Act
            arrayCreationExpressionEater.Eat(snapshot, arrayCreationExpression);

            // Assert
            initilizerEater.Verify(t => t.Eat(snapshot, initializer));
        }

        [Test]
        public void ReturnStubCandidateTest()
        {
            var arrayCreationExpression = Mock.Of<IArrayCreationExpression>();
            Mock.Get(arrayCreationExpression).Setup(t => t.Sizes)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var initilizerEater = Mock.Of<IVariableInitializerEater>();
            var arrayCreationExpressionEater = new ArrayCreationExpressionEater(eater, initilizerEater);

            // Act
            var kind = arrayCreationExpressionEater.Eat(snapshot, arrayCreationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);

        }
    }
}