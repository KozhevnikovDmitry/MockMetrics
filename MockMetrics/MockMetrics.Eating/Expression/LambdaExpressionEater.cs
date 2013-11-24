using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class LambdaExpressionEater : ExpressionEater<ILambdaExpression>
    {
        public LambdaExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, ILambdaExpression expression, bool innerEat)
        {
            foreach (var anonymousMethodParameterDeclaration in expression.ParameterDeclarations)
            {
                Eater.Eat(snapshot, anonymousMethodParameterDeclaration);
            }

            if (expression.BodyBlock != null)
            {
                Eater.Eat(snapshot, expression.BodyBlock);
            }
            else
            {
                Eater.Eat(snapshot, expression.BodyExpression, innerEat);
            }

            return ExpressionKind.StubCandidate;
        }
    }
}
