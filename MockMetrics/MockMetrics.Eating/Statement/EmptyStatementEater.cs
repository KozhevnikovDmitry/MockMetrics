using JetBrains.ReSharper.Psi.CSharp.Tree;

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
