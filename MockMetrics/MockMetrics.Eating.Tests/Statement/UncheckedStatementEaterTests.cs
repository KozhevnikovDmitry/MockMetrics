using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class UncheckedStatementEaterTests
    {
        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<IBlock>();
            var uncheckedStatement = Mock.Of<IUncheckedStatement>(t => t.Body == body);
            var eater = new Mock<IEater>();
            var uncheckedStatementEater = new UncheckedStatementEater(eater.Object);

            // Act
            uncheckedStatementEater.Eat(snapshot, uncheckedStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }
    }
}