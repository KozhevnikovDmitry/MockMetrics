using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class RegularParameterDeclarationEaterTests
    {
        [Test]
        public void AddParamterToSnapshotAsStubTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var regularParameterDeclaration = Mock.Of<IRegularParameterDeclaration>();
            var eater = Mock.Of<IEater>();
            var regularParameterDeclarationEater = new RegularParameterDeclarationEater(eater);

            // Act
            regularParameterDeclarationEater.Eat(snapshot.Object, regularParameterDeclaration);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.Stub, regularParameterDeclaration), Times.Once);
        }
    }
}