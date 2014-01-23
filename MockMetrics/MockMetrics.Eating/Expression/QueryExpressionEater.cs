using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;

namespace MockMetrics.Eating.Expression
{
    public class QueryExpressionEater : ExpressionEater<IQueryExpression>
    {
        private readonly IQueryContinuationEater _continuationEater;
        private readonly IQueryFirstFromEater _queryFirstFromEater;

        public QueryExpressionEater(IEater eater, IQueryContinuationEater continuationEater, IQueryFirstFromEater queryFirstFromEater)
            : base(eater)
        {
            _continuationEater = continuationEater;
            _queryFirstFromEater = queryFirstFromEater;
        }

        public override Variable Eat(ISnapshot snapshot, IQueryExpression expression)
        {
            var result = _queryFirstFromEater.Eat(snapshot, expression.From);

            foreach (var queryClause in expression.Clauses)
            {
                result = Eater.Eat(snapshot, queryClause);
            }

            foreach (var queryContinuation in expression.Continuations)
            {
                result = _continuationEater.Eat(snapshot, queryContinuation);
            }

            return result;
        }
    }
}
