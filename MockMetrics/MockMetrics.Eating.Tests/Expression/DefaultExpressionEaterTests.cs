using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class DefaultExpressionEaterTests
    {
        [Test]
        public void EatDynamicAsStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var typeUsage = Mock.Of<IDynamicTypeUsage>();
            var defaultExpression = Mock.Of<IDefaultExpression>(t => t.TypeName == typeUsage);
            var typeEater = new Mock<ITypeEater>();
            typeEater.Setup(t => t.EatCastType(snapshot, typeUsage)).Returns(ExpressionKind.Stub).Verifiable();
            var eater = Mock.Of<IEater>();
            var defaultExpressionEater = new DefaultExpressionEater(eater, typeEater.Object);

            // Act
            var kind = defaultExpressionEater.Eat(snapshot, defaultExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
            typeEater.VerifyAll();
        }
    }
}
