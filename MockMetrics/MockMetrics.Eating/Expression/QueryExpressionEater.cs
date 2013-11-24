using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class QueryExpressionEater : ExpressionEater<IQueryExpression>
    {
        private readonly ITypeEater _typeEater;

        public QueryExpressionEater(IEater eater, ITypeEater typeEater) : base(eater)
        {
            _typeEater = typeEater;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IQueryExpression expression, bool innerEat)
        {
           
            Eater.Eat(snapshot, expression.From.Expression, innerEat);
            snapshot.Add(expression.From.Declaration);

            IQuerySelectClause lastSelect;
            foreach (var queryClause in expression.Clauses)
            {
                Eater.Eat(snapshot, queryClause);
            }

            lastSelect = expression.Clauses.Last() as IQuerySelectClause;

            foreach (var queryContinuation in expression.Continuations)
            {
                snapshot.Add(queryContinuation.Declaration);

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
