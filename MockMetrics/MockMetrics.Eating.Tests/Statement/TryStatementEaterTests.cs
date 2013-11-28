using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
	[TestFixture]
	public class TryStatementEaterTests
	{
		[Test]
		public void EatTryTest()
		{
			// Arrange
			var snapshot = Mock.Of<ISnapshot>();
			var eater = new Mock<IEater>();
			var tryBlock = Mock.Of<IBlock>();
			var tryStatement = Mock.Of<ITryStatement>(t => t.Try == tryBlock);
			Mock.Get(tryStatement)
				.Setup(t => t.Catches)
				.Returns(new TreeNodeCollection<ICatchClause>(new ICatchClause[0]));

			var tryStatementEater = new TryStatementEater(eater.Object);

			// Act
			tryStatementEater.Eat(snapshot, tryStatement);

			// Assert
			eater.Verify(t => t.Eat(snapshot, tryBlock), Times.Once);
		}

		[Test]
		public void EatCatchTest()
		{
			// Arrange
			var snapshot = Mock.Of<ISnapshot>();
			var eater = new Mock<IEater>();
		    var body = Mock.Of<IBlock>();
			var catchClause = Mock.Of<ICatchClause>(t => t.Body == body);
			var tryStatement = Mock.Of<ITryStatement>();
			Mock.Get(tryStatement)
				.Setup(t => t.Catches)
				.Returns(new TreeNodeCollection<ICatchClause>(new[] {catchClause }));

			var tryStatementEater = new TryStatementEater(eater.Object);

			// Act
			tryStatementEater.Eat(snapshot, tryStatement);

			// Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
		}

		[Test]
		public void EatSpecificCatchTest()
		{
			// Arrange
			var snapshot = Mock.Of<ISnapshot>();
			var eater = new Mock<IEater>();
		    var varDeclaration = Mock.Of<ICatchVariableDeclaration>();
			var catchClause = Mock.Of<ISpecificCatchClause>(t => t.ExceptionDeclaration == varDeclaration);
			var tryStatement = Mock.Of<ITryStatement>();
			Mock.Get(tryStatement)
				.Setup(t => t.Catches)
				.Returns(new TreeNodeCollection<ICatchClause>(new[] { catchClause }));

			var tryStatementEater = new TryStatementEater(eater.Object);

			// Act
			tryStatementEater.Eat(snapshot, tryStatement);

			// Assert
            eater.Verify(t => t.Eat(snapshot, varDeclaration), Times.Once);
		}

		[Test]
		public void EatFinallyTest()
		{
			// Arrange
			var snapshot = Mock.Of<ISnapshot>();
			var eater = new Mock<IEater>();
			var finallyBlock = Mock.Of<IBlock>();
			var tryStatement = Mock.Of<ITryStatement>(t => t.FinallyBlock == finallyBlock);
			Mock.Get(tryStatement)
				.Setup(t => t.Catches)
				.Returns(new TreeNodeCollection<ICatchClause>(new ICatchClause[0]));

			var tryStatementEater = new TryStatementEater(eater.Object);

			// Act
			tryStatementEater.Eat(snapshot, tryStatement);

			// Assert
			eater.Verify(t => t.Eat(snapshot, finallyBlock), Times.Once);
		}

	}
}
