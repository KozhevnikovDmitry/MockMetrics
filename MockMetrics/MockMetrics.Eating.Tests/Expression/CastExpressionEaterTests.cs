using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class CastExpressionEaterTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void EatTest(bool innerEat)
        {
            var snapshot = Mock.Of<ISnapshot>();
            var op = Mock.Of<IUnaryExpression>();
            var targetType = Mock.Of<ITypeUsage>();
            var castExpression = Mock.Of<ICastExpression>(t => t.Op == op && t.TargetType == targetType);

            var eater = new Mock<IEater>();
            eater.Setup(t => t.Eat(snapshot, castExpression.Op, innerEat)).Returns(ExpressionKind.None).Verifiable();

            var typeEater = new Mock<ITypeEater>();
            typeEater.Setup(t => t.EatCastType(snapshot, targetType)).Returns(ExpressionKind.Mock).Verifiable();

            var kindHelper = new Mock<VarTypeHelper>();
            kindHelper.Setup(t => t.CastExpressionType(ExpressionKind.None, ExpressionKind.Mock))
                .Returns(ExpressionKind.Result)
                .Verifiable();

            var castExpressionEater = new CastExpressionEater(eater.Object, typeEater.Object, kindHelper.Object);

            // Act
            var kind = castExpressionEater.Eat(snapshot, castExpression, innerEat);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Result);
            typeEater.VerifyAll();
            kindHelper.VerifyAll();
            eater.VerifyAll();
        }  
    }
}