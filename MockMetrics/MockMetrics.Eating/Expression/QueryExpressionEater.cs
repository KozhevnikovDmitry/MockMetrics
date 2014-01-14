using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class QueryExpressionEater : ExpressionEater<IQueryExpression>
    {
        private readonly IMetricHelper _metricHelper;

        public QueryExpressionEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IQueryExpression expression)
        {
            return Variable.None;
            //var fromMetrics = Eater.Eat(snapshot, expression.From.Expression);
            //snapshot.AddVariable(expression.From.Declaration, Metrics.Create(Scope.Local));

            //IQuerySelectClause lastSelect;
            //foreach (var queryClause in expression.Clauses)
            //{
            //    Eater.Eat(snapshot, queryClause);
            //}

            //lastSelect = expression.Clauses.Last() as IQuerySelectClause;

            //foreach (var queryContinuation in expression.Continuations)
            //{
            //    snapshot.AddVariable(queryContinuation.Declaration, Metrics.Create(Scope.Local));

            //    foreach (var queryClause in queryContinuation.Clauses)
            //    {
            //        Eater.Eat(snapshot, queryClause);
            //    }

            //    lastSelect = queryContinuation.Clauses.Last() as IQuerySelectClause;
            //}

            //// the final query kind is provided based on type of last select clause
            //// so if it return stubs(for example), all query returns stubs
            //return Eater.Eat(snapshot, lastSelect);
        }
    }
}
