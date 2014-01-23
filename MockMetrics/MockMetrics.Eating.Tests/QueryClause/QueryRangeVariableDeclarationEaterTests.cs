using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.DeclaredElements;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryRangeVariableDeclarationEaterTests
    {
        [Test]
        public void EatVariableByTypeTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var type = Mock.Of<IType>();
            var queryVariableDeclaration = Mock.Of<IQueryRangeVariableDeclaration>(t => t.DeclaredElement == Mock.Of<IQueryRangeVariable>(q => q.Type == type));
             var eater = Mock.Of<IEater>();
            var metricHelper = Mock.Of<IMetricHelper>(t =>t.MetricsForType(snapshot.Object, type) == Variable.None);
            var queryRangeVariableDeclarationEater = new QueryRangeVariableDeclarationEater(eater,metricHelper);

            // Act
            var result = queryRangeVariableDeclarationEater.Eat(snapshot.Object, queryVariableDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(queryVariableDeclaration, Variable.None));
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void ArgumentVariableDeclaredElementNullTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var queryVariableDeclaration = Mock.Of<IQueryRangeVariableDeclaration>();
            var eater = Mock.Of<IEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var queryRangeVariableDeclarationEater = new QueryRangeVariableDeclarationEater(eater,metricHelper);

            // Assert
            Assert.Throws<ArgumentException>(() => queryRangeVariableDeclarationEater.Eat(snapshot, queryVariableDeclaration));
        }
    }
}