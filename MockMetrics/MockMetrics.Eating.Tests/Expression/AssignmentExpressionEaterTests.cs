using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Tests.SutbTypes;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AssignmentExpressionEaterTests
    {
        [Test]
        public void EatEventAssingmentTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest);
            var declElement = Mock.Of<IEvent>();
            var eater = Mock.Of<IEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == declElement);
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Act
            var kind = assignmentExpressionEater.Eat(snapshot, assignmentExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.None);
        }

        [Test]
        public void EatVariableAssingmentTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest);
            var declElement = Mock.Of<IVariableDeclarationAndIDeclaredElement>();
            var eater = Mock.Of<IEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == declElement);
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Act
            var kind = assignmentExpressionEater.Eat(snapshot, assignmentExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.None);
        }

        [Test]
        public void EatAssignmnetTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var dest = Mock.Of<IReferenceExpression>();
            var source = Mock.Of<ICSharpExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var declElement = Mock.Of<ILocalVariableAndIDeclaredElement>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, source) == ExpressionKind.Stub);
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == declElement);
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>(t => t.KindOfAssignment(ExpressionKind.Stub) == ExpressionKind.Stub);
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Act
            var kind = assignmentExpressionEater.Eat(snapshot, assignmentExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
        }

        [Test]
        public void AddAssignmentDestToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var dest = Mock.Of<IReferenceExpression>();
            var source = Mock.Of<ICSharpExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var declElement = Mock.Of<ILocalVariableAndIDeclaredElement>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, source) == ExpressionKind.Stub);
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == declElement);
            var expressionKindHelper = Mock.Of<ExpressionKindHelper>(t => t.KindOfAssignment(ExpressionKind.Stub) == ExpressionKind.Stub);
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, expressionKindHelper);

            // Act
            var kind = assignmentExpressionEater.Eat(snapshot.Object, assignmentExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
            snapshot.Verify(t => t.AddTreeNode(kind, dest));
        }
    }
}
