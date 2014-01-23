using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryLetClauseEaterTests
    {
        [Test]
        public void EatExpressionTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var expression = Mock.Of<IQueryParameterPlatform>();
            var queryLetClause = Mock.Of<IQueryLetClause>(t => t.Expression == expression);
            var variableEater = Mock.Of<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = new Mock<IQueryParameterPlatformEater>();
            var queryLetClauseEater = new QueryLetClauseEater(eater, parameterPlatformEater.Object, variableEater);

            // Act
            var result = queryLetClauseEater.Eat(snapshot, queryLetClause);

            // Assert
            parameterPlatformEater.Verify(t => t.Eat(snapshot, expression), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatVariableDeclarationTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var variableDeclaration = Mock.Of<IQueryRangeVariableDeclaration>();
            var queryLetClause = Mock.Of<IQueryLetClause>(t => t.Declaration == variableDeclaration);
            var variableEater = new Mock<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = Mock.Of<IQueryParameterPlatformEater>();
            var queryLetClauseEater = new QueryLetClauseEater(eater, parameterPlatformEater, variableEater.Object);

            // Act
            var result = queryLetClauseEater.Eat(snapshot, queryLetClause);

            // Assert
            variableEater.Verify(t => t.Eat(snapshot, variableDeclaration), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }
    }
}