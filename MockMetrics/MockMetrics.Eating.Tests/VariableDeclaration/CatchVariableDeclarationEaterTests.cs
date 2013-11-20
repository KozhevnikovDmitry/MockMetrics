using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class CatchVariableDeclarationEaterTests
    {
        [Test]
        public void AddCatchVariableToSnapshotAsVariableTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var catchVariableDeclaration = Mock.Of<ICatchVariableDeclaration>();
            var eater = Mock.Of<IEater>();
            var catchVariableDeclarationEater = new CatchVariableDeclarationEater(eater);

            // Act
            catchVariableDeclarationEater.Eat(snapshot.Object, catchVariableDeclaration);

            // Assert
            snapshot.Verify(t => t.Add(catchVariableDeclaration), Times.Once);
        }
    }
}