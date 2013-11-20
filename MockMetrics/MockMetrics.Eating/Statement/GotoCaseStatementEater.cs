using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.Statement
{
    public class GotoCaseStatementEater : StatementEater<IGotoCaseStatement>
    {
        public GotoCaseStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IGotoCaseStatement statement)
        {
            ExpressionKind kind = Eater.Eat(snapshot, statement.ValueExpression);
            snapshot.Add(kind, statement.ValueExpression);
        }
    }
}