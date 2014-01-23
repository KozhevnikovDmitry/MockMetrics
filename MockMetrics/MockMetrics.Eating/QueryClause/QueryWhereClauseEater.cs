using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryWhereClauseEater : QueryClauseEater<IQueryWhereClause>
    {
        private readonly IQueryParameterPlatformEater _parameterPlatformEater;

        public QueryWhereClauseEater([NotNull]IEater eater, [NotNull] IQueryParameterPlatformEater parameterPlatformEater)
            : base(eater)
        {
            if (parameterPlatformEater == null) 
                throw new ArgumentNullException("parameterPlatformEater");

            _parameterPlatformEater = parameterPlatformEater;
        }

        public override Variable Eat(ISnapshot snapshot, IQueryWhereClause queryClause)
        {
            return _parameterPlatformEater.Eat(snapshot, queryClause.Expression);
        }
    }
}

