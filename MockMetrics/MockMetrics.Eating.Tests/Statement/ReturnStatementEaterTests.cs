using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class ReturnStatementEaterTests
    {
        [Test]
        public void EatValueTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var value = Mock.Of<ICSharpExpression>();
            var returnStatement = Mock.Of<IReturnStatement>(t => t.Value == value);
            var eater = new Mock<IEater>();
            var returnStatementEater = new ReturnStatementEater(eater.Object);

            // Act
            returnStatementEater.Eat(snapshot, returnStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, value), Times.Once);
        }

        [Test]
        public void NotEatNullValueTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var returnStatement = Mock.Of<IReturnStatement>();
            var eater = new Mock<IEater>();
            var returnStatementEater = new ReturnStatementEater(eater.Object);

            // Act
            returnStatementEater.Eat(snapshot, returnStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, It.IsAny<ICSharpExpression>()), Times.Never);
        }
    }
}