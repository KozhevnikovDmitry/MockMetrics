using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class YieldStatementEater : StatementEater<IYieldStatement>
    {
        public YieldStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IYieldStatement statement)
        {
            Eater.Eat(snapshot, statement.Expression);
        }
    }
}