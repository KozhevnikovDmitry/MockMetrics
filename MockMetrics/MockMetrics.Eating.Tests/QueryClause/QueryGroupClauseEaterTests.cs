using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryGroupClauseEaterTests
    {
        [Test]
        public void EatSubjectTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var subject = Mock.Of<IQueryParameterPlatform>();
            var queryGroupClause = Mock.Of<IQueryGroupClause>(t => t.Subject == subject);
            var parameterPlatformEater = new Mock<IQueryParameterPlatformEater>();
            var queryGroupClauseEater = new QueryGroupClauseEater(eater, parameterPlatformEater.Object);

            // Act
            var result = queryGroupClauseEater.Eat(snapshot, queryGroupClause);

            // Assert
            parameterPlatformEater.Verify(t => t.Eat(snapshot, subject), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatCriteriaTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var criteria = Mock.Of<IQueryParameterPlatform>();
            var queryGroupClause = Mock.Of<IQueryGroupClause>(t => t.Criteria == criteria);
            var parameterPlatformEater = new Mock<IQueryParameterPlatformEater>();
            var queryGroupClauseEater = new QueryGroupClauseEater(eater, parameterPlatformEater.Object);

            // Act
            var result = queryGroupClauseEater.Eat(snapshot, queryGroupClause);

            // Assert
            parameterPlatformEater.Verify(t => t.Eat(snapshot, criteria), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }
    }
}