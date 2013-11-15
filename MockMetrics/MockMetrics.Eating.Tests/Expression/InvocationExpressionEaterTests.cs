using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class InvocationExpressionEaterTests
    {
        [Test]
        public void EatParentReferenceTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var cSharpArgument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new[] { cSharpArgument }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var parentEater = new Mock<IParentReferenceEater>();
            parentEater.Setup(t => t.Eat(snapshot, invocationExpression)).Returns(ExpressionKind.None).Verifiable();
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.Mock.Of()");
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, kindHelper, parentEater.Object);

            // Act
            invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            parentEater.VerifyAll();
        }

        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var cSharpArgument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new[] { cSharpArgument }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression) == ExpressionKind.None);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.Mock.Of()");
            var invocationEater = new InvocationExpressionEater(eater.Object, expressionHelper, kindHelper, parentEater);

            // Act
            invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression));
        }

        [Test]
        public void AddArgumentToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var cSharpArgument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new[] { cSharpArgument }));
            var snapshot = new Mock<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression) == ExpressionKind.None);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == ExpressionKind.Stub);
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.Mock.Of()");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater);

            // Act
            invocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Stub, cSharpArgument), Times.Once);
        }

        [Test]
        public void AddStubCandidateArgumentToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var cSharpArgument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new[] { cSharpArgument }));
            var snapshot = new Mock<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression) == ExpressionKind.None);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == ExpressionKind.StubCandidate);
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.Mock.Of()");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater);

            // Act
            invocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(It.IsAny<ExpressionKind>(), It.IsAny<ICSharpArgument>()), Times.Never);
        }

        [TestCase("Method:Moq.Mock.Of()", Result = ExpressionKind.Stub)]
        [TestCase("Method:NUnit.Framework.Assert", Result = ExpressionKind.Assert)]
        [TestCase("Method:Moq.Mock.Verify", Result = ExpressionKind.Assert)]
        public ExpressionKind EatSpecialInvocationTest(string invokedElementName)
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression) == ExpressionKind.None);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == invokedElementName);
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater);

            // Assert
            return invocationEater.Eat(snapshot, invocationExpression);

        }

        [Test]
        public void EatTargetCallTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == true);
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression) == ExpressionKind.None);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.TargetCall);
        }

        [Test]
        public void AddTargetCallToSnapshotTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = new Mock<ISnapshot>();
            snapshot.Setup(t => t.IsInTestScope("ModuleName")).Returns(true);
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression) == ExpressionKind.None);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater);

            // Act
            invocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.TargetCall, invocationExpression), Times.Once);
        }

        [Test]
        public void EatStubBecauseInvokedElementIsNotTestMethodTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression) == ExpressionKind.None);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>();
            var invoked = Mock.Of<IDeclaredElement>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invoked
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatStubBecauseInvokedElementIsNotInTestScopeTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false);
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression) == ExpressionKind.None);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void ReturnKindOfInvocationBasedOnPArentReferenceKindTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression) == ExpressionKind.StubCandidate);
            var kindHelper = Mock.Of<ExpressionKindHelper>(t => t.InvocationKindByParentReferenceKind(ExpressionKind.StubCandidate) == ExpressionKind.Stub);
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
        }

        [Test]
        public void AddExpressionToSnapshotIfBasedOnPArentReferenceKindisTargetCallTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = new Mock<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression) == ExpressionKind.StubCandidate);
            var kindHelper = Mock.Of<ExpressionKindHelper>(t => t.InvocationKindByParentReferenceKind(ExpressionKind.StubCandidate) == ExpressionKind.TargetCall);
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater);

            // Act
            var kind = invocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.TargetCall, invocationExpression), Times.Once);
            Assert.AreEqual(kind, ExpressionKind.TargetCall);
        }
    }
}
