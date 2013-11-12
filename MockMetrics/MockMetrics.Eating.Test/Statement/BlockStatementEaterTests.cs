using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Test.Statement
{
    [TestFixture]
    public class BlockStatementEaterTests
    {
        [Test]
        public void EatContainedStatementsTest()
        {
            // Arrange
            var firstStatement = Mock.Of<ICSharpStatement>();
            var secondStatement = Mock.Of<ICSharpStatement>();
            var block = Mock.Of<IBlock>();
            Mock.Get(block)
                .Setup(t => t.Statements)
                .Returns(new TreeNodeCollection<ICSharpStatement>(new[] {firstStatement, secondStatement}));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var blockEater = new BlockStatementEater(eater.Object);

            // Act
            blockEater.Eat(snapshot, block);

            // Assert
            eater.Verify(t => t.Eat(snapshot, firstStatement), Times.Once);
            eater.Verify(t => t.Eat(snapshot, secondStatement), Times.Once);
        }
    }
}
