using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class CastExpressionEaterTests
    {
        [Test]
        public void EatTest()
        {
            var snapshot = Mock.Of<ISnapshot>();
            var op = Mock.Of<IUnaryExpression>();
            var targetType = Mock.Of<ITypeUsage>();
            var castExpression = Mock.Of<ICastExpression>(t => t.Op == op && t.TargetType == targetType);

            var eater = new Mock<IEater>();
            eater.Setup(t => t.Eat(snapshot, castExpression.Op)).Returns(ExpressionKind.None).Verifiable();

            var typeUsageEater = new Mock<ITypeUsageEater>();
            typeUsageEater.Setup(t => t.Eat(snapshot, targetType)).Returns(ExpressionKind.Mock).Verifiable();

            var kindHelper = new Mock<ExpressionKindHelper>();
            kindHelper.Setup(t => t.ValueOfKindAsTypeOfKind(ExpressionKind.None, ExpressionKind.Mock))
                .Returns(ExpressionKind.Result)
                .Verifiable();

            var castExpressionEater = new CastExpressionEater(eater.Object, typeUsageEater.Object, kindHelper.Object);

            // Act
            var kind = castExpressionEater.Eat(snapshot, castExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Result);
            typeUsageEater.VerifyAll();
            kindHelper.VerifyAll();
            eater.VerifyAll();
        }  
    }
}