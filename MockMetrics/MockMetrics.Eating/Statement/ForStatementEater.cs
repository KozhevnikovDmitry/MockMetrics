using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class ForStatementEater : StatementEater<IForStatement>
    {
        public ForStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IForStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);

            var condKind = Eater.Eat(snapshot, statement.Condition);
            snapshot.Add(condKind, statement.Condition);

            foreach (var initializer in statement.Initializer.Expressions)
            {
                var kind = Eater.Eat(snapshot, initializer);
                snapshot.Add(kind, initializer);
            }

            foreach (var iterator in statement.Iterators.Expressions)
            {
                var kind = Eater.Eat(snapshot, iterator);
                snapshot.Add(kind, iterator);
            }
        }
    }
}
