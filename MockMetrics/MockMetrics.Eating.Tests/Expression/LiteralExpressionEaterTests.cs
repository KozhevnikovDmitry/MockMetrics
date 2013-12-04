using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class LiteralExpressionEaterTests
    {
        [Test]
        public void ReturnDataTest()
        {
            // Arrange
            var literalExpression = Mock.Of<ICSharpLiteralExpression>();
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var literalExpressionEater = new LiteralExpressionEater(eater);

            // Act
            var metrics = literalExpressionEater.Eat(snapshot.Object, literalExpression);

            // Assert
            Assert.AreEqual(metrics.Scope, Scope.Local);
            Assert.AreEqual(metrics.Variable, Variable.Data);
            snapshot.Verify(t => t.AddOperand(literalExpression, metrics));
        }
    }
}
