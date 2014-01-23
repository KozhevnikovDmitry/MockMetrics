using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryOrderByClauseEater : QueryClauseEater<IQueryOrderByClause>
    {
        private readonly IQueryParameterPlatformEater _parameterPlatformEater;

        public QueryOrderByClauseEater([NotNull] IEater eater, [NotNull] IQueryParameterPlatformEater parameterPlatformEater)
            : base(eater)
        {
            if (parameterPlatformEater == null) throw new ArgumentNullException("parameterPlatformEater");
            _parameterPlatformEater = parameterPlatformEater;
        }

        public override Variable Eat(ISnapshot snapshot, IQueryOrderByClause queryClause)
        
        {
            foreach (var queryOrdering in queryClause.Orderings)
            {
                _parameterPlatformEater.Eat(snapshot, queryOrdering.Expression); 
            }
            
            return Variable.None;
        }
    }
}