using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class UnsafeCodeFixedStatementEaterTests
    {
        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<ICSharpStatement>();
            var unsafeCodeFixedStatement = Mock.Of<IUnsafeCodeFixedStatement>(t => t.Body == body);
            Mock.Get(unsafeCodeFixedStatement)
                .Setup(t => t.PointerDeclarations)
                .Returns(new TreeNodeCollection<IUnsafeCodeFixedPointerDeclaration>(new IUnsafeCodeFixedPointerDeclaration[0]));
            var eater = new Mock<IEater>();
            var unsafeCodeFixedStatementEater = new UnsafeCodeFixedStatementEater(eater.Object);

            // Act
            unsafeCodeFixedStatementEater.Eat(snapshot, unsafeCodeFixedStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }

        [Test]
        public void EatPointerDeclarationsTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var unsafeCodeFixedPointerDeclaration = Mock.Of<IUnsafeCodeFixedPointerDeclaration>();
            var unsafeCodeFixedStatement = Mock.Of<IUnsafeCodeFixedStatement>();
            Mock.Get(unsafeCodeFixedStatement)
                .Setup(t => t.PointerDeclarations)
                .Returns(new TreeNodeCollection<IUnsafeCodeFixedPointerDeclaration>(new[] { unsafeCodeFixedPointerDeclaration }));

            var eater = new Mock<IEater>();
            var unsafeCodeFixedStatementEater = new UnsafeCodeFixedStatementEater(eater.Object);

            // Act
            unsafeCodeFixedStatementEater.Eat(snapshot, unsafeCodeFixedStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, unsafeCodeFixedPointerDeclaration), Times.Once);
        }
    }
}