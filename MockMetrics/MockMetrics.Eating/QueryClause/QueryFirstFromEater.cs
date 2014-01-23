using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public interface IQueryFirstFromEater : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IQueryFirstFrom firstFrom);
    }

    public class QueryFirstFromEater : IQueryFirstFromEater
    {
        private readonly IEater _eater;
        private readonly IQueryRangeVariableDeclarationEater _queryVariableEater;

        public QueryFirstFromEater([NotNull] IEater eater,
            [NotNull] IQueryRangeVariableDeclarationEater queryVariableEater)
        {
            if (eater == null) throw new ArgumentNullException("eater");
            if (queryVariableEater == null)
                throw new ArgumentNullException("queryVariableEater");

            _eater = eater;
            _queryVariableEater = queryVariableEater;
        }

        public Variable Eat(ISnapshot snapshot, IQueryFirstFrom firstFrom)
        {
            _eater.Eat(snapshot, firstFrom.Expression);
            return _queryVariableEater.Eat(snapshot, firstFrom.Declaration);
        }
    }
}