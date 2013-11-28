using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class LambdaExpressionEaterTests
    {
        [Test]
        public void EatParametersTest()
        {
            // Arrange
            var parameterDeclaration = Mock.Of<ILambdaParameterDeclaration>();
            var lambdaExpression = Mock.Of<ILambdaExpression>();
            Mock.Get(lambdaExpression).Setup(t => t.ParameterDeclarations)
                .Returns(new TreeNodeCollection<ILambdaParameterDeclaration>(new[] { parameterDeclaration }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var lambdaExpressionEater = new LambdaExpressionEater(eater.Object);

            // Act
            lambdaExpressionEater.Eat(snapshot, lambdaExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, parameterDeclaration));
        }

        [Test]
        public void EatBodyBlockTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var bodyBlock = Mock.Of<IBlock>();
            var lambdaExpression = Mock.Of<ILambdaExpression>(t => t.BodyBlock == bodyBlock);
            Mock.Get(lambdaExpression).Setup(t => t.ParameterDeclarations)
                .Returns(new TreeNodeCollection<ILambdaParameterDeclaration>(new ILambdaParameterDeclaration[0]));
            var eater = new Mock<IEater>();
            var lambdaExpressionEater = new LambdaExpressionEater(eater.Object);

            // Act
            lambdaExpressionEater.Eat(snapshot, lambdaExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, bodyBlock));
        }
        
        [Test]
        public void EatBodyExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var bodyExpression = Mock.Of<ICSharpExpression>();
            var lambdaExpression = Mock.Of<ILambdaExpression>(t => t.BodyExpression == bodyExpression);
            Mock.Get(lambdaExpression).Setup(t => t.ParameterDeclarations)
                .Returns(new TreeNodeCollection<ILambdaParameterDeclaration>(new ILambdaParameterDeclaration[0]));
            var eater = new Mock<IEater>();
            var lambdaExpressionEater = new LambdaExpressionEater(eater.Object);

            // Act
            lambdaExpressionEater.Eat(snapshot, lambdaExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, bodyExpression));
        }

        [Test]
        public void ReturnStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var lambdaExpression = Mock.Of<ILambdaExpression>();
            Mock.Get(lambdaExpression).Setup(t => t.ParameterDeclarations)
                .Returns(new TreeNodeCollection<ILambdaParameterDeclaration>(new ILambdaParameterDeclaration[0]));
            var eater = Mock.Of<IEater>();
            var lambdaExpressionEater = new LambdaExpressionEater(eater);

            // Act
            var metrics = lambdaExpressionEater.Eat(snapshot, lambdaExpression);

            // Assert
            Assert.AreEqual(metrics.VarType, VarType.Internal);
        }
    }
}