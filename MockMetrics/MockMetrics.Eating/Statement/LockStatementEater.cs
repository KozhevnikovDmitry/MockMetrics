using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class LockStatementEater : StatementEater<ILockStatement>
    {
        public LockStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ILockStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);

            Eater.Eat(snapshot, statement.Monitor);
        }
    }
}