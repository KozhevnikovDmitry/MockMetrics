using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryLetClauseEater : QueryClauseEater<IQueryLetClause>
    {
        private readonly IQueryParameterPlatformEater _parameterPlatformEater;
        private readonly IQueryRangeVariableDeclarationEater _queryVariableEater;

        public QueryLetClauseEater([NotNull] IEater eater, 
                              [NotNull] IQueryParameterPlatformEater parameterPlatformEater,
                              [NotNull] IQueryRangeVariableDeclarationEater queryVariableEater) 
            : base(eater)
        {
            if (parameterPlatformEater == null) throw new ArgumentNullException("parameterPlatformEater");
            if (queryVariableEater == null) throw new ArgumentNullException("queryVariableEater");
            _parameterPlatformEater = parameterPlatformEater;
            _queryVariableEater = queryVariableEater;
        }

        public override Variable Eat(ISnapshot snapshot, IQueryLetClause queryClause)
        {
            _parameterPlatformEater.Eat(snapshot, queryClause.Expression);
            _queryVariableEater.Eat(snapshot, queryClause.Declaration);

            return Variable.None;
        }
    }
}