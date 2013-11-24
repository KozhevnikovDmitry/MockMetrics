using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Tests.StubTypes;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AssignmentExpressionEaterTests
    {
        [Test]
        public void EatAssignmentSourceExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = new Mock<IEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == Mock.Of<IVariableDeclarationAndIDeclaredElement>());
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater.Object, eatExpressionHelper, expressionKindHelper);

            // Act
            assignmentExpressionEater.Eat(snapshot, assignmentExpression, false);

            // Assert
            eater.Verify(t => t.Eat(snapshot, source, false));
        }

        [Test]
        public void ReturnNoneForEventAssigmentDestTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = Mock.Of<IEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == Mock.Of<IEventDeclarationAndIDeclaredElement>());
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Act
            var kind = assignmentExpressionEater.Eat(snapshot, assignmentExpression, false);

            // Assert
            Assert.That(kind, Is.EqualTo(ExpressionKind.None));
        }

        [Test]
        public void EatVariableAssignmentTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, source, false) == ExpressionKind.StubCandidate);
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == Mock.Of<IVariableDeclarationAndIDeclaredElement>());
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>(t => t.KindOfAssignment(ExpressionKind.StubCandidate) == ExpressionKind.Stub);
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Act
            var kind = assignmentExpressionEater.Eat(snapshot, assignmentExpression, false);

            // Assert
            Assert.That(kind, Is.EqualTo(ExpressionKind.Stub));
        }

        [Test]
        public void AddAssigningVariableToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, source, false) == ExpressionKind.StubCandidate);
            var variableDeclaration = Mock.Of<IVariableDeclarationAndIDeclaredElement>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == variableDeclaration);
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>(t => t.KindOfAssignment(ExpressionKind.StubCandidate) == ExpressionKind.Stub);
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Act
            assignmentExpressionEater.Eat(snapshot.Object, assignmentExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.Stub, variableDeclaration), Times.Once);
        }



        [Test]
        public void ExceptAssigningLocalVariableFromSnapshotBeforeEatTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, source, false) == ExpressionKind.StubCandidate);
            var variableDeclaration = Mock.Of<ILocalVariableAndIDeclaredElement>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == variableDeclaration);
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>(t => t.KindOfAssignment(ExpressionKind.StubCandidate) == ExpressionKind.Stub);
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Act
            assignmentExpressionEater.Eat(snapshot.Object, assignmentExpression, false);

            // Assert
            snapshot.Verify(t => t.Except(variableDeclaration), Times.Once);
            snapshot.Verify(t => t.Add(ExpressionKind.Stub, variableDeclaration), Times.Once);
        }

        [Test]
        public void ExceptAssigningEatUnsafeCodeFixedPointeFromSnapshotBeforeEatTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, source, false) == ExpressionKind.StubCandidate);
            var variableDeclaration = Mock.Of<IUnsafeCodeFixedPointerAndIDeclaredElement>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == variableDeclaration);
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>(t => t.KindOfAssignment(ExpressionKind.StubCandidate) == ExpressionKind.Stub);
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Act
            assignmentExpressionEater.Eat(snapshot.Object, assignmentExpression, false);

            // Assert
            snapshot.Verify(t => t.Except(variableDeclaration), Times.Once);
            snapshot.Verify(t => t.Add(ExpressionKind.Stub, variableDeclaration), Times.Once);
        }

        [Test]
        public void UnexpectedAssignDestinationTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var dest = Mock.Of<ICSharpExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest);
            var eater = Mock.Of<IEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>();
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Assert
            Assert.Throws<UnexpectedAssignDestinationException>(() => assignmentExpressionEater.Eat(snapshot, assignmentExpression, false));
        }
    }
}
