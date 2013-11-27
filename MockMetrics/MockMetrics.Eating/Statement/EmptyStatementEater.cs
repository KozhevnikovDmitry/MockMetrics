using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class EmptyStatementEater : StatementEater<IEmptyStatement>
    {
        public EmptyStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IEmptyStatement statement)
        {
            
        }
    }
}
