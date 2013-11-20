using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class TryCatchStatementEater : StatementEater<ITryStatement>
    {
        public TryCatchStatementEater(IEater eater) : base(eater)
        {

        }

        public override void Eat(ISnapshot snapshot, ITryStatement statement)
        {
            Eater.Eat(snapshot, statement.Try);

            foreach (var catchClause in statement.Catches)
            {
                Eater.Eat(snapshot, catchClause.Body);
                if (catchClause is ISpecificCatchClause)
                {
                    Eater.Eat(snapshot, (catchClause as ISpecificCatchClause).ExceptionDeclaration);
                }
                
            }

            if (statement.FinallyBlock != null)
            {
                Eater.Eat(snapshot, statement.FinallyBlock);
            }


            
        }
    }
}
