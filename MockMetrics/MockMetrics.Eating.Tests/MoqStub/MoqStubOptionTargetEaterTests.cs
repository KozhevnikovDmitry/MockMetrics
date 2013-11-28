using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqStub;
using MockMetrics.Eating.Tests.StubTypes;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.MoqStub
{
    [TestFixture]
    public class MoqStubOptionTargetEaterTests
    {
        [Test]
        public void EatInvocationOptionArgumentsTest()
        {
            // Arrange
            var argEater = new Mock<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentReference = Mock.Of<IReferenceExpression>();
            var lambdaParamDeclaration = Mock.Of<ILambdaParameterDeclarationAndIDeclaredElement>();
            var args = new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]);
            var invocationOption = Mock.Of<IInvocationExpression>();
            Mock.Get(invocationOption)
                .Setup(t => t.Arguments)
                .Returns(args);
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvocationReference(invocationOption) == parentReference
                                                           && t.GetReferenceElement(parentReference) == lambdaParamDeclaration);
            var stubTargetEater = new MoqStubOptionTargetEater(eatHelper, argEater.Object);

            // Act
            stubTargetEater.EatOption(snapshot, invocationOption);

            // Assert
            argEater.Verify(t => t.Eat(snapshot, args));
        }

        [Test]
        public void NoParentForInvocationTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var invocationOption = Mock.Of<IInvocationExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>();
            var stubTargetEater = new MoqStubOptionTargetEater(eatHelper, argEater);
            
            // Assert
            Assert.Throws<MoqStubWrongSyntaxException>(() => stubTargetEater.EatOption(snapshot, invocationOption));
        }

        [Test]
        public void RecoursiveEatInvocationOptionTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentParentReference = Mock.Of<IReferenceExpression>();
            var parentInvocation = Mock.Of<IInvocationExpression>();
            var lambdaParamDeclaration = Mock.Of<ILambdaParameterDeclarationAndIDeclaredElement>();
            var invocationOption = Mock.Of<IInvocationExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvocationReference(invocationOption) == parentInvocation
                                                           && t.GetInvocationReference(parentInvocation) == parentParentReference
                                                           && t.GetReferenceElement(parentParentReference) == lambdaParamDeclaration);
            var stubTargetEater = new Mock<MoqStubOptionTargetEater>(eatHelper, argEater) { CallBase = true };
            stubTargetEater.Setup(t => t.EatOption(snapshot, parentInvocation)).Verifiable();

            // Act
            stubTargetEater.Object.EatOption(snapshot, invocationOption);

            // Assert
            stubTargetEater.VerifyAll();
        }

        [Test]
        public void RecoursiveEatInvocationOptionPаrentReferenceTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentReference = Mock.Of<IReferenceExpression>();
            var variableDecalration = Mock.Of<IVariableDeclarationAndIDeclaredElement>();
            var invocationOption = Mock.Of<IInvocationExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvocationReference(invocationOption) == parentReference
                                                           && t.GetReferenceElement(parentReference) == variableDecalration);
            var stubTargetEater = new Mock<MoqStubOptionTargetEater>(eatHelper, argEater) { CallBase = true };
            stubTargetEater.Setup(t => t.EatOption(snapshot, parentReference));

            // Act
            stubTargetEater.Object.EatOption(snapshot, invocationOption);

            // Assert
            stubTargetEater.Verify(t => t.EatOption(snapshot, parentReference), Times.Once);
        }

        [Test]
        public void ReturnFakeTypeMethodForParentLambdaParameterDeclarationOfInvocationOptionTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentReference = Mock.Of<IReferenceExpression>();
            var lambdaParamDeclaration = Mock.Of<ILambdaParameterDeclarationAndIDeclaredElement>();
            var invocationOption = Mock.Of<IInvocationExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvocationReference(invocationOption) == parentReference
                                                           && t.GetReferenceElement(parentReference) == lambdaParamDeclaration);
            var stubTargetEater = new MoqStubOptionTargetEater(eatHelper, argEater);

            // Act
            var fakeOptionType = stubTargetEater.EatOption(snapshot, invocationOption);

            // Assert
            Assert.AreEqual(fakeOptionType, FakeOption.Method);
        }

        [Test]
        public void MoqStubOptionTargetWrongTypeForInvocationOptionTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentReference = Mock.Of<ICSharpExpression>();
            var lambdaParamDeclaration = Mock.Of<ILambdaParameterDeclarationAndIDeclaredElement>();
            var invocationOption = Mock.Of<IInvocationExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetInvocationReference(invocationOption) == parentReference);
            var stubTargetEater = new MoqStubOptionTargetEater(eatHelper, argEater);
            
            // Act
            Assert.Throws<MoqStubOptionTargetWrongTypeException>(() => stubTargetEater.EatOption(snapshot, invocationOption));
        }
        
        [Test]
        public void NoParentForReferenceTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>();
            var stubTargetEater = new MoqStubOptionTargetEater(eatHelper, argEater);

            // Assert
            Assert.Throws<MoqStubWrongSyntaxException>(() => stubTargetEater.EatOption(snapshot, referenceExpression));
        }

        [Test]
        public void RecoursiveEatReferenceOptionTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentInvocation = Mock.Of<IInvocationExpression>();
            var referenceExpression = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == parentInvocation);
            var eatHelper = Mock.Of<EatExpressionHelper>();
            var stubTargetEater = new Mock<MoqStubOptionTargetEater>(eatHelper, argEater) { CallBase = true };
            stubTargetEater.Setup(t => t.EatOption(snapshot, parentInvocation)).Verifiable();

            // Act
            stubTargetEater.Object.EatOption(snapshot, referenceExpression);

            // Assert
            stubTargetEater.VerifyAll();
        }

        [Test]
        public void RecoursiveEatReferenceOptionPаrentReferenceTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentInvocation = Mock.Of<IInvocationExpression>();
            var parentReference = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == parentInvocation);
            var variableDecalration = Mock.Of<IVariableDeclarationAndIDeclaredElement>();
            var referenceExpression = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == parentReference);
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(parentReference) == variableDecalration);
            var stubTargetEater = new Mock<MoqStubOptionTargetEater>(eatHelper, argEater) { CallBase = true };
            stubTargetEater.Setup(t => t.EatOption(snapshot, parentReference)).Verifiable();
            //stubTargetEater.Setup(t => t.EatOption(snapshot, parentInvocation));

            // Act
            stubTargetEater.Object.EatOption(snapshot, referenceExpression);

            // Assert
            stubTargetEater.VerifyAll();
        }

        [Test]
        public void ReturnFakeTypePropertyForParentLambdaParameterDeclarationOfReferenceOptionTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentReference = Mock.Of<IReferenceExpression>();
            var lambdaParamDeclaration = Mock.Of<ILambdaParameterDeclarationAndIDeclaredElement>();
            var referenceExpression = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == parentReference);
            var eatHelper = Mock.Of<EatExpressionHelper>(t =>  t.GetReferenceElement(parentReference) == lambdaParamDeclaration);
            var stubTargetEater = new MoqStubOptionTargetEater(eatHelper, argEater);

            // Act
            var fakeOptionType = stubTargetEater.EatOption(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(fakeOptionType, FakeOption.Property);
        }

        [Test]
        public void MoqStubOptionTargetWrongTypeForReferenceOptionTest()
        {
            // Arrange
            var argEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentReference = Mock.Of<ICSharpExpression>();
            var referenceExpression = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == parentReference);
            var eatHelper = Mock.Of<EatExpressionHelper>();
            var stubTargetEater = new MoqStubOptionTargetEater(eatHelper, argEater);

            // Act
            Assert.Throws<MoqStubOptionTargetWrongTypeException>(() => stubTargetEater.EatOption(snapshot, referenceExpression));
        }
    }
}