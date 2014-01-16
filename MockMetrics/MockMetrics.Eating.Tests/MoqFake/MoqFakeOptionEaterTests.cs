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
    class MoqFakeOptionEaterTests
    {
        [Test]
        public void EatMethodOptionTest()
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
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetParentReference(methodOption) == methodOptionParent
                                                                        && t.GetReferenceElement(methodOptionParent) == param);

            var snapshot = new Mock<ISnapshot>();
            var itIsInvocationEater = new MoqFakeOptionEater(eater, eatExpressionHelper);

            // Act
            itIsInvocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddFakeOption(methodOption, FakeOption.Method));
        }

        [Test]
        public void EatPropertyOptionTest()
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
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetParentReference(propertyOption) == propertyOptionParent
                                                                        && t.GetReferenceElement(propertyOptionParent) == param);
            var snapshot = new Mock<ISnapshot>();
            var itIsInvocationEater = new MoqFakeOptionEater(eater, eatExpressionHelper);

            // Act
            itIsInvocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddFakeOption(propertyOption, FakeOption.Property));
        }

        [Test]
        public void EatWithoutArgumentsTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]);
            var invocationExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationExpression).Setup(t => t.Arguments)
                .Returns(args); 
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>();
            var itIsInvocationEater = new MoqFakeOptionEater(eater, eatExpressionHelper);

            // Act
            itIsInvocationEater.Eat(snapshot.Object, invocationExpression);

            // Assert
            snapshot.Verify(t => t.AddFakeOption(It.IsAny<ICSharpExpression>(), It.IsAny<FakeOption>()), Times.Never());
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
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>();

            var snapshot = Mock.Of<ISnapshot>();
            var itIsInvocationEater = new MoqFakeOptionEater(eater, eatExpressionHelper);

            // Assert
            Assert.Throws<NotSingleMoqFakeOptionLambdaParameterException>(() => itIsInvocationEater.Eat(snapshot, invocationExpression));
        }
    }
}