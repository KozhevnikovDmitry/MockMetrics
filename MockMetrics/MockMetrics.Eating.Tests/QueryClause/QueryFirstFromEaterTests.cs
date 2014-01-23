using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryFirstFromEaterTests
    {
        [Test]
        public void EatFromExpressionTest()
        { 
            // Arrange
            var eater = new Mock<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var fromExpression = Mock.Of<ICSharpExpression>();
            var queryFirstFrom = Mock.Of<IQueryFirstFrom>(t => t.Expression == fromExpression);
            var variableEater = Mock.Of<IQueryRangeVariableDeclarationEater>();
            var queryFirstFromEater = new QueryFirstFromEater(eater.Object, variableEater);

            // Act
            queryFirstFromEater.Eat(snapshot, queryFirstFrom);

            // Assert
            eater.Verify(t => t.Eat(snapshot, fromExpression), Times.Once);
        }

        [Test]
        public void EatVariableDeclarationTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var variableDeclaration = Mock.Of<IQueryRangeVariableDeclaration>();
            var queryFirstFrom = Mock.Of<IQueryFirstFrom>(t => t.Declaration == variableDeclaration);
            var variableEater = Mock.Of<IQueryRangeVariableDeclarationEater>(t => t.Eat(snapshot, variableDeclaration) == Variable.None);
            var queryFirstFromEater = new QueryFirstFromEater(eater, variableEater);
            
            // Act
            var result = queryFirstFromEater.Eat(snapshot, queryFirstFrom);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }
    }
}