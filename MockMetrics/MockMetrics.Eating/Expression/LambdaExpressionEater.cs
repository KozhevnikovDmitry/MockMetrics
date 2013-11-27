using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class LambdaExpressionEater : ExpressionEater<ILambdaExpression>
    {
        public LambdaExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, ILambdaExpression expression)
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
                Eater.Eat(snapshot, expression.BodyExpression);
            }

            return VarType.Internal;
        }
    }
}
