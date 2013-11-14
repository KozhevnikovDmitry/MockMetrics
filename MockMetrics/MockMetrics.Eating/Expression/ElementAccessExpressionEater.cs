using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ElementAccessExpressionEater : ExpressionEater<IElementAccessExpression>
    {
        public ElementAccessExpressionEater(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IElementAccessExpression expression)
        {
            foreach (ICSharpArgument arg in expression.Arguments)
            {
                ExpressionKind kind = Eater.Eat(snapshot, arg.Value);
                if (kind != ExpressionKind.StubCandidate)
                {
                    snapshot.AddTreeNode(kind, arg);
                }
            }

            // TODO : cover by functional tests
            // TODO : what if array of results or targets
            Eater.Eat(snapshot, expression.Operand);

            return ExpressionKind.StubCandidate;
        }
    }
}
