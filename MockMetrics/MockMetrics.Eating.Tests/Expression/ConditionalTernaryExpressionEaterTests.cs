using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ConditionalTernaryExpressionEaterTests
    {
        [Test]
        public void ReturnLibraryVarTypeTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>();
            var eater = Mock.Of<IEater>();
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater);

            // Act
            var metrics = ternaryExpressionEater.Eat(snapshot, ternaryExpression);

            // Assert
            Assert.AreEqual(metrics, Variable.Library);
        }

        [Test]
        public void EatConditionExpressionTestTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ConditionOperand == condition);
            var eater = new Mock<IEater>();
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater.Object);

            // Act
            ternaryExpressionEater.Eat(snapshot, ternaryExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, condition), Times.Once);
        }

        [Test]
        public void EatThenExpressionTestTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var then = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ThenResult == then);
            var eater = new Mock<IEater>();
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater.Object);

            // Act
            ternaryExpressionEater.Eat(snapshot, ternaryExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, then), Times.Once);
        }

        [Test]
        public void EatElseExpressionTestTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var elseOperand = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ElseResult == elseOperand);
            var eater = new Mock<IEater>();
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater.Object);

            // Act
            ternaryExpressionEater.Eat(snapshot, ternaryExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, elseOperand), Times.Once);
        }
    }
}
