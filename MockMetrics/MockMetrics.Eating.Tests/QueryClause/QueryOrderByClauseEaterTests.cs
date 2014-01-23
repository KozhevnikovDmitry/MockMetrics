using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryOrderByClauseEaterTests
    {
        [Test]
        public void EatOrderingsTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var orderinExpression1 = Mock.Of<IQueryParameterPlatform>();
            var orderinExpression2 = Mock.Of<IQueryParameterPlatform>();
            var ordering1 = Mock.Of<IQueryOrdering>(t => t.Expression == orderinExpression1);
            var ordering2 = Mock.Of<IQueryOrdering>(t => t.Expression == orderinExpression2);
            var orderings = new TreeNodeCollection<IQueryOrdering>(new[] { ordering1, ordering2 });
            var orderByClause = Mock.Of<IQueryOrderByClause>();
            Mock.Get(orderByClause).Setup(t => t.Orderings).Returns(orderings);
            var parameterPlatformEater = new Mock<IQueryParameterPlatformEater>();
            var queryOrderByClauseEater = new QueryOrderByClauseEater(eater, parameterPlatformEater.Object);

            // Act
            var result = queryOrderByClauseEater.Eat(snapshot, orderByClause);

            // Assert
            parameterPlatformEater.Verify(t => t.Eat(snapshot, orderinExpression1), Times.Once);
            parameterPlatformEater.Verify(t => t.Eat(snapshot, orderinExpression2), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }
    }
}