using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class ReturnStatementEater : StatementEater<IReturnStatement>
    {
        public ReturnStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IReturnStatement statement)
        {
           Eater.Eat(snapshot, statement.Value);
        }
    }
}