using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class ForeachStatementEaterTests
    {
        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<ICSharpStatement>();
            var foreachStatement = Mock.Of<IForeachStatement>(t => t.Body == body);
            var eater = new Mock<IEater>();
            var foreachEater = new ForeachStatementEater(eater.Object);

            // Act
            foreachEater.Eat(snapshot, foreachStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }

        [Test]
        public void EatCollectionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var collection = Mock.Of<ICSharpExpression>();
            var foreachStatement = Mock.Of<IForeachStatement>(t => t.Collection == collection);
            var eater = new Mock<IEater>();
            var foreachEater = new ForeachStatementEater(eater.Object);

            // Act
            foreachEater.Eat(snapshot, foreachStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, collection), Times.Once);
        }

        [Test]
        public void EatIteratorDeclarationTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var iterator = Mock.Of<IForeachVariableDeclaration>();
            var foreachStatement = Mock.Of<IForeachStatement>(t => t.IteratorDeclaration == iterator);
            var eater = new Mock<IEater>();
            var foreachEater = new ForeachStatementEater(eater.Object);

            // Act
            foreachEater.Eat(snapshot, foreachStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, iterator), Times.Once);
        }
    }
}