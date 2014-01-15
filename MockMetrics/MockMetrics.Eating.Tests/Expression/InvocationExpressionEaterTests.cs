using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqFake;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class InvocationExpressionEaterTests
    {
        [Test]
        public void EatMoqInvocationTest()
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
            var mockEater = Mock.Of<IMoqInvocationEater>(t => t.Eat(snapshot, invocationExpression) == Variable.None);
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq");
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, parentEater, argsEater, mockEater);

            // Act
            var result = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
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
            var mockEater = Mock.Of<IMoqInvocationEater>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == string.Empty);
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, parentEater, argsEater.Object, mockEater);

            // Act
            invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            argsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }

        [Test]
        public void EatInvocationWithoutParentReferenceTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>(t => t.ExtensionQualifier == Mock.Of<ICSharpArgumentInfo>());
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var parentEater = Mock.Of<IParentReferenceEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMoqInvocationEater>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == string.Empty);
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, parentEater, argsEater, mockEater);

            // Act
            var result = invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(result, Variable.Service);
        }
        
        
        [TestCase(Variable.None, Result = Variable.Service)]
        [TestCase(Variable.Library, Result = Variable.Library)]
        [TestCase(Variable.Stub, Result = Variable.Service)]
        [TestCase(Variable.Mock, Result = Variable.Service)]
        [TestCase(Variable.Target, Result = Variable.Service)]
        [TestCase(Variable.Service, Result = Variable.Service)]
        public Variable AddCallBasedOnParentMetricsTest(Variable parentVarType)
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>(t => t.ExtensionQualifier == Mock.Of<ICSharpArgumentInfo>());
            var snapshot = new Mock<ISnapshot>();
            var parentEater = Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression) == parentVarType);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMoqInvocationEater>();
            var eater = Mock.Of<IEater>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == string.Empty);
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, parentEater, argsEater, mockEater);

            // Act
            return invocationEater.Eat(snapshot.Object, invocationExpression);
        }
    }
}
