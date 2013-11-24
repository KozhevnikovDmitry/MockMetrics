using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.VariableDeclaration;

namespace MockMetrics.Eating.Expression
{
    public class ArrayCreationExpressionEater : ExpressionEater<IArrayCreationExpression>
    {
        private readonly IVariableInitializerEater _variableInitializerEater;

        public ArrayCreationExpressionEater(IEater eater, IVariableInitializerEater variableInitializerEater) : base(eater)
        {
            _variableInitializerEater = variableInitializerEater;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IArrayCreationExpression expression, bool innerEat)
        {
            // TODO : check in functional tests
            foreach (ICSharpExpression size in expression.Sizes)
            {
                ExpressionKind kind = Eater.Eat(snapshot, size, innerEat);
                if (kind != ExpressionKind.StubCandidate)
                {
                    snapshot.Add(kind, size);
                }
            }

            _variableInitializerEater.Eat(snapshot, expression.ArrayInitializer);

            return ExpressionKind.StubCandidate;
        }
    }
}
