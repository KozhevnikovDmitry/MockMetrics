using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryJoinClauseEater : QueryClauseEater<IQueryJoinClause>
    {
        private readonly IQueryParameterPlatformEater _parameterPlatformEater;
        private readonly IQueryRangeVariableDeclarationEater _queryVariableEater;

        public QueryJoinClauseEater([NotNull] IEater eater, 
                               [NotNull] IQueryParameterPlatformEater parameterPlatformEater,
                               [NotNull] IQueryRangeVariableDeclarationEater queryVariableEater) 
            : base(eater)
        {
            if (parameterPlatformEater == null) throw new ArgumentNullException("parameterPlatformEater");
            if (queryVariableEater == null) throw new ArgumentNullException("queryVariableEater");
            _parameterPlatformEater = parameterPlatformEater;
            _queryVariableEater = queryVariableEater;
        }
        

        public override Variable Eat(ISnapshot snapshot, IQueryJoinClause queryClause)
        {
            _queryVariableEater.Eat(snapshot, queryClause.JoinDeclaration);

            if (queryClause.IntoDeclaration != null)
            {
                _queryVariableEater.Eat(snapshot, queryClause.IntoDeclaration);
            }

            _parameterPlatformEater.Eat(snapshot, queryClause.EqualsExpression);
            _parameterPlatformEater.Eat(snapshot, queryClause.OnExpression);
            Eater.Eat(snapshot, queryClause.InExpression);

            return Variable.None;
        }
    }
}