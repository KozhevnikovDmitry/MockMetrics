using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
	public class IfStatementEater : StatementEater<IIfStatement>
	{
		public IfStatementEater(IEater eater)
			: base(eater)
		{			
		}

		public override void Eat(ISnapshot snapshot, IIfStatement statement)
		{
			Eater.Eat(snapshot, statement.Then);
			if (statement.Else != null)
			{
				Eater.Eat(snapshot, statement.Else);
			}			

			Eater.Eat(snapshot, statement.Condition);	
		}
	}
}
