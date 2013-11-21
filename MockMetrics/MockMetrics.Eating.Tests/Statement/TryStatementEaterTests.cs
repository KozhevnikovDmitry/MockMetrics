using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
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
			var catchClause = Mock.Of<ICatchClause>();
			var tryStatement = Mock.Of<ITryStatement>();
			Mock.Get(tryStatement)
				.Setup(t => t.Catches)
				.Returns(new TreeNodeCollection<ICatchClause>(new[] {catchClause}));

			var tryStatementEater = new TryStatementEater(eater.Object);

			// Act
			tryStatementEater.Eat(snapshot, tryStatement);

			// Assert
			eater.Verify(t => t.Eat(snapshot, catchClause.Body), Times.Once);
		}

		[Test]
		public void EatSpecificCatchTest()
		{
			// Arrange
			var snapshot = Mock.Of<ISnapshot>();
			var eater = new Mock<IEater>();
			var catchClause = Mock.Of<ISpecificCatchClause>();
			var tryStatement = Mock.Of<ITryStatement>();
			Mock.Get(tryStatement)
				.Setup(t => t.Catches)
				.Returns(new TreeNodeCollection<ICatchClause>(new[] { catchClause }));

			var tryStatementEater = new TryStatementEater(eater.Object);

			// Act
			tryStatementEater.Eat(snapshot, tryStatement);

			// Assert
			eater.Verify(t => t.Eat(snapshot, catchClause.ExceptionDeclaration), Times.Once);
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
