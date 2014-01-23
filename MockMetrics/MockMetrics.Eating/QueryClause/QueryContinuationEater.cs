using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public interface IQueryContinuationEater : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IQueryContinuation continuation);
    }

    public class QueryContinuationEater : IQueryContinuationEater
    {
        private readonly IEater _eater;
        private readonly IQueryRangeVariableDeclarationEater _queryVariableEater;

        public QueryContinuationEater([NotNull] IEater eater,
                                       [NotNull] IQueryRangeVariableDeclarationEater queryVariableEater)
        {
            if (eater == null) 
                throw new ArgumentNullException("eater");
            if (queryVariableEater == null) throw new ArgumentNullException("queryVariableEater");

            _eater = eater;
            _queryVariableEater = queryVariableEater;
        }

        public Variable Eat(ISnapshot snapshot, IQueryContinuation continuation)
        {
            var result = _queryVariableEater.Eat(snapshot, continuation.Declaration);

            foreach (var queryClause in continuation.Clauses)
            {
                result = _eater.Eat(snapshot, queryClause);
            }

            return result;
        }
    }
}