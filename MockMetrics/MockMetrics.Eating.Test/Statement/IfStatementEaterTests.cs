using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Test.Statement
{
	[TestFixture]
	public class IfStatementEaterTests
	{
		[Test]
		public void EatThenTest()
		{
			// Arrange
			var snapshot = Mock.Of<ISnapshot>();
			var body = Mock.Of<ICSharpStatement>();
			var ifStatement = Mock.Of<IIfStatement>(t => t.Then == body);
			var eater = new Mock<IEater>();
			var ifStatementEater = new IfStatementEater(eater.Object);

			// Act
			ifStatementEater.Eat(snapshot, ifStatement);

			// Assert
			eater.Verify(t => t.Eat(snapshot, body), Times.Once);
		}

		[Test]
		public void EatElseTest()
		{
			// Arrange
			var snapshot = Mock.Of<ISnapshot>();
			var body = Mock.Of<ICSharpStatement>();
			var ifStatement = Mock.Of<IIfStatement>(t => t.Else == body);
			var eater = new Mock<IEater>();
			var ifStatementEater = new IfStatementEater(eater.Object);

			// Act
			ifStatementEater.Eat(snapshot, ifStatement);

			// Assert
			eater.Verify(t => t.Eat(snapshot, body), Times.Once);
		}

		[Test]
		public void EatConditionTest()
		{
			// Arrange
			var snapshot = Mock.Of<ISnapshot>();
			var condition = Mock.Of<ICSharpExpression>();
			var ifStatement = Mock.Of<IIfStatement>(t => t.Condition == condition);
			var eater = new Mock<IEater>();
			var ifStatementEater = new IfStatementEater(eater.Object);

			// Act
			ifStatementEater.Eat(snapshot, ifStatement);

			// Assert
			eater.Verify(t => t.Eat(snapshot, condition), Times.Once);
		}

		[Test]
		public void AddConditionToSnapshotTest()
		{
			// Arrange
			var snapshot = new Mock<ISnapshot>();
			var condition = Mock.Of<ICSharpExpression>();
			var ifStatement = Mock.Of<IIfStatement>(t => t.Condition == condition);

			var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, condition) == ExpressionKind.None);
			var ifStatementEater = new IfStatementEater(eater);

			// Act
			ifStatementEater.Eat(snapshot.Object, ifStatement);

			// Assert
			snapshot.Verify(t => t.AddTreeNode(ExpressionKind.None, condition), Times.Once);
		} 
	}
}
