using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryJoinClauseEaterTests
    {
        [Test]
        public void EatJoinVariableDeclarationTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var joinDeclaration = Mock.Of<IQueryRangeVariableDeclaration>();
            var queryJoinClause = Mock.Of<IQueryJoinClause>(t => t.JoinDeclaration == joinDeclaration);
            var variableEater = new Mock<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = Mock.Of<IQueryParameterPlatformEater>();
            var queryJoinClauseEater = new QueryJoinClauseEater(eater, parameterPlatformEater, variableEater.Object);
            
            // Act
            var result = queryJoinClauseEater.Eat(snapshot, queryJoinClause);

            // Assert
            variableEater.Verify(t => t.Eat(snapshot, joinDeclaration), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }
        
        [Test]
        public void EatIntoVariableDeclarationTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var intoDeclaration = Mock.Of<IQueryRangeVariableDeclaration>();
            var queryJoinClause = Mock.Of<IQueryJoinClause>(t => t.IntoDeclaration == intoDeclaration);
            var variableEater = new Mock<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = Mock.Of<IQueryParameterPlatformEater>();
            var queryJoinClauseEater = new QueryJoinClauseEater(eater, parameterPlatformEater, variableEater.Object);

            // Act
            var result = queryJoinClauseEater.Eat(snapshot, queryJoinClause);

            // Assert
            variableEater.Verify(t => t.Eat(snapshot, intoDeclaration), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void NotEatNullIntoVariableDeclarationTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var joinDeclaration = Mock.Of<IQueryRangeVariableDeclaration>();
            var queryJoinClause = Mock.Of<IQueryJoinClause>(t => t.JoinDeclaration == joinDeclaration);
            var variableEater = new Mock<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = Mock.Of<IQueryParameterPlatformEater>();
            var queryJoinClauseEater = new QueryJoinClauseEater(eater, parameterPlatformEater, variableEater.Object);

            // Act
            var result = queryJoinClauseEater.Eat(snapshot, queryJoinClause);

            // Assert
            variableEater.Verify(t => t.Eat(snapshot, It.Is<IQueryRangeVariableDeclaration>(q => !q.Equals(joinDeclaration))), Times.Never);
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatEqualsExpressionTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var equalsExpression = Mock.Of<IQueryParameterPlatform>();
            var queryJoinClause = Mock.Of<IQueryJoinClause>(t => t.EqualsExpression == equalsExpression);
            var variableEater = Mock.Of<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = new Mock<IQueryParameterPlatformEater>();
            var queryJoinClauseEater = new QueryJoinClauseEater(eater, parameterPlatformEater.Object, variableEater);

            // Act
            var result = queryJoinClauseEater.Eat(snapshot, queryJoinClause);

            // Assert
            parameterPlatformEater.Verify(t => t.Eat(snapshot, equalsExpression), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatOnExpressionTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var onExpression = Mock.Of<IQueryParameterPlatform>();
            var queryJoinClause = Mock.Of<IQueryJoinClause>(t => t.OnExpression == onExpression);
            var variableEater = Mock.Of<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = new Mock<IQueryParameterPlatformEater>();
            var queryJoinClauseEater = new QueryJoinClauseEater(eater, parameterPlatformEater.Object, variableEater);

            // Act
            var result = queryJoinClauseEater.Eat(snapshot, queryJoinClause);

            // Assert
            parameterPlatformEater.Verify(t => t.Eat(snapshot, onExpression), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }
        
        [Test]
        public void EatInExpressionTest()
        {
            // Arrange
            var eater = new Mock<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var inExpression = Mock.Of<ICSharpExpression>();
            var queryJoinClause = Mock.Of<IQueryJoinClause>(t => t.InExpression == inExpression);
            var variableEater = Mock.Of<IQueryRangeVariableDeclarationEater>();
            var parameterPlatformEater = Mock.Of<IQueryParameterPlatformEater>();
            var queryJoinClauseEater = new QueryJoinClauseEater(eater.Object, parameterPlatformEater, variableEater);

            // Act
            var result = queryJoinClauseEater.Eat(snapshot, queryJoinClause);

            // Assert
            eater.Verify(t => t.Eat(snapshot, inExpression), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }
    }
}