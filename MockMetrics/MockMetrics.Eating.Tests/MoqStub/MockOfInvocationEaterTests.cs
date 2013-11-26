using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MoqStub;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.MoqStub
{
    [TestFixture]
    public class MockOfInvocationEaterTests
    {
        [Test]
        public void ReturnStubTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var mockOfExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(mockOfExpression).Setup(t => t.Arguments).Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var stubOptionEater = Mock.Of<IMoqStubOptionsEater>();
            var moqOfEater = new MockOfInvocationEater(stubOptionEater);

            // Act
            var kind = moqOfEater.Eat(snapshot, mockOfExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
        }

        [Test]
        public void AddToSnapshotWhenInnerEatIsTrueTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var mockOfExpression = Mock.Of<IInvocationExpression>();
            Mock.Get(mockOfExpression).Setup(t => t.Arguments).Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var stubOptionEater = Mock.Of<IMoqStubOptionsEater>();
            var moqOfEater = new MockOfInvocationEater(stubOptionEater);

            // Act
            var kind = moqOfEater.Eat(snapshot.Object, mockOfExpression, true);

            // Assert
            snapshot.Verify(t => t.Add(kind, mockOfExpression), Times.Once);
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
            var moqOfEater = new MockOfInvocationEater(stubOptionEater);
            
            // Assert
            Assert.Throws<MoqStubWrongSyntaxException>(() => moqOfEater.Eat(snapshot, mockOfExpression, false));
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
            var moqOfEater = new MockOfInvocationEater(stubOptionEater);

            // Assert
            Assert.Throws<MoqStubWrongSyntaxException>(() => moqOfEater.Eat(snapshot, mockOfExpression, false));
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
            var moqOfEater = new MockOfInvocationEater(stubOptionEater.Object);

            // Act
            moqOfEater.Eat(snapshot, mockOfExpression, false);

            // Assert
            stubOptionEater.Verify(t => t.EatStubOptions(snapshot, option));
        }
    }
}