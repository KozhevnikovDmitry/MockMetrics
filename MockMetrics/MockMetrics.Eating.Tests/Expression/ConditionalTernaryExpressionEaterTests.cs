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
            var kind = ternaryExpressionEater.Eat(snapshot, ternaryExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatConditionExpressionTest()
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
        public void AddConditionExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ConditionOperand == condition);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, condition) == ExpressionKind.None);
            var binaryExpressionEater = new ConditionalTernaryExpressionEater(eater);

            // Act
            binaryExpressionEater.Eat(snapshot.Object, ternaryExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.None, condition), Times.Once);
        }

        [Test]
        public void EatThenExpressionTest()
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
        public void AddThenExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var then = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ThenResult == then);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, then) == ExpressionKind.None);
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater);

            // Act
            ternaryExpressionEater.Eat(snapshot.Object, ternaryExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.None, then), Times.Once);
        }

        [Test]
        public void EatElseExpressionTest()
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

        [Test]
        public void AddElseExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var elseOperand = Mock.Of<ICSharpExpression>();
            var ternaryExpression = Mock.Of<IConditionalTernaryExpression>(t => t.ElseResult == elseOperand);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, elseOperand) == ExpressionKind.None);
            var ternaryExpressionEater = new ConditionalTernaryExpressionEater(eater);

            // Act
            ternaryExpressionEater.Eat(snapshot.Object, ternaryExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.None, elseOperand), Times.Once);
        }
    }
}
