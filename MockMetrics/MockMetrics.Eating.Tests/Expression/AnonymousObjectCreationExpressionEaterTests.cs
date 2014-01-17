using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AnonymousObjectCreationExpressionEaterTests
    {
        [Test]
        public void EatMemberDeclarationsTest()
        {
            // Arrange
            var memberDeclaration = Mock.Of<IAnonymousMemberDeclaration>();
            var anonymousObjectCreationExpression = Mock.Of<IAnonymousObjectCreationExpression>();
            Mock.Get(anonymousObjectCreationExpression).Setup(t => t.AnonymousInitializer.MemberInitializers)
                .Returns(new TreeNodeCollection<IAnonymousMemberDeclaration>(new[] { memberDeclaration }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var anonymousObjectCreationExpressionEater = new AnonymousObjectCreationExpressionEater(eater.Object);

            // Act
            anonymousObjectCreationExpressionEater.Eat(snapshot, anonymousObjectCreationExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, memberDeclaration), Times.Once());
        }

        [Test]
        public void ReturnStubCandidateTest()
        {
            var anonymousObjectCreationExpression = Mock.Of<IAnonymousObjectCreationExpression>();
            Mock.Get(anonymousObjectCreationExpression).Setup(t => t.AnonymousInitializer.MemberInitializers)
                .Returns(new TreeNodeCollection<IAnonymousMemberDeclaration>(new IAnonymousMemberDeclaration[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var anonymousObjectCreationExpressionEater = new AnonymousObjectCreationExpressionEater(eater);

            // Act
            var metrics = anonymousObjectCreationExpressionEater.Eat(snapshot, anonymousObjectCreationExpression);

            // Assert
            Assert.AreEqual(metrics, Variable.Library);

        } 
    }
}