using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    class InvocationStuffEaterTests
    {
        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var snapshot = Mock.Of<ISnapshot>();
            var parentEater = Mock.Of<IParentReferenceEater>();
            var argsEater = new Mock<IArgumentsEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var invocationStuffEater = new InvocationStuffEater(parentEater, argsEater.Object, metricHelper);
            
            // Act
            invocationStuffEater.Eat(snapshot, invocationExpression);

            // Assert
            argsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }

        [Test]
        public void EatInvocationWithoutParentReferenceTest()
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>(t => t.ExtensionQualifier == Mock.Of<ICSharpArgumentInfo>());
            var snapshot = Mock.Of<ISnapshot>();
            var parentEater = Mock.Of<IParentReferenceEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var metricHelper = Mock.Of<IMetricHelper>(t => t.GetReturnVarType(invocationExpression, snapshot) == Variable.None);
            var invocationStuffEater = new InvocationStuffEater(parentEater, argsEater, metricHelper);

            // Act
            var result = invocationStuffEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [TestCase(Variable.None, Result = Variable.None)]
        [TestCase(Variable.Library, Result = Variable.Library)]
        [TestCase(Variable.Stub, Result = Variable.None)]
        [TestCase(Variable.Mock, Result = Variable.None)]
        [TestCase(Variable.Target, Result = Variable.None)]
        [TestCase(Variable.Service, Result = Variable.None)]
        public Variable AddCallBasedOnParentMetricsTest(Variable parentVarType)
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>(t => t.ExtensionQualifier == Mock.Of<ICSharpArgumentInfo>());
            var snapshot = Mock.Of<ISnapshot>();
            var parentEater = Mock.Of<IParentReferenceEater>(t => t.Eat(snapshot, invocationExpression) == parentVarType);
            var argsEater = Mock.Of<IArgumentsEater>();
            var metricHelper = Mock.Of<IMetricHelper>(t => t.GetReturnVarType(invocationExpression, snapshot) == Variable.None);
            var invocationStuffEater = new InvocationStuffEater(parentEater, argsEater, metricHelper);

            // Act
            return invocationStuffEater.Eat(snapshot, invocationExpression);
        }
    }
}