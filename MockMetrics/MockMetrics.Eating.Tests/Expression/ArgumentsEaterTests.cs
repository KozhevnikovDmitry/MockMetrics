using System;
using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ArgumentsEaterTests
    {
        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var mockRep = new MockRepository(MockBehavior.Default);
            var args = new  TreeNodeCollection<ICSharpArgument>(mockRep.Of<ICSharpArgument>().Where(t => t.Value == expression).Take(10).ToArray());
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var argumentsEater = new ArgumentsEater(eater.Object);

            // Act
            argumentsEater.Eat(snapshot, args);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression, true), Times.Exactly(10));
        }

        [Test]
        public void NotAddToSnapshotReferenseArgumentsTest()
        {
            // Arrange
            var expression = Mock.Of<IReferenceExpression>();
            var argument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var args = new TreeNodeCollection<ICSharpArgument>(new[] { argument });
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argumentsEater = new ArgumentsEater(eater);

            // Act
            argumentsEater.Eat(snapshot.Object, args);

            // Assert
            snapshot.Verify(t => t.Add(It.IsAny<ExpressionKind>(), argument), Times.Never);
        }

        [Test]
        public void NotAddToSnapshotStubCandidateArgumentsTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var argument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var args = new TreeNodeCollection<ICSharpArgument>(new[] { argument });
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression, false) == ExpressionKind.StubCandidate);
            var argumentsEater = new ArgumentsEater(eater);

            // Act
            argumentsEater.Eat(snapshot.Object, args);

            // Assert
            snapshot.Verify(t => t.Add(It.IsAny<ExpressionKind>(), argument), Times.Never);
        }

        [Test]
        public void AddArgumentToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var argument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var args = new TreeNodeCollection<ICSharpArgument>(new[] { argument });
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression, true) == ExpressionKind.Stub);
            var argumentsEater = new ArgumentsEater(eater);

            // Act
            argumentsEater.Eat(snapshot.Object, args);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.Stub, argument), Times.Once);
        }
        
        [Test]
        public void NullSnapshotTest()
        {
            // Arrange
            var argumentsEater = new ArgumentsEater(Mock.Of<IEater>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => argumentsEater.Eat(null, new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0])));
        }
    }
}
