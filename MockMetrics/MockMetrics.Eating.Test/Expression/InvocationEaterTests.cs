using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Test.Expression
{
    [TestFixture]
    public class InvocationEaterTests
    {
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
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.Mock.Of()");
            var invocationEater = new InvocationEater(eater.Object, helper);

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
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == ExpressionKind.Stub);
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.Mock.Of()");
            var invocationEater = new InvocationEater(eater, helper);

            // Act
            invocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Stub, cSharpArgument));
        }

        [TestCase("Method:Moq.Mock.Of()", Result = ExpressionKind.Stub)]
        [TestCase("Method:NUnit.Framework.Assert", Result = ExpressionKind.Assert)]
        [TestCase("Method:Moq.Mock.Verify", Result = ExpressionKind.Assert)]
        public ExpressionKind EatMoqStubTest(string invokedElementName)
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == invokedElementName);
            var invocationEater = new InvocationEater(eater, helper);

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
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationEater(eater, helper);

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
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationEater(eater, helper);

            // Act
            invocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.TargetCall, invocationExpression), Times.Once);
        }

        [Test]
        public void EatStubBecauseInvokedElementIsNotMethodTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var invoked = Mock.Of<IDeclaredElement>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invoked
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationEater(eater, helper);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
        }

        [Test]
        public void EatStubBecauseInvokedElementIsNotInTestScopeTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false);
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationEater(eater, helper);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
        }
    }
}
