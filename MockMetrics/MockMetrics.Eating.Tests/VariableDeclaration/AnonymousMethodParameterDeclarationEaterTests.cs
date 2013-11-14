using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class AnonymousMethodParameterDeclarationEaterTests
    {
        [Test]
        public void AddAnonymousMethodParameterToSnapshotAsVariableTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var anonymousParameterDeclaration = Mock.Of<IAnonymousMethodParameterDeclaration>();
            var eater = Mock.Of<IEater>();
            var anonymousParameterDeclarationEater = new AnonymousMethodParameterDeclarationEater(eater);

            // Act
            anonymousParameterDeclarationEater.Eat(snapshot.Object, anonymousParameterDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(anonymousParameterDeclaration), Times.Once);
        }
    }
}