using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ElementAccessExpressionEaterTests
    {
        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var cSharpArgument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var elementAccessExpression = Mock.Of<IElementAccessExpression>();
            Mock.Get(elementAccessExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new[] { cSharpArgument }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var elementAccessExpressionEater = new ElementAccessExpressionEater(eater.Object);

            // Act
            elementAccessExpressionEater.Eat(snapshot, elementAccessExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression));
        }

        [Test]
        public void AddArgumentToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var cSharpArgument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var elementAccessExpression = Mock.Of<IElementAccessExpression>();
            Mock.Get(elementAccessExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new[] { cSharpArgument }));
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == ExpressionKind.Stub);
            var elementAccessExpressionEater = new ElementAccessExpressionEater(eater);

            // Act
            elementAccessExpressionEater.Eat(snapshot.Object, elementAccessExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Stub, cSharpArgument), Times.Once);
        }

        [Test]
        public void ReturnStubCandidateTest()
        {
            // Arrange
            var elementAccessExpression = Mock.Of<IElementAccessExpression>();
            Mock.Get(elementAccessExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var elementAccessExpressionEater = new ElementAccessExpressionEater(eater);

            // Act
            var kind = elementAccessExpressionEater.Eat(snapshot, elementAccessExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }
    }
}