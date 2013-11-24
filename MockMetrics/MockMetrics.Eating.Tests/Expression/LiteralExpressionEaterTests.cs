using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class LiteralExpressionEaterTests
    {
        [Test]
        public void ReturnStubCandidateTest()
        {
            // Arrange
            var literalExpression = Mock.Of<ICSharpLiteralExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var literalExpressionEater = new LiteralExpressionEater(eater);

            // Act
            var kind = literalExpressionEater.Eat(snapshot, literalExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }
    }
}
