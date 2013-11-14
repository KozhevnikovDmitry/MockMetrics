using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class LambdaParameterDeclarationEaterTests
    {
        [Test]
        public void AddLamdaParameterToSnapshotAsVariableTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var lambdaParameterDeclaration = Mock.Of<ILambdaParameterDeclaration>();
            var eater = Mock.Of<IEater>();
            var lambdaParameterDeclarationEater = new LambdaParameterDeclarationEater(eater);

            // Act
            lambdaParameterDeclarationEater.Eat(snapshot.Object, lambdaParameterDeclaration);

            // Assert
            snapshot.Verify(t => t.AddVariable(lambdaParameterDeclaration), Times.Once);
        }
    }
}