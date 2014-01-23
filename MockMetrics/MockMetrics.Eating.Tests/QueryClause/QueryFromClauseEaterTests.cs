using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryFromClauseEaterTests
    {
        [Test]
        public void EatFromExpressionTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var fromExpression = Mock.Of<IQueryParameterPlatform>();
            var queryFromClause = Mock.Of<IQueryFromClause>(t => t.Expression == fromExpression);
            var variableEater = Mock.Of<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = new Mock<IQueryParameterPlatformEater>();
            var queryFromClauseEater = new QueryFromClauseEater(eater, parameterPlatformEater.Object, variableEater);

            // Act
            var result = queryFromClauseEater.Eat(snapshot, queryFromClause);

            // Assert
            parameterPlatformEater.Verify(t => t.Eat(snapshot, fromExpression), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatVariableDeclarationTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var variableDeclaration = Mock.Of<IQueryRangeVariableDeclaration>();
            var queryFromClause = Mock.Of<IQueryFromClause>(t => t.Declaration == variableDeclaration);
            var variableEater = new Mock<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = Mock.Of<IQueryParameterPlatformEater>();
            var queryFromClauseEater = new QueryFromClauseEater(eater, parameterPlatformEater, variableEater.Object);
            
            // Act
            var result = queryFromClauseEater.Eat(snapshot, queryFromClause);

            // Assert
            variableEater.Verify(t => t.Eat(snapshot, variableDeclaration), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }
    }
}