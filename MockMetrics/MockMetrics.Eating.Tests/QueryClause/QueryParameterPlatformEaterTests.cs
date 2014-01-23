using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.QueryClause
{
    [TestFixture]
    public class QueryParameterPlatformEaterTests
    {
        [Test]
        public void EatValueTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var valueExpression = Mock.Of<ICSharpExpression>();
            var queryParameterPlatform = Mock.Of<IQueryParameterPlatform>(t => t.Value == valueExpression);
             var eater = Mock.Of<IEater>(t => t.Eat(snapshot, valueExpression) == Variable.None);
            var queryParameterPlatformEater = new QueryParameterPlatformEater(eater);

            // Act
            var result = queryParameterPlatformEater.Eat(snapshot, queryParameterPlatform);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }
    }
}