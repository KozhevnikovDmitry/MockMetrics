using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqStub;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.MoqStub
{
    [TestFixture]
    public class MockOfInvocationEaterTests
    {
        [Test]
        public void AddToSnapshotStandaloneMoqStubExpressionTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var mockOfExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(mockOfExpression).Setup(t => t.Arguments).Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var stubOptionEater = Mock.Of<IMoqStubOptionsEater>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.IsStandaloneMoqStubExpression(mockOfExpression) == true);
            var moqOfEater = new MockOfInvocationEater(stubOptionEater, eatHelper);

            // Act
            moqOfEater.Eat(snapshot.Object, mockOfExpression);

            // Assert
            snapshot.Verify(t => t.AddOperand(mockOfExpression, It.Is<Metrics>(m => m.Scope == Scope.Local && m.Variable == Variable.Mock)), Times.Once);
        }

        [Test]
        public void StubOptionPredicateHasMoreThenOneParametersTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var mockOfExpression = Mock.Of<IInvocationExpression>();
            var stubOptions = Mock.Of<ILambdaExpression>();
            Mock.Get(mockOfExpression).Setup(t => t.Arguments).Returns(new TreeNodeCollection<ICSharpArgument>(new [] {Mock.Of<ICSharpArgument>(a => a.Value == stubOptions)}));
            Mock.Get(stubOptions).Setup(t => t.ParameterDeclarations).Returns(new TreeNodeCollection<ILambdaParameterDeclaration>(new ILambdaParameterDeclaration[2]));
            var stubOptionEater = Mock.Of<IMoqStubOptionsEater>(); 
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.IsStandaloneMoqStubExpression(mockOfExpression) == false);
            var moqOfEater = new MockOfInvocationEater(stubOptionEater, eatHelper);
            
            // Assert
            Assert.Throws<MoqStubWrongSyntaxException>(() => moqOfEater.Eat(snapshot, mockOfExpression));
        }

        [Test]
        public void StubOptionPredicateHasBodyBlockInsteadOfBodyExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var mockOfExpression = Mock.Of<IInvocationExpression>();
            var stubOptions = Mock.Of<ILambdaExpression>(t => t.BodyBlock == Mock.Of<IBlock>());
            Mock.Get(mockOfExpression).Setup(t => t.Arguments).Returns(new TreeNodeCollection<ICSharpArgument>(new[] { Mock.Of<ICSharpArgument>(a => a.Value == stubOptions) }));
            Mock.Get(stubOptions).Setup(t => t.ParameterDeclarations).Returns(new TreeNodeCollection<ILambdaParameterDeclaration>(new ILambdaParameterDeclaration[1]));
            var stubOptionEater = Mock.Of<IMoqStubOptionsEater>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.IsStandaloneMoqStubExpression(mockOfExpression) == false);
            var moqOfEater = new MockOfInvocationEater(stubOptionEater, eatHelper);

            // Assert
            Assert.Throws<MoqStubWrongSyntaxException>(() => moqOfEater.Eat(snapshot, mockOfExpression));
        }

        [Test]
        public void EatStubOptionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var mockOfExpression = Mock.Of<IInvocationExpression>();
            var option = Mock.Of<ICSharpExpression>();
            var stubOptions = Mock.Of<ILambdaExpression>(t => t.BodyExpression == option);
            Mock.Get(mockOfExpression).Setup(t => t.Arguments).Returns(new TreeNodeCollection<ICSharpArgument>(new[] { Mock.Of<ICSharpArgument>(a => a.Value == stubOptions) }));
            Mock.Get(stubOptions).Setup(t => t.ParameterDeclarations).Returns(new TreeNodeCollection<ILambdaParameterDeclaration>(new ILambdaParameterDeclaration[1]));
            var stubOptionEater = new Mock<IMoqStubOptionsEater>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.IsStandaloneMoqStubExpression(mockOfExpression) == false);
            var moqOfEater = new MockOfInvocationEater(stubOptionEater.Object, eatHelper);

            // Act
            moqOfEater.Eat(snapshot, mockOfExpression);

            // Assert
            stubOptionEater.Verify(t => t.EatStubOptions(snapshot, option));
        }
    }
}