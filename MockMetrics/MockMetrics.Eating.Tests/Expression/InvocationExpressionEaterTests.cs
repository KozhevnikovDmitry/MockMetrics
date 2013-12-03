using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqStub;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class InvocationExpressionEaterTests
    {
        [Test]
        public void EatMockOfInvocationTest()
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
            var metricHelper = Mock.Of<IMetricHelper>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.Mock.Of");
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, metricHelper, parentEater, argsEater, mockEater.Object);

            // Act
            invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            mockEater.Verify(t => t.Eat(snapshot, invocationExpression), Times.Once);
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
            var metricHelper = Mock.Of<IMetricHelper>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:NUnit.Framework.Assert");
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, metricHelper, parentEater, argsEater.Object, mockEater);

            // Act
            invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            argsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
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
            var metricHelper = Mock.Of<IMetricHelper>();
            var expressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:NUnit.Framework.Assert");
            var invocationEater = new InvocationExpressionEater(eater, expressionHelper, metricHelper, parentEater.Object, argsEater, mockEater);

            // Act
            invocationEater.Eat(snapshot, invocationExpression);

            // Assert
            parentEater.Verify(t => t.Eat(snapshot, invocationExpression), Times.Once());
        }

        [Test]
        public void EatAssertTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = new Mock<ISnapshot>();
            var parentEater = Mock.Of<IParentReferenceEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:NUnit.Framework.Assert");
            var invocationEater = new InvocationExpressionEater(eater, helper, metricHelper, parentEater, argsEater, mockEater);

            // Act
            var metrics = invocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddCall(invocationExpression, It.Is<Metrics>(m => m.Call == Call.Assert && m.Scope == Scope.Local && m.Equals(metrics))));
        }

        [Test]
        public void EatMoqVerifyTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = new Mock<ISnapshot>();
            var parentMetrics = Metrics.Create();
            parentMetrics.Scope = Scope.Internal;
            var parentEater = Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression) == parentMetrics);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.Mock.Verify");
            var invocationEater = new InvocationExpressionEater(eater, helper, metricHelper, parentEater, argsEater, mockEater);

            // Act
            var metrics = invocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddCall(invocationExpression, It.Is<Metrics>(m => m.Call == Call.Assert && m.Scope == Scope.Internal && m.Equals(metrics))));
        }
        
        [Test]
        public void AddCallBasedOnParentMetricsTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = new Mock<ISnapshot>();
            var parentMetrics = Metrics.Create();
            var childMetrics = Metrics.Create();
            var parentEater = Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression) == parentMetrics);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var invokedMethod = Mock.Of<IMethod>();
            var metricHelper = Mock.Of<IMetricHelper>(t => t.CallMetrics(snapshot.Object, invokedMethod, parentMetrics) == childMetrics);
            var eater = Mock.Of<IEater>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, metricHelper, parentEater, argsEater, mockEater);

            // Act
            var metrics = invocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddCall(invocationExpression, childMetrics), Times.Once);
            Assert.AreEqual(metrics, childMetrics);
        }

        [Test]
        public void UnexpectedInvokedElementTypeExceptionTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = new Mock<ISnapshot>();
            var parentMetrics = Metrics.Create();
            var parentEater = Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot.Object, invocationExpression) == parentMetrics);
            var argsEater = Mock.Of<IArgumentsEater>();
            var mockEater = Mock.Of<IMockOfInvocationEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var invokedMethod = Mock.Of<IDeclaredElement>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElement(invocationExpression) == invokedMethod
                                                        && t.GetInvokedElementName(invocationExpression) == "");
            var invocationEater = new InvocationExpressionEater(eater, helper, metricHelper, parentEater, argsEater, mockEater);

            // Assert
            Assert.Throws<UnexpectedInvokedElementTypeException>(() => invocationEater.Eat(snapshot.Object, invocationExpression));
        }
    }
}
