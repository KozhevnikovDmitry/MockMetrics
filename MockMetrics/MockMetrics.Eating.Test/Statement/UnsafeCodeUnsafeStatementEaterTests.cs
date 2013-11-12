using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Test.Statement
{
    [TestFixture]
    public class UnsafeCodeUnsafeStatementEaterTests
    {
        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<IBlock>();
            var unsafeCodeUnsafeStatement = Mock.Of<IUnsafeCodeUnsafeStatement>(t => t.Body == body);
            var eater = new Mock<IEater>();
            var unsafeCodeUnsafeStatementEater = new UnsafeCodeUnsafeStatementEater(eater.Object);

            // Act
            unsafeCodeUnsafeStatementEater.Eat(snapshot, unsafeCodeUnsafeStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }
    }
}