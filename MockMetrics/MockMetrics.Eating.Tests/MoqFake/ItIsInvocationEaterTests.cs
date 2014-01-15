using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqFake;
using MockMetrics.Eating.Tests.StubTypes;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.MoqFake
{
    [TestFixture]
    public class ItIsInvocationEaterTests
    {
        [TestCase("Method:Moq.It.IsAny")]
        [TestCase("Method:Moq.It.IsIn")]
        [TestCase("Method:Moq.It.IsInRange")]
        [TestCase("Method:Moq.It.IsNotNull")]
        [TestCase("Method:Moq.It.IsNotIn")]
        [TestCase("Method:Moq.It.IsRegex")]
        [TestCase("WOWOWOWOWO", ExpectedException = typeof(UnexpectedMoqItIsMethodNameException))]
        public void EatItIsReturnStubAnyWayTest(string methodName)
        {
            // Arrange
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var eater = Mock.Of<IEater>();
            var argumentsEater = Mock.Of<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == methodName);
            var snapshot = Mock.Of<ISnapshot>();
            var itIsInvocationEater = new ItIsInvocationEater(eater, eatExpressionHelper, argumentsEater);

            // Act
            var result = itIsInvocationEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(result, Variable.Stub);
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
            var argumentsEater = new Mock<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.It.IsAny");
            var itIsInvocationEater = new ItIsInvocationEater(eater, eatExpressionHelper, argumentsEater.Object);

            // Act
            itIsInvocationEater.Eat(snapshot, invocationExpression);

            // Assert
            argumentsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }

        [Test]
        public void EatItIsMethodOptionTest()
        {
            // Arrange
            var methodOptionParent = Mock.Of<IReferenceExpression>();
            var methodOption = Mock.Of<IInvocationExpression>();
            var param = Mock.Of<ILambdaParameterDeclarationAndIDeclaredElement>();
            var parameters = new TreeNodeCollection<ILambdaParameterDeclaration>(new[] { param });
            var options = Mock.Of<ILambdaExpression>();
            Mock.Get(options).Setup(t => t.ParameterDeclarations)
                .Returns(parameters);
            var arg = Mock.Of<ICSharpArgument>(t => t.Expression == options);
            var args = new TreeNodeCollection<ICSharpArgument>(new[] { arg });
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var eater = Mock.Of<IEater>(t => t.EatedNodes == new List<ICSharpTreeNode> { methodOption, Mock.Of<IInvocationExpression>() });
            var argumentsEater = Mock.Of<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.It.Is"
                                                                     && t.GetParentReference(methodOption) == methodOptionParent
                                                                     && t.GetReferenceElement(methodOptionParent) == param);

            var snapshot = new Mock<ISnapshot>();
            var itIsInvocationEater = new ItIsInvocationEater(eater, eatExpressionHelper, argumentsEater);

            // Act
            itIsInvocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddFakeOption(methodOption, FakeOption.Method));
        }

        [Test]
        public void EatItIsPropertyOptionTest()
        {
            // Arrange
            var propertyOptionParent = Mock.Of<IReferenceExpression>();
            var propertyOption = Mock.Of<IReferenceExpression>();
            var param = Mock.Of<ILambdaParameterDeclarationAndIDeclaredElement>();
            var parameters = new TreeNodeCollection<ILambdaParameterDeclaration>(new[] { param });
            var options = Mock.Of<ILambdaExpression>();
            Mock.Get(options).Setup(t => t.ParameterDeclarations)
                .Returns(parameters);
            var arg = Mock.Of<ICSharpArgument>(t => t.Expression == options);
            var args = new TreeNodeCollection<ICSharpArgument>(new[] { arg });
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var eater = Mock.Of<IEater>(t => t.EatedNodes == new List<ICSharpTreeNode> { propertyOption, Mock.Of<IInvocationExpression>() });
            var argumentsEater = Mock.Of<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.It.Is"
                                                                     && t.GetParentReference(propertyOption) == propertyOptionParent
                                                                     && t.GetReferenceElement(propertyOptionParent) == param);
            var snapshot = new Mock<ISnapshot>();
            var itIsInvocationEater = new ItIsInvocationEater(eater, eatExpressionHelper, argumentsEater);

            // Act
            itIsInvocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddFakeOption(propertyOption, FakeOption.Property));
        }

        [Test]
        public void NotSingleItIsInvacotaionArgumentTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>(new[]
            {
                Mock.Of<ICSharpArgument>(), Mock.Of<ICSharpArgument>()
            });
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argumentsEater = new Mock<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.It.Is");
            var itIsInvocationEater = new ItIsInvocationEater(eater, eatExpressionHelper, argumentsEater.Object);

            // Assert
            Assert.Throws<NotSingleItIsInvacotaionArgumentException>(() => itIsInvocationEater.Eat(snapshot, invocationExpression));
        }

        [Test]
        public void NotSingleInIsOptionLambdaParameterTest()
        {
            // Arrange
            var methodOption = Mock.Of<IInvocationExpression>();
            var param = Mock.Of<ILambdaParameterDeclarationAndIDeclaredElement>();
            var parameters = new TreeNodeCollection<ILambdaParameterDeclaration>(new[] { param, param });
            var options = Mock.Of<ILambdaExpression>();
            Mock.Get(options).Setup(t => t.ParameterDeclarations)
                .Returns(parameters);
            var arg = Mock.Of<ICSharpArgument>(t => t.Expression == options);
            var args = new TreeNodeCollection<ICSharpArgument>(new[] { arg });
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var eater = Mock.Of<IEater>(t => t.EatedNodes == new List<ICSharpTreeNode> { methodOption, Mock.Of<IInvocationExpression>() });
            var argumentsEater = Mock.Of<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvokedElementName(invocationExpression) == "Method:Moq.It.Is");

            var snapshot = Mock.Of<ISnapshot>();
            var itIsInvocationEater = new ItIsInvocationEater(eater, eatExpressionHelper, argumentsEater);

            // Assert
            Assert.Throws<NotSingleInIsOptionLambdaParameterException>(() => itIsInvocationEater.Eat(snapshot, invocationExpression));
        }
    }
}
