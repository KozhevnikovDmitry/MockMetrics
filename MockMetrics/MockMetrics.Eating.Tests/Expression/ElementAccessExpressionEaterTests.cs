using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
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
            elementAccessExpressionEater.Eat(snapshot, elementAccessExpression, false);

            // Assert
            argsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EatOperand_TranslateInnerEatTest(bool innerEat)
        {
            // Arrange
            var operand = Mock.Of<IPrimaryExpression>();
            var elementAccessExpression = Mock.Of<IElementAccessExpression>(t => t.Operand == operand);
            Mock.Get(elementAccessExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>());
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var argsEater = new Mock<IArgumentsEater>();
            var elementAccessExpressionEater = new ElementAccessExpressionEater(eater.Object, argsEater.Object);

            // Act
            elementAccessExpressionEater.Eat(snapshot, elementAccessExpression, innerEat);

            // Assert
            eater.Verify(t => t.Eat(snapshot, operand, innerEat), Times.Once);
        }

        [Test]
        public void ReturnStubCandidateTest()
        {
            // Arrange
            var elementAccessExpression = Mock.Of<IElementAccessExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var elementAccessExpressionEater = new ElementAccessExpressionEater(eater, argsEater);

            // Act
            var kind = elementAccessExpressionEater.Eat(snapshot, elementAccessExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }
    }
}