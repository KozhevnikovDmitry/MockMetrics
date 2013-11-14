using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AsExpressionEaterTests
    {
        [Test]
        public void EatTest()
        {
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<ICSharpExpression>();
            var typeOperand = Mock.Of<ITypeUsage>();
            var asExpression = Mock.Of<IAsExpression>(t => t.Operand == operand && t.TypeOperand == typeOperand);

            var eater = new Mock<IEater>();
            eater.Setup(t => t.Eat(snapshot, asExpression.Operand)).Returns(ExpressionKind.None).Verifiable();
            
            var typeUsageEater = new Mock<ITypeUsageEater>();
            typeUsageEater.Setup(t => t.Eat(snapshot, typeOperand)).Returns(ExpressionKind.Mock).Verifiable();
            
            var kindHelper = new Mock<ExpressionKindHelper>();
            kindHelper.Setup(t => t.ValueOfKindAsTypeOfKind(ExpressionKind.None, ExpressionKind.Mock))
                .Returns(ExpressionKind.Result)
                .Verifiable();

            var asExpressionEater = new AsExpressionEater(eater.Object, typeUsageEater.Object, kindHelper.Object);

            // Act
            var kind = asExpressionEater.Eat(snapshot, asExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Result);
            typeUsageEater.VerifyAll();
            kindHelper.VerifyAll();
            eater.VerifyAll();
        } 
    }
}