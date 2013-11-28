using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ElementAccessExpressionEaterTests
    {
        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var elementAccessExpression = Mock.Of<IElementAccessExpression>();
            Mock.Get(elementAccessExpression).Setup(t => t.Arguments)
                .Returns(args);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argsEater = new Mock<IArgumentsEater>();
            var elementAccessExpressionEater = new ElementAccessExpressionEater(eater, argsEater.Object);

            // Act
            elementAccessExpressionEater.Eat(snapshot, elementAccessExpression);

            // Assert
            argsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }

        [Test]
        public void EatOperandTest()
        {
            // Arrange
            var operand = Mock.Of<IPrimaryExpression>();
            var elementAccessExpression = Mock.Of<IElementAccessExpression>(t => t.Operand == operand);
            Mock.Get(elementAccessExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>());
            var metrics = Metrics.Create();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, operand) == metrics);
            var argsEater = Mock.Of<IArgumentsEater>();
            var elementAccessExpressionEater = new ElementAccessExpressionEater(eater, argsEater);

            // Act
            var result = elementAccessExpressionEater.Eat(snapshot, elementAccessExpression);

            // Assert
            Assert.AreEqual(result, metrics);
        }
    }
}