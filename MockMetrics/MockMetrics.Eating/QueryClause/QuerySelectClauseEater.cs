using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QuerySelectClauseEater : QueryClauseEater<IQuerySelectClause> 
    {
        private readonly IQueryParameterPlatformEater _parameterPlatformEater;

        public QuerySelectClauseEater([NotNull] IEater eater,
                                      [NotNull] IQueryParameterPlatformEater parameterPlatformEater)
            : base(eater)
        {
            if (parameterPlatformEater == null) throw new ArgumentNullException("parameterPlatformEater");
            _parameterPlatformEater = parameterPlatformEater;
        }

        public override Variable Eat(ISnapshot snapshot, IQuerySelectClause queryClause)
        {
            return _parameterPlatformEater.Eat(snapshot, queryClause.Expression);
        }
    }
}