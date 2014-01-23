using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryWhereClauseEaterTests
    {
        [Test]
        public void EatExpressionTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var expression = Mock.Of<IQueryParameterPlatform>();
            var queryWhereClause = Mock.Of<IQueryWhereClause>(t => t.Expression == expression);
            var parameterPlatformEater = new Mock<IQueryParameterPlatformEater>();
            parameterPlatformEater.Setup(t => t.Eat(snapshot, expression)).Returns(Variable.None);
            var queryWhereClauseEater = new QueryWhereClauseEater(eater, parameterPlatformEater.Object);

            // Act
            var result = queryWhereClauseEater.Eat(snapshot, queryWhereClause);

            // Assert
            parameterPlatformEater.Verify(t => t.Eat(snapshot, expression), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }
    }
}

