using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Statement
{
	[TestFixture]
	public class IfStatementEaterTests
	{
		[Test]
		public void EatThenTest()
		{
			// Arrange
			var snapshot = Mock.Of<ISnapshot>();
			var thenBlock = Mock.Of<ICSharpStatement>();
			var ifStatement = Mock.Of<IIfStatement>(t => t.Then == thenBlock);
			var eater = new Mock<IEater>();
			var ifStatementEater = new IfStatementEater(eater.Object);

			// Act
			ifStatementEater.Eat(snapshot, ifStatement);

			// Assert
			eater.Verify(t => t.Eat(snapshot, thenBlock), Times.Once);
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
        public void NotEatNullElseTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var thenBlock = Mock.Of<ICSharpStatement>();
            var ifStatement = Mock.Of<IIfStatement>(t => t.Then == thenBlock);
            var eater = new Mock<IEater>();
            var ifStatementEater = new IfStatementEater(eater.Object);

            // Act
            ifStatementEater.Eat(snapshot, ifStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, It.Is<ICSharpStatement>(c => !c.Equals(thenBlock))), Times.Never);
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
	}
}
