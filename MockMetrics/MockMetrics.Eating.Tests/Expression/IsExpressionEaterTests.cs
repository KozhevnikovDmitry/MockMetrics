using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class IsExpressionEaterTests
    {
        [Test]
        public void ReturnStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<ICSharpExpression>();
            var isExpression = Mock.Of<IIsExpression>(t => t.Operand == operand);
            var eater = Mock.Of<IEater>();
            var isExpressionEater = new IsExpressionEater(eater);

            // Act
            var kind = isExpressionEater.Eat(snapshot, isExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatLeftExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<ICSharpExpression>();
            var isExpression = Mock.Of<IIsExpression>(t => t.Operand == operand);
            var eater = new Mock<IEater>();
            var isExpressionEater = new IsExpressionEater(eater.Object);

            // Act
            isExpressionEater.Eat(snapshot, isExpression, false);

            // Assert
            eater.Verify(t => t.Eat(snapshot, operand, false), Times.Once);
        }

        [Test]
        public void AddLeftExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var operand = Mock.Of<ICSharpExpression>();
            var isExpression = Mock.Of<IIsExpression>(t => t.Operand == operand);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, operand, false) == ExpressionKind.None);
            var isExpressionEater = new IsExpressionEater(eater);

            // Act
            isExpressionEater.Eat(snapshot.Object, isExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, operand), Times.Once);
        }
    }
}
