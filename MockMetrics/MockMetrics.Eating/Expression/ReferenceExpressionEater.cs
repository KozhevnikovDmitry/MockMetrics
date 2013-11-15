using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        public ReferenceExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            if (expression.QualifierExpression != null)
            {
                var parentKind = Eater.Eat(snapshot, expression.QualifierExpression);
            }

            return ExpressionKind.None;
        }
    }
}