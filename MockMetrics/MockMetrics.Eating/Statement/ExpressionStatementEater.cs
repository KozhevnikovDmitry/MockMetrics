using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class ExpressionStatementEater : StatementEater<IExpressionStatement>
    {
        public ExpressionStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IExpressionStatement statement)
        {
            Eater.Eat(snapshot, statement.Expression);
        }
    }
}