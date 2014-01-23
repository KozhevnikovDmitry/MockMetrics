using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryContinuationEaterTests
    {
        [Test]
        public void EatIntoVariableTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var clauses = new TreeNodeCollection<IQueryClause>(new IQueryClause[0]);
            var variableDeclaration = Mock.Of<IQueryRangeVariableDeclaration>();
            var queryContinuation = Mock.Of<IQueryContinuation>(t => t.Declaration == variableDeclaration);
            Mock.Get(queryContinuation).Setup(t => t.Clauses).Returns(clauses);
            var variableEater = Mock.Of<IQueryRangeVariableDeclarationEater>(t => t.Eat(snapshot, variableDeclaration) == Variable.None);
            var queryContinuationEater = new QueryContinuationEater(eater, variableEater);

            // Act
            var result = queryContinuationEater.Eat(snapshot, queryContinuation);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatClausesTest()
        {
            // Arrange
            var clause1 = Mock.Of<IQueryClause>();
            var clause2 = Mock.Of<IQueryClause>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, clause1) == Variable.Mock
                                          && t.Eat(snapshot, clause2) == Variable.Stub);
            var clauses = new TreeNodeCollection<IQueryClause>(new[] {clause1, clause2});
            var variableDeclaration = Mock.Of<IQueryRangeVariableDeclaration>();
            var queryContinuation = Mock.Of<IQueryContinuation>(t => t.Declaration == variableDeclaration);
            Mock.Get(queryContinuation).Setup(t => t.Clauses).Returns(clauses);
            var variableEater = Mock.Of<IQueryRangeVariableDeclarationEater>(t => t.Eat(snapshot, variableDeclaration) == Variable.None);
            var queryContinuationEater = new QueryContinuationEater(eater, variableEater);

            // Act
            var result = queryContinuationEater.Eat(snapshot, queryContinuation);

            // Assert
            Assert.AreEqual(result, Variable.Stub);
            Mock.Get(eater).Verify(t => t.Eat(snapshot, clause1), Times.Once);
            Mock.Get(eater).Verify(t => t.Eat(snapshot, clause2), Times.Once);
        }
    }
}