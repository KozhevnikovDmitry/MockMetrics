using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ConditionalTernaryExpressionEaterTests
    {
        [Test]
        public void ReturnStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>();
            var eater = Mock.Of<IEater>();
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater);

            // Act
            var kind = ternaryExpressionEater.Eat(snapshot, ternaryExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EatConditionExpressionTest_TranslateInnerEatTest(bool innerEat)
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ConditionOperand == condition);
            var eater = new Mock<IEater>();
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater.Object);

            // Act
            ternaryExpressionEater.Eat(snapshot, ternaryExpression, innerEat);

            // Assert
            eater.Verify(t => t.Eat(snapshot, condition, innerEat), Times.Once);
        }

        public void AddConditionExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ConditionOperand == condition);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, condition, false) == ExpressionKind.None);
            var binaryExpressionEater = new ConditionalTernaryExpressionEater(eater);

            // Act
            binaryExpressionEater.Eat(snapshot.Object, ternaryExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, condition), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EatThenExpressionTest_TranslateInnerEatTest(bool innerEat)
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var then = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ThenResult == then);
            var eater = new Mock<IEater>();
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater.Object);

            // Act
            ternaryExpressionEater.Eat(snapshot, ternaryExpression, innerEat);

            // Assert
            eater.Verify(t => t.Eat(snapshot, then, innerEat), Times.Once);
        }

        [Test]
        public void AddThenExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var then = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ThenResult == then);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, then, false) == ExpressionKind.None);
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater);

            // Act
            ternaryExpressionEater.Eat(snapshot.Object, ternaryExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, then), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EatElseExpressionTest_TranslateInnerEatTest(bool innerEat)
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var elseOperand = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ElseResult == elseOperand);
            var eater = new Mock<IEater>();
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater.Object);

            // Act
            ternaryExpressionEater.Eat(snapshot, ternaryExpression, innerEat);

            // Assert
            eater.Verify(t => t.Eat(snapshot, elseOperand, innerEat), Times.Once);
        }

        [Test]
        public void AddElseExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var elseOperand = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ElseResult == elseOperand);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, elseOperand, false) == ExpressionKind.None);
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater);

            // Act
            ternaryExpressionEater.Eat(snapshot.Object, ternaryExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, elseOperand), Times.Once);
        }
    }
}
