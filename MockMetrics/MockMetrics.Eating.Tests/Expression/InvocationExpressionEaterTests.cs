using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqStub;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class InvocationExpressionEaterTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void EatMockOfInvocation_TranslateInnerEatTest(bool innerEat)
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var parentEater = Mock.Of<IParentReferenceEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = new Mock<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.Mock.Of");
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, kindHelper, parentEater, argsEater, mockEater.Object);

            // Act
            invocationEater.Eat(snapshot, invocationExpression, innerEat);

            // Assert
            mockEater.Verify(t => t.Eat(snapshot, invocationExpression, innerEat), Times.Once);
        }

        [Test]
        public void EatParentReferenceTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var parentEater = new Mock<IParentReferenceEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            parentEater.Setup(t => t.Eat(snapshot, invocationExpression, false)).Returns(ExpressionKind.None).Verifiable();
            var kindHelper = Mock.Of<VarTypeHelper>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:NUnit.Framework.Assert");
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, kindHelper, parentEater.Object, argsEater, mockEater);

            // Act
            invocationEater.Eat(snapshot, invocationExpression, false);

            // Assert
            parentEater.VerifyAll();
        }

        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var parentEater = Mock.Of<IParentReferenceEater>();
            var argsEater = new Mock<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:NUnit.Framework.Assert");
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, kindHelper, parentEater, argsEater.Object, mockEater);

            // Act
            invocationEater.Eat(snapshot, invocationExpression, false);

            // Assert
            argsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }
        
        [TestCase("Method:NUnit.Framework.Assert", Result = ExpressionKind.Assert)]
        [TestCase("Method:Moq.Mock.Verify", Result = ExpressionKind.Assert)]
        public ExpressionKind EatAssertationTest(string invokedElementName)
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression, false) == ExpressionKind.None);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>();
            var eater = Mock.Of<IEater>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == invokedElementName);
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater, argsEater, mockEater);

            // Assert
            return invocationEater.Eat(snapshot, invocationExpression, false);

        }

        [TestCase("Method:NUnit.Framework.Assert")]
        [TestCase("Method:Moq.Mock.Verify")]
        public void AddAssertationToSnapshotTest(string invokedElementName)
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = new Mock<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression, false) == ExpressionKind.None);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>();
            var eater = Mock.Of<IEater>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == invokedElementName);
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater, argsEater, mockEater);

            // Act
            invocationEater.Eat(snapshot.Object, invocationExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.Assert, invocationExpression));
        }

        [Test]
        public void EatTargetCallTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == true);
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression, false) == ExpressionKind.None);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>();
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater, argsEater, mockEater);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.TargetCall);
        }

        [Test]
        public void AddTargetCallToSnapshotTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = new Mock<ISnapshot>();
            snapshot.Setup(t => t.IsInTestScope("ModuleName")).Returns(true);
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression, false) == ExpressionKind.None);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>();
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater, argsEater, mockEater);

            // Act
            invocationEater.Eat(snapshot.Object, invocationExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.TargetCall, invocationExpression), Times.Once);
        }

        [Test]
        public void EatStubBecauseInvokedElementIsNotTestMethodTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression, false) == ExpressionKind.None);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>();
            var eater = Mock.Of<IEater>();
            var invoked = Mock.Of<IDeclaredElement>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invoked
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater, argsEater, mockEater);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatStubBecauseInvokedElementIsNotInTestScopeTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false);
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression, false) == ExpressionKind.None);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>();
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater, argsEater, mockEater);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void ReturnKindOfInvocationBasedOnPArentReferenceKindTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression, false) == ExpressionKind.StubCandidate);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>(t => t.InvocationKindByParentReferenceKind(ExpressionKind.StubCandidate) == ExpressionKind.Stub);
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater, argsEater, mockEater);

            // Act
            var kind = invocationEater.Eat(snapshot, invocationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
        }

        [Test]
        public void AddExpressionToSnapshotIfBasedOnPArentReferenceKindisTargetCallTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = new Mock<ISnapshot>();
            var parentEater =
                Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression, false) == ExpressionKind.StubCandidate);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var kindHelper = Mock.Of<VarTypeHelper>(t => t.InvocationKindByParentReferenceKind(ExpressionKind.StubCandidate) == ExpressionKind.TargetCall);
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IMethod>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, kindHelper, parentEater, argsEater, mockEater);

            // Act
            var kind = invocationEater.Eat(snapshot.Object, invocationExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.TargetCall, invocationExpression), Times.Once);
            Assert.AreEqual(kind, ExpressionKind.TargetCall);
        }
    }
}
