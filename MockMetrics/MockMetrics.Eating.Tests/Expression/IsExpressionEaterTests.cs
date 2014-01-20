using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
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
            var metrics = isExpressionEater.Eat(snapshot, isExpression);

            // Assert
            Assert.AreEqual(metrics, Variable.Library);
        }

        [Test]
        public void AddToSnapshotTypeUsageTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var operand = Mock.Of<ICSharpExpression>();
            var typeOperand = Mock.Of<ITypeUsage>();
            var isExpression = Mock.Of<IIsExpression>(t => t.Operand == operand && t.TypeOperand == typeOperand);
            var eater = Mock.Of<IEater>();
            var isExpressionEater = new IsExpressionEater(eater);

            // Act
            isExpressionEater.Eat(snapshot.Object, isExpression);

            // Assert
            snapshot.Verify(t =>
                t.AddVariable(typeOperand, Variable.Library),
                             Times.Once);
        }

        [Test]
        public void EatExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var operand = Mock.Of<ICSharpExpression>();
            var typeOperand = Mock.Of<ITypeUsage>();
            var isExpression = Mock.Of<IIsExpression>(t => t.Operand == operand && t.TypeOperand == typeOperand);
            var eater = new Mock<IEater>();
            var isExpressionEater = new IsExpressionEater(eater.Object);

            // Act
            isExpressionEater.Eat(snapshot, isExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, operand), Times.Once());
        }
    }
}
