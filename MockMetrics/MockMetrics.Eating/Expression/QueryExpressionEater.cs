using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class QueryExpressionEater : ExpressionEater<IQueryExpression>
    {
        private readonly ITypeHelper _typeHelper;

        public QueryExpressionEater(IEater eater, ITypeHelper typeHelper) : base(eater)
        {
            _typeHelper = typeHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IQueryExpression expression)
        {
            var varType = Eater.Eat(snapshot, expression.From.Expression);
            snapshot.AddVariable(expression.From.Declaration, Scope.Local,  , varType);

            IQuerySelectClause lastSelect;
            foreach (var queryClause in expression.Clauses)
            {
                Eater.Eat(snapshot, queryClause);
            }

            lastSelect = expression.Clauses.Last() as IQuerySelectClause;

            foreach (var queryContinuation in expression.Continuations)
            {
               snapshot.AddVariable(queryContinuation.Declaration, Scope.Local, , );

                foreach (var queryClause in queryContinuation.Clauses)
                {
                    Eater.Eat(snapshot, queryClause);
                }

                lastSelect = queryContinuation.Clauses.Last() as IQuerySelectClause;
            }

            // the final query kind is provided based on type of last select clause
            // so if it return stubs(for example), all query returns stubs
            return Eater.Eat(snapshot, lastSelect);
        }
    }
}
