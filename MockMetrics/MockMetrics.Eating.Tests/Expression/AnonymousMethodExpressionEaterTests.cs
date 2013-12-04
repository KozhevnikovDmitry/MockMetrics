using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AnonymousMethodExpressionEaterTests
    {
        [Test]
        public void EatParametersTest()
        {
            // Arrange
            var parameterDeclaration = Mock.Of<IAnonymousMethodParameterDeclaration>();
            var anonymousMethodExpression = Mock.Of<IAnonymousMethodExpression>();
            Mock.Get(anonymousMethodExpression).Setup(t => t.ParameterDeclarations)
                .Returns(new TreeNodeCollection<IAnonymousMethodParameterDeclaration>(new[] { parameterDeclaration }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var anonymousMethodExpressionEater = new AnonymousMethodExpressionEater(eater.Object);

            // Act
            anonymousMethodExpressionEater.Eat(snapshot, anonymousMethodExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, parameterDeclaration));
        }

        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<IBlock>();
            var anonymousMethodExpression = Mock.Of<IAnonymousMethodExpression>(t => t.Body == body);
            Mock.Get(anonymousMethodExpression).Setup(t => t.ParameterDeclarations)
                .Returns(new TreeNodeCollection<IAnonymousMethodParameterDeclaration>(new IAnonymousMethodParameterDeclaration[0]));
            var eater = new Mock<IEater>();
            var anonymousMethodExpressionEater = new AnonymousMethodExpressionEater(eater.Object);

            // Act
            anonymousMethodExpressionEater.Eat(snapshot, anonymousMethodExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body));
        }

        [Test]
        public void ReturnStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var anonymousMethodExpression = Mock.Of<IAnonymousMethodExpression>();
            Mock.Get(anonymousMethodExpression).Setup(t => t.ParameterDeclarations)
                .Returns(new TreeNodeCollection<IAnonymousMethodParameterDeclaration>(new IAnonymousMethodParameterDeclaration[0]));
            var eater = Mock.Of<IEater>();
            var anonymousMethodExpressionEater = new AnonymousMethodExpressionEater(eater);

            // Act
            var metrics = anonymousMethodExpressionEater.Eat(snapshot, anonymousMethodExpression);

            // Assert
            Assert.AreEqual(metrics.Variable, Variable.Data);
        }
    }
}