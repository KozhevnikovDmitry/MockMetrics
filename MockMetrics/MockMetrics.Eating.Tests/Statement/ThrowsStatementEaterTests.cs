using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class ThrowsStatementEaterTests
    {
        [Test]
        public void EatExceptionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var exception = Mock.Of<ICSharpExpression>();
            var throwStatement = Mock.Of<IThrowStatement>(t => t.Exception == exception);
            var eater = new Mock<IEater>();
            var throwStatementEater = new ThrowStatementEater(eater.Object);

            // Act
            throwStatementEater.Eat(snapshot, throwStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, exception), Times.Once);
        }
        [Test]
        public void NotEatNullExceptionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var throwStatement = Mock.Of<IThrowStatement>();
            var eater = new Mock<IEater>();
            var throwStatementEater = new ThrowStatementEater(eater.Object);

            // Act
            throwStatementEater.Eat(snapshot, throwStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, It.IsAny<ICSharpExpression>()), Times.Never);
        }
    }
}
