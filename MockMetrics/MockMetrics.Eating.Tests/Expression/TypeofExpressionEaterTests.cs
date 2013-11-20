using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class TypeofExpressionEaterTests
    {
        [Test]
        public void ReturnStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var typeofExpression = Mock.Of<ITypeofExpression>();
            var eater = Mock.Of<IEater>();
            var typeofExpressionEater = new TypeofExpressionEater(eater);

            // Act
            var kind = typeofExpressionEater.Eat(snapshot, typeofExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        } 
    }
}