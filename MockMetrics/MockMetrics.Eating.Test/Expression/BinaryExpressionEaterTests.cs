using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Test.Expression
{
    [TestFixture]
    public class BinaryExpressionEaterTests
    {
        [Test]
        public void EatLeftExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var left = Mock.Of<ICSharpExpression>();
            var binaryExpression = Mock.Of<IBinaryExpression>(t => t.LeftOperand == left);
            var eater = new Mock<IEater>();
            var binaryExpressionEater = new BinaryExpressionEater(eater.Object);

            // Act
            binaryExpressionEater.Eat(snapshot, binaryExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, left), Times.Once);
        }

        [Test]
        public void AddLeftExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var left = Mock.Of<ICSharpExpression>();
            var binaryExpression = Mock.Of<IBinaryExpression>(t => t.LeftOperand == left);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, left) == ExpressionKind.None);
            var binaryExpressionEater = new BinaryExpressionEater(eater);

            // Act
            binaryExpressionEater.Eat(snapshot.Object, binaryExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.None, left), Times.Once);
        }
        
        [Test]
        public void EatRightExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var right = Mock.Of<ICSharpExpression>();
            var binaryExpression = Mock.Of<IBinaryExpression>(t => t.RightOperand == right);
            var eater = new Mock<IEater>();
            var binaryExpressionEater = new BinaryExpressionEater(eater.Object);

            // Act
            binaryExpressionEater.Eat(snapshot, binaryExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, right), Times.Once);
        }

        [Test]
        public void AddRightExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var right = Mock.Of<ICSharpExpression>();
            var binaryExpression = Mock.Of<IBinaryExpression>(t => t.RightOperand == right);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, right) == ExpressionKind.None);
            var binaryExpressionEater = new BinaryExpressionEater(eater);

            // Act
            binaryExpressionEater.Eat(snapshot.Object, binaryExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.None, right), Times.Once);
        }

        [TestCase(ExpressionKind.StubCandidate, ExpressionKind.StubCandidate, Result = ExpressionKind.StubCandidate)]
        [TestCase(ExpressionKind.Target, ExpressionKind.StubCandidate, Result = ExpressionKind.Target)]
        [TestCase(ExpressionKind.StubCandidate, ExpressionKind.Target, Result = ExpressionKind.Target)]
        [TestCase(ExpressionKind.TargetCall, ExpressionKind.StubCandidate, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.StubCandidate, ExpressionKind.TargetCall, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.Result, ExpressionKind.StubCandidate, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.StubCandidate, ExpressionKind.Result, Result = ExpressionKind.Result)]
        public ExpressionKind RetrunKindTest(ExpressionKind leftKind, ExpressionKind rightKind)
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var left = Mock.Of<ICSharpExpression>();
            var right = Mock.Of<ICSharpExpression>();
            var binaryExpression = Mock.Of<IBinaryExpression>(t => t.LeftOperand == left && t.RightOperand == right);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, left) == leftKind
                                          && t.Eat(snapshot, right) == rightKind);
            var binaryExpressionEater = new BinaryExpressionEater(eater);

            // Assert
            return binaryExpressionEater.Eat(snapshot, binaryExpression);
        }
    }
}
