using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryGroupClauseEater : QueryClauseEater<IQueryGroupClause>
    {
        private readonly IQueryParameterPlatformEater _parameterPlatformEater;

        public QueryGroupClauseEater(IEater eater, [NotNull] IQueryParameterPlatformEater parameterPlatformEater) : base(eater)
        {
            if (parameterPlatformEater == null) throw new ArgumentNullException("parameterPlatformEater");
            _parameterPlatformEater = parameterPlatformEater;
        }

        public override Variable Eat(ISnapshot snapshot, IQueryGroupClause queryClause)
        {
            _parameterPlatformEater.Eat(snapshot, queryClause.Subject);
            _parameterPlatformEater.Eat(snapshot, queryClause.Criteria);

            return Variable.None;
        }
    }
}